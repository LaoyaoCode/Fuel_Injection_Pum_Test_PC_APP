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
using Fip.Code.Trans;
using DialogBox = System.Windows.Forms;

namespace Fip.MControls
{
    /// <summary>
    /// DeviceSMessageControler.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceSMessageControler : UserControl
    {
        private DeviceShortMessageLine NowSelectDevice = null;

        private DeviceShortMessageLine NowTestDevice = null;

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
            DeviceShortMessageLine line = new DeviceShortMessageLine(id, equType, equCode, DeviceStartTest,(DeviceShortMessageLine which) =>
            {
                //之前的器件取消选择状态
                if (NowSelectDevice != null)
                {
                    NowSelectDevice.Selected_Cancel();
                }

                NowSelectDevice = which;
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
                //保存数据
                AddNewDeviceDispose(result);
            }
        }

        private void AddNewDeviceDispose(StandardDeviceDesModel result)
        {

            int repetitionId = 0;
            //查重,如果出现了型号和编号重复
            if (DBControler.UnityIns.CheckSSDesRepetition(result.EquCode, result.EquType, out repetitionId))
            {
                //询问是否覆盖数据
                MessageDialog mDialog = new MessageDialog("数据库中已存在该器件\n器件型号 : " + result.EquType + "\n是否覆盖数据?", true);

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
                                if (errorDialog.ShowDialog().Value)
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
                    DeviceShortMessageLine newLine = new DeviceShortMessageLine(result.Id, result.EquType, result.EquCode, DeviceStartTest, (DeviceShortMessageLine which) =>
                    {
                        //之前的器件取消选择状态
                        if (NowSelectDevice != null)
                        {
                            NowSelectDevice.Selected_Cancel();
                        }

                        NowSelectDevice = which;
                    });
                    DeviceSMesageContainer.Children.Add(newLine);
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


        private void DeviceStartTest(DeviceShortMessageLine line)
        {
            //有正在测试的器件，则需要等待
            if(NowTestDevice != null)
            {
                MessageDialog dialog = new MessageDialog("有器件正在测试，请等待......");
                dialog.ShowDialog();
            }
            //如果没有，则可以使用这个器件开始测试
            else
            {
                if(BottomPart.Unity.IsConnect())
                {
                    MessageDialog dialog = new MessageDialog(String.Format("喷油泵型号 : {0}\n是否开始测试？", line.GetEquType()), true);

                    //同意开始测试
                    if (dialog.ShowDialog().Value)
                    {
                        line.SetTesting(true);
                        NowTestDevice = line;
                        StandardDeviceDesModel model = DBControler.UnityIns.GetSSDesTotalRecord(line.GetID());
                        model.Id = line.GetID();

                        //注册接受事件代理
                        ITrans.UnityIns.Add_GetMeeageDel(this.RecieveData);


                        ITrans.UnityIns.SendMeesageAsync(model.ToString(), ITrans.CommandEnum.REQUIRE, (result , data , commmand) =>
                        {
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                if (result)
                                {
                                    BottomPart.Log("发送喷油泵标准测试数据成功", LogMessage.LevelEnum.Important);
                                }
                                else
                                {
                                    BottomPart.Log("发送喷油泵标准测试数据失败", LogMessage.LevelEnum.Important);
                                }
                            }));
                            
                        });
                    }
                }
                //还未连接，则提示
                else
                {
                    MessageDialog dialog = new MessageDialog("测试台未连接，请先连接测试台");
                    dialog.ShowDialog();
                }
               
            }
        }

        /// <summary>
        /// 接受数据代理
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        /// <param name="command"></param>
        private void RecieveData(bool result, String data, ITrans.CommandEnum command)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                //接受到了数据
                if (command == ITrans.CommandEnum.RESULT)
                {
                    HistoryModel model = new HistoryModel(data);

                    if (model.Id == NowTestDevice.GetID())
                    {
                        int index = -1;
                        StandardDeviceDesModel sdd = DBControler.UnityIns.GetSSDesTotalRecord(NowTestDevice.GetID());

                        //测试状态取消
                        NowTestDevice.SetTesting(false);
                        NowTestDevice = null;

                        CombinaPara(sdd, model);

                        BottomPart.Log("接受结果数据成功", LogMessage.LevelEnum.Important);

                        index = DBControler.UnityIns.AddHistoryRecord(model);
                        model.Id = index;

                        if (index > 0)
                        {
                            BottomPart.Log("保存测试数据成功", LogMessage.LevelEnum.Important);
                            MainWindow.MWindow.AddAHistoryLine(model);
                        }
                        else
                        {
                            BottomPart.Log("保存测试数据失败", LogMessage.LevelEnum.Error);
                        }
                    }
                    else
                    {
                        BottomPart.Log("接受结果数据与当前测试数据ID不匹配", LogMessage.LevelEnum.Error);
                    }

                }
            }));
           
           
        }

        /// <summary>
        /// 组合参数
        /// </summary>
        /// <param name="sdd"></param>
        /// <param name="model"></param>
        public static void CombinaPara(StandardDeviceDesModel sdd , HistoryModel model)
        {
            model.AdjWork.S_RotateSpeed = sdd.AdjWork.S_RotateSpeed;
            model.AdjWork.S_InjectionTime = sdd.AdjWork.S_InjectionTime;

            model.DemWork.S_RotateSpeed = sdd.DemWork.S_RotateSpeed;
            model.DemWork.S_InjectionTime = sdd.DemWork.S_InjectionTime;

            model.EquCode = sdd.EquCode;
            model.EquType = sdd.EquType;

            model.HighBreak.S_RotateSpeed = sdd.HighBreak.S_RotateSpeed;
            model.HighBreak.S_InjectionTime = sdd.HighBreak.S_InjectionTime;

            model.IdlingBreak.S_RotateSpeed = sdd.IdlingBreak.S_RotateSpeed;
            model.IdlingBreak.S_InjectionTime = sdd.IdlingBreak.S_InjectionTime;

            model.IdlingWork.S_RotateSpeed = sdd.IdlingWork.S_RotateSpeed;
            model.IdlingWork.S_InjectionTime = sdd.IdlingWork.S_InjectionTime;

            model.ReviseBegin.S_RotateSpeed = sdd.ReviseBegin.S_RotateSpeed;
            model.ReviseBegin.S_InjectionTime = sdd.ReviseBegin.S_InjectionTime;

            model.ReviseEnd.S_RotateSpeed = sdd.ReviseEnd.S_RotateSpeed;
            model.ReviseEnd.S_InjectionTime = sdd.ReviseEnd.S_InjectionTime;

            model.ReviseWork.S_RotateSpeed = sdd.ReviseWork.S_RotateSpeed;
            model.ReviseWork.S_InjectionTime = sdd.ReviseWork.S_InjectionTime;

            model.StartWork.S_RotateSpeed = sdd.StartWork.S_RotateSpeed;
            model.StartWork.S_InjectionTime = sdd.StartWork.S_InjectionTime;

            //设置时间
            model.HTime = DateTime.Now.ToShortTimeString();
            model.HDate = DateTime.Now.ToShortDateString();

            //测试数据是否符合要求
            model.IsPass = sdd.IsMatchRequire(model);
        }

        private void ImportAllButton_Click()
        {
            string[] filePath;
            DialogBox.OpenFileDialog dialog = new DialogBox.OpenFileDialog();
            dialog.Title = "选择导出文件";
            dialog.Filter = "xml File(*.xml)|*.xml";
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //文件路径
                filePath = dialog.FileNames;

                for(int counter = 0; counter <filePath.Length; counter++)
                {
                    List<StandardDeviceDesModel> lists = null;

                    try
                    {
                        lists = SDDAndXml.GetSDDFromXml(filePath[counter]);
                    }
                    catch(Exception e)
                    {
                        BottomPart.Log("文件解析失败，文件名 : " + filePath[counter] , LogMessage.LevelEnum.Error);
                        continue;
                    }

                    for(int index = 0; index < lists.Count;index++)
                    {
                        AddNewDeviceDispose(lists[index]);
                    }
                }
            }
               
        }


        private void ExportAllButton_Click()
        {
            DialogBox.SaveFileDialog dialog = new DialogBox.SaveFileDialog();
            string filePath;
            List<int> indexs = new List<int>();

            foreach (DeviceShortMessageLine line in DeviceSMesageContainer.Children)
            {
                indexs.Add(line.GetID());
            }

            dialog.Title = "选择导出文件保存路径";
            dialog.Filter = "xml File(*.xml)|*.xml";
            dialog.DefaultExt = "xml";
            //如果用户没有添加则自动添加扩展
            dialog.AddExtension = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //文件路径
                filePath = dialog.FileName;
                List<StandardDeviceDesModel> list = new List<StandardDeviceDesModel>();

                for(int counter = 0; counter < indexs.Count; counter++)
                {
                    list.Add(DBControler.UnityIns.GetSSDesTotalRecord(indexs[counter]));
                }
                

                SDDAndXml.SaveToXml(filePath, list);
                BottomPart.Log("导出所有器件信息成功", LogMessage.LevelEnum.Important);
            }
        }


    }
}
