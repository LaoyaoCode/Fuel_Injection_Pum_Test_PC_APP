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
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Fip.Dialog.Tools;

namespace Fip.Dialog
{
    /// <summary>
    /// NewDeviceDialog.xaml 的交互逻辑
    /// </summary>
    public partial class NewDeviceDialog : Window
    {
        /// <summary>
        /// 现在正在活跃的界面
        /// </summary>
        private CircleButton_NDD NowActiveButton = null;
        /// <summary>
        /// 内容的宽度
        /// </summary>
        private const double CONTENT_WIDTH = 580;

        public NewDeviceDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口top bar拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DargWindowMove(object sender, MouseButtonEventArgs e)
        {
            //move the windows
            this.DragMove();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void CloseButton_Click()
        {
            this.DialogResult = false;
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(CircleButton_NDD button in StepButtonContainer.Children)
            {
                button.ClickEvent = StepButton_Click;
            }

            ((CircleButton_NDD)StepButtonContainer.Children[0]).VirtualButton_Click();
        }

        private void StepButton_Click(String tag , CircleButton_NDD which)
        {
            if (NowActiveButton != null)
            {
                //检查数据，然后根据数据来判断修改状态枚举

                //返回到普通的状态，并且根据之前修改的状态枚举来改变颜色
                NowActiveButton.BackToNormalCondition();
            }

            //根据tag来执行动画
            StepAnimation(int.Parse(tag));
            NowActiveButton = which;
        }

        private void StepAnimation(int index)
        {
            Thickness now = MainContent.Margin;
            Thickness target = MainContent.Margin;
            target.Left = -CONTENT_WIDTH * index;
            int time = (int)(Math.Sqrt(Math.Abs(target.Left - now.Left))) * 10;
            ThicknessAnimation animation = new ThicknessAnimation(now, target, new Duration(new TimeSpan(0,0,0,0, time) ) );
            MainContent.BeginAnimation(Grid.MarginProperty, animation);
        }
    }
}
