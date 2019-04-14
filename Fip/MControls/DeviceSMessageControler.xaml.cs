using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fip.Dialog;
using Fip.Code.DB;

namespace Fip.MControls
{
    /// <summary>
    /// DeviceSMessageControler.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceSMessageControler : UserControl
    {
        private DeviceShortMessageLine NowUseDevice = null;

        public DeviceSMessageControler()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(StandardDeviceDesModel model in DBControler.UnityIns.GetSSDesAllShortRecord())
            {
                AddDSMessage(model.Id, model.EquType , model.EquCode);
            }
        }

        /// <summary>
        /// 添加器件信息(显示部分，不包含数据库操作)
        /// </summary>
        /// <param name="id">信息Id</param>
        private void AddDSMessage(int id , String equType , String equCode)
        {
            DeviceShortMessageLine line = new DeviceShortMessageLine(id, equType, equCode,(DeviceShortMessageLine which) =>
            {
                //之前的器件取消选择状态
                if (NowUseDevice != null)
                {
                    NowUseDevice.Selected_Cancel();
                }

                NowUseDevice = which;
            });

            DeviceSMesageContainer.Children.Add(line);
        }

        private void SearchButton_Click()
        {
            //全部转化为小写
            String message = SearchTB.Text.Trim().ToLower();

            if(String.IsNullOrWhiteSpace(message) || String.IsNullOrEmpty(message))
            {
                foreach(DeviceShortMessageLine line in DeviceSMesageContainer.Children)
                {
                    line.Visibility = Visibility.Visible;
                }
            }
            else
            {
                foreach (DeviceShortMessageLine line in DeviceSMesageContainer.Children)
                {
                    //如果包含搜索信息，则可见
                    if(line.GetEquType().ToLower().Contains(message))
                    {
                        line.Visibility = Visibility.Visible;
                    }
                    //如果不包含搜索信息，则不可见
                    else
                    {
                        line.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void AddNewDeviceButton_Click()
        {
            NewDeviceDialog dialog = new NewDeviceDialog();
            dialog.Owner = MainWindow.MWindow;

            //完成输入工作，成功添加了
            if(dialog.ShowDialog().Value)
            {
                //获取数据
                StandardDeviceDesModel result = dialog.GetResult();

                int repetitionId = 0;
                //查重,如果出现了型号和编号重复
                if (DBControler.UnityIns.CheckSSDesRepetition(result.EquCode, result.EquType, out repetitionId))
                {
                    //询问是否覆盖数据
                    MessageDialog mDialog = new MessageDialog("数据库中已存在该器件\n(油泵编号和型号完全相同)\n是否覆盖数据?" , true);
                    
                    //无需覆盖，直接返回
                    if (!mDialog.ShowDialog().Value)
                    {
                        return;
                    }
                    //覆盖数据
                    else
                    {
                        result.Id = repetitionId;

                        //寻找相同数据
                        foreach (DeviceShortMessageLine line in DeviceSMesageContainer.Children)
                        {
                            //出现了重复，则是覆盖了之前的内容
                            if (line.GetID() == result.Id)
                            {
                                //在这里需要检查该器件是否正在被使用，如果是，则无法修改
                                if (!line.IsTesting())
                                {
                                    //可以修改数据
                                    //修改成功
                                    if (DBControler.UnityIns.ModifySSDesRecord(result))
                                    {
                                        BottomPart.Log(String.Format("覆盖数据成功(<编号:{0}><型号:{1}>)", result.EquCode, result.EquType), LogMessage.LevelEnum.Normal);
                                    }
                                    //修改失败
                                    else
                                    {
                                        BottomPart.Log(String.Format("覆盖数据失败(<编号:{0}><型号:{1}>)", result.EquCode, result.EquType), LogMessage.LevelEnum.Error);
                                        return;
                                    }

                                    line.ModifyEquType(result.EquType, result.EquCode);
                                    return;
                                }
                                //器件正在被使用，故而无法修改数据
                                else
                                {
                                    MessageDialog errorDialog = new MessageDialog("该器件信息正在被使用，无法修改!");
                                    if(errorDialog.ShowDialog().Value)
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                //没有重复的编号和型号
                else
                {
                    int newId = DBControler.UnityIns.AddSSDesRecord(result);
                    //插入失败
                    if (newId < 0)
                    {
                        BottomPart.Log(String.Format("添加新器件信息失败"), LogMessage.LevelEnum.Error);
                        return;
                    }
                    else
                    {
                        //保存获得的id
                        result.Id = newId;
                        BottomPart.Log(String.Format("添加新器件信息成功(<编号:{0}><型号:{1}>)", result.EquCode, result.EquType), LogMessage.LevelEnum.Normal);
                        
                        //新添加的器件信息
                        DeviceShortMessageLine newLine = new DeviceShortMessageLine(result.Id, result.EquType, result.EquCode,(DeviceShortMessageLine which) =>
                        {
                            //之前的器件取消选择状态
                            if (NowUseDevice != null)
                            {
                                NowUseDevice.Selected_Cancel();
                            }

                            NowUseDevice = which;
                        });
                        DeviceSMesageContainer.Children.Add(newLine);
                    }
                }
            }
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            //全部转化为小写
            String message = SearchTB.Text.Trim().ToLower();

            if (String.IsNullOrWhiteSpace(message) || String.IsNullOrEmpty(message))
            {
                foreach (DeviceShortMessageLine line in DeviceSMesageContainer.Children)
                {
                    line.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
