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
    /// LogMessage.xaml 的交互逻辑
    /// </summary>
    public partial class LogMessage : UserControl
    {
        /// <summary>
        /// 已查阅代理
        /// </summary>
        public delegate void CheckDel();

        /// <summary>
        /// 已查阅事件
        /// </summary>
        private CheckDel CheckEvent = null;

        /// <summary>
        /// 日志信息
        /// </summary>
        private String Message;
        /// <summary>
        /// 等级
        /// </summary>
        private LevelEnum Level;
        /// <summary>
        /// 父亲控件
        /// </summary>
        private StackPanel Father;

        /// <summary>
        /// 等级枚举
        /// </summary>
        public enum LevelEnum
        {
            /// <summary>
            /// 正常日志
            /// </summary>
            Normal ,
            /// <summary>
            /// 重要信息
            /// </summary>
            Important ,
            /// <summary>
            /// 错误信息
            /// </summary>
            Error
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="father"></param>
        /// <param name="del">已查阅事件，在日志被删除之后调用</param>
        public LogMessage(String message , LevelEnum level , StackPanel father , CheckDel del = null)
        {
            InitializeComponent();
            Message = message;
            Level = level;
            Father = father;
            CheckEvent = del;
        }

        /// <summary>
        /// 获取等级信息
        /// </summary>
        /// <returns></returns>
        public LevelEnum GetLevel()
        {
            return Level;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            CheckButton.Visibility = Visibility.Visible;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            CheckButton.Visibility = Visibility.Collapsed;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = null;

            //初始情况下折叠check按钮
            CheckButton.Visibility = Visibility.Collapsed;
            LogMessageTB.Text = Message;
            LogMessageTB.ToolTip = Message;

            switch(Level)
            {
                case LevelEnum.Normal:
                    brush = (SolidColorBrush)FindResource("NormalLevelColor");
                    break;
                case LevelEnum.Important:
                    brush = (SolidColorBrush)FindResource("ImportantLevelColor");
                    break;
                case LevelEnum.Error:
                    brush = (SolidColorBrush)FindResource("ErrorLevelColor");
                    break;
            }

            //设置颜色
            CLineColor.Color = brush.Color;
            LogMessageTB.Foreground = brush;
            CheckButton.IconColor = brush;
        }

        //已查看按钮点击
        private void CheckButton_Click()
        {
            
            this.Father.Children.Remove(this);

            //执行代理事件
            if (CheckEvent != null)
            {
                CheckEvent.Invoke();
            }

        }
    }
}
