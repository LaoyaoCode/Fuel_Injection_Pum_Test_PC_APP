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
        private static BottomPart Unity = null;

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
    }
}
