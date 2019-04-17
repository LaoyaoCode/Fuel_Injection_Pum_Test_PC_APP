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
using Fip.Code.Trans;


namespace Fip.MControls
{
    /// <summary>
    /// BottomPart.xaml 的交互逻辑
    /// </summary>
    public partial class BottomPart : UserControl
    {
        /// <summary>
        /// 唯一实例
        /// </summary>
        public static BottomPart Unity = null;

        private bool _IsConnected = false;

        public BottomPart()
        {
            InitializeComponent();

            Unity = this;
        }

        private void ConnectedIcon_MouseLeftButtonUp()
        {
            DeviceToolTip.IsOpen = false;
            DeviceToolTip.IsOpen = true;
        }

        private void LogIcon_MouseLeftButtonUp()
        {
            LogToolTip.IsOpen = false;
            LogToolTip.IsOpen = true;
        }

        /// <summary>
        /// 所有已阅按钮点击
        /// </summary>
        private void CheckAllButton_Click()
        {
            LogMessageContainer.Children.Clear();
            //直接变成普通level
            LogIcon.ImageSource = "/Fip;component/Images/message_normal.png";
        }

        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="text">日志信息</param>
        /// <param name="level">等级</param>
        public static void Log(String text , LogMessage.LevelEnum level)
        {
            LogMessage log = new LogMessage(text, level, Unity.LogMessageContainer, () =>
           {
               //更新颜色
               Unity.UpdateLogIcon();
           });

            Unity.LogMessageContainer.Children.Add(log);
            //更新颜色
            Unity.UpdateLogIcon();
        }

        /// <summary>   
        /// 更新日志标志，根据内容切换图标
        /// </summary>
        private void UpdateLogIcon()
        {
            int max = 0;

            foreach(LogMessage message in LogMessageContainer.Children)
            {
                if((int)message.GetLevel() > max)
                {
                    max = (int)message.GetLevel();
                }
            }

            switch(max)
            {
                case 0:
                    LogIcon.ImageSource = "/Fip;component/Images/message_normal.png";
                    break;
                case 1:
                    LogIcon.ImageSource = "/Fip;component/Images/message_important.png";
                    break;
                case 2:
                    LogIcon.ImageSource = "/Fip;component/Images/message_error.png";
                    break;
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
        
        /// <summary>
        /// 搜索按钮被点击
        /// </summary>
        private void SearchButton_Click()
        {
            ITrans.UnityIns.Add_LostConnectDel(DeviceLostConnect);
            ITrans.UnityIns.Add_GetMeeageDel(RecieveMessage);

            ITrans.UnityIns.ConnectToDeviceAsync((result , message , type)=>
            {
                //成功连接了，然后等待获取信息

            });
            SearchDeviceButton.IsEnabled = false;
            LoadingIcon_Appear();
        }

        /// <summary>
        /// 加载图标显示
        /// </summary>
        public static void LoadingIcon_Appear()
        {
            Unity.RollingIcon.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 加载图标隐藏
        /// </summary>
        public static void LoadingIcon_Disappear()
        {
            Unity.RollingIcon.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 器件失去了连接
        /// </summary>
        private void DeviceLostConnect()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                _IsConnected = false;
                SearchDeviceButton.Visibility = Visibility.Visible;
                ConnectedIcon.Visibility = Visibility.Collapsed;
                Log("测试台失去连接", LogMessage.LevelEnum.Error);
            }));
           
        }

        /// <summary>
        /// 是否成功连接
        /// </summary>
        /// <returns></returns>
        public bool IsConnect()
        {
            return _IsConnected;
        }

        /// <summary>
        /// 接收到数据
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        /// <param name="command"></param>
        private void RecieveMessage(bool result, String data, TcpIpTrans.CommandEnum command)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                //接受到了连接数据，下位机发送了自身器件数据
                if (command == ITrans.CommandEnum.CONNECT)
                {
                    _IsConnected = true;
                    //解析填装数据
                    String[] message = data.Split('$');
                    DeviceName.Text = message[0];
                    DeviceVender.Text = message[1];
                    DeviceMakeTime.Text = message[2];

                    //加载图标消失
                    LoadingIcon_Disappear();
                    SearchDeviceButton.Visibility = Visibility.Hidden;
                    ConnectedIcon.Visibility = Visibility.Visible;
                    Log("连接测试台成功", LogMessage.LevelEnum.Important);
                }
            }));
            
        }
    }
}
