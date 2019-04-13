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

        private bool IsConnected = false;

        public BottomPart()
        {
            InitializeComponent();

            Unity = this;

            new TcpIpTrans(DeviceLostConnect);
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
            TcpIpTrans.UnityIns.ConnectToDeviceAsync((result , message , type)=>
            {
                IsConnected = true;
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

        }
    }
}
