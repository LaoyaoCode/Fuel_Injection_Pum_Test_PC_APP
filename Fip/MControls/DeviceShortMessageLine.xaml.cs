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
    /// DeviceShortMessageLine.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceShortMessageLine : UserControl
    {
        /// <summary>
        /// 是否已经选择这个器件
        /// </summary>
        private bool IsSelected = false;
        /// <summary>
        /// 是否正在测试
        /// </summary>
        private bool Testing = false;
        /// <summary>
        /// 器件ID
        /// </summary>
        private int DeviceID = 0;

        private String EquCode = String.Empty;
        private String EquType = String.Empty;

        /// <summary>
        /// 被选择代理事件
        /// </summary>
        /// <param name="line"></param>
        public delegate void SelectDel(DeviceShortMessageLine line);

        private SelectDel SelectEvent = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">器件id</param>
        /// <param name="equType">油泵型号</param>
        /// <param name="equCode">喷油泵编码</param>
        /// <param name="del">被点击选择代理事件</param>
        public DeviceShortMessageLine(int id ,String equType ,String equCode, SelectDel del = null)
        {
            InitializeComponent();
            DeviceID = id;

            SelectEvent = del;
            DeviceName.Text = equType;
            InitialTB.Text = equCode.ToCharArray()[0].ToString();

            EquCode = equCode;
            EquType = equType;
        }

        /// <summary>
        /// 获取器件的型号
        /// </summary>
        /// <returns></returns>
        public String GetEquType()
        {
            return EquType;
        }

        private void UserControl_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsSelected = true;
            //执行代理事件
            if (SelectEvent != null)
            {
                SelectEvent.Invoke(this);
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            //只有在没有测试的时候才可以删除和修改
            if (!Testing)
            {
                ModifyButton.Visibility = Visibility.Visible;
                RemoveButton.Visibility = Visibility.Visible;
            }
            DetailButton.Visibility = Visibility.Visible;

            //改变背景颜色
            RootGrid.Background = (SolidColorBrush)FindResource("SelectedColor");
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            ModifyButton.Visibility = Visibility.Collapsed;
            RemoveButton.Visibility = Visibility.Collapsed;
            DetailButton.Visibility = Visibility.Collapsed;

            //只有没有被选中的时候才会变回正常颜色
            if (!IsSelected)
            {
                RootGrid.Background = (SolidColorBrush)FindResource("AppBackBrush");
            }
        }

        /// <summary>
        /// 离开被选中状态
        /// </summary>
        public void Selected_Cancel()
        {
            IsSelected = false;
            //恢复背景
            RootGrid.Background = (SolidColorBrush)FindResource("AppBackBrush");
        }

        /// <summary>
        /// 离开正在测试状态
        /// </summary>
        public void Testing_Cancel()
        {
            Testing = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// 获取短器件的ID
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return DeviceID;
        }

        /// <summary>
        /// 修改喷油泵型号
        /// </summary>
        /// <param name="equType">喷油泵型号</param>
        /// <param name="equCode">喷油泵编码</param>
        public void ModifyEquType(String equType , String equCode)
        {
            DeviceName.Text = equType;
            InitialTB.Text = equCode.ToCharArray()[0].ToString();
        }
        
        /// <summary>
        /// 是否正在使用这个器件信息来测试
        /// </summary>
        /// <returns></returns>
        public bool IsTesting()
        {
            return Testing;
        }

        private void SeeDetailButton_Click()
        {
            DeviceDetailsWindow detail = new DeviceDetailsWindow(DeviceID);
            detail.Show();
        }

        private void ModifyButton_Click()
        {
            //正在测试，无法删除该器件
            if (Testing)
            {
                MessageDialog dialogForError = new MessageDialog("该器件信息正在被使用无法修改");
                return;
            }

            StandardDeviceDesModel model = DBControler.UnityIns.GetSSDesTotalRecord(DeviceID);
            if(model != null)
            {
                NewDeviceDialog newDDialog = new NewDeviceDialog(model, true);
                if(newDDialog.ShowDialog().Value)
                {
                    MessageDialog sureDialog = new MessageDialog(String.Format("是否确认修改该标准器件测试信息?\n喷油泵型号 : {0}\n喷油泵编号 : {1}", EquType, EquCode), true);

                    //确认修改
                    if (sureDialog.ShowDialog().Value)
                    {
                        StandardDeviceDesModel result = newDDialog.GetResult();
                        result.Id = DeviceID;

                        if (DBControler.UnityIns.ModifySSDesRecord(result))
                        {
                            BottomPart.Log("成功修改", LogMessage.LevelEnum.Important);
                        }
                        else
                        {
                            BottomPart.Log("修改失败", LogMessage.LevelEnum.Error);
                        }
                    }
                    
                }
            }
        }

        private void DeleteButton_Click()
        {
            //正在测试，无法删除该器件
            if(Testing)
            {
                MessageDialog dialogForError = new MessageDialog("该器件信息正在被使用无法删除");
                return;
            }

            MessageDialog dialog = new MessageDialog(String.Format("是否确认删除该标准器件测试信息?\n喷油泵型号 : {0}\n喷油泵编号 : {1}" , EquType ,EquCode), true);

            //确认删除
            if(dialog.ShowDialog().Value)
            {
                if(DBControler.UnityIns.DeleteSSDesRecord(DeviceID))
                {
                    BottomPart.Log("成功删除", LogMessage.LevelEnum.Important);
                    //移除自己的显示
                    ((StackPanel)(this.Parent)).Children.Remove(this);
                }
                else
                {
                    BottomPart.Log("删除失败", LogMessage.LevelEnum.Important);
                }
            }
        }
    }
}
