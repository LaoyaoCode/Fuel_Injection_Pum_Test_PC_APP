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

namespace Fip.Dialog.Tools
{
    /// <summary>
    /// CircleButton_NDD.xaml 的交互逻辑
    /// </summary>
    public partial class CircleButton_NDD : UserControl
    {
        public SolidColorBrush NormalColor
        {
            get { return (SolidColorBrush)GetValue(NormalColorProperty); }
            set { SetValue(NormalColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NormalColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalColorProperty =
            DependencyProperty.Register("NormalColor", typeof(SolidColorBrush), typeof(CircleButton_NDD), new PropertyMetadata(null));


        public SolidColorBrush ActiveColor
        {
            get { return (SolidColorBrush)GetValue(ActiveColorProperty); }
            set { SetValue(ActiveColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveColorProperty =
            DependencyProperty.Register("ActiveColor", typeof(SolidColorBrush), typeof(CircleButton_NDD), new PropertyMetadata(null));


        public SolidColorBrush FinishColor
        {
            get { return (SolidColorBrush)GetValue(FinishColorProperty); }
            set { SetValue(FinishColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FinishColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FinishColorProperty =
            DependencyProperty.Register("FinishColor", typeof(SolidColorBrush), typeof(CircleButton_NDD), new PropertyMetadata(null));


        public SolidColorBrush ErrorColor
        {
            get { return (SolidColorBrush)GetValue(ErrorColorProperty); }
            set { SetValue(ErrorColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorColorProperty =
            DependencyProperty.Register("ErrorColor", typeof(SolidColorBrush), typeof(CircleButton_NDD), new PropertyMetadata(null));


        public String ButtonTag
        {
            get { return (String)GetValue(ButtonTagProperty); }
            set { SetValue(ButtonTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonTagProperty =
            DependencyProperty.Register("ButtonTag", typeof(String), typeof(CircleButton_NDD), new PropertyMetadata(String.Empty));


        public delegate void ClickDel(String tag , CircleButton_NDD which);
        public ClickDel ClickEvent
        {
            get { return (ClickDel)GetValue(ClickEventProperty); }
            set { SetValue(ClickEventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClickEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClickEventProperty =
            DependencyProperty.Register("ClickEvent", typeof(ClickDel), typeof(CircleButton_NDD), new PropertyMetadata(null));


        public enum StateEnum
        {
            /// <summary>
            /// 正常情况，即没有填写任何数据
            /// </summary>
            Normal,
            /// <summary>
            /// 完成情况，即填写了数据并且数据格式正确
            /// </summary>
            Finish,
            /// <summary>
            /// 填写数据错误
            /// </summary>
            Error
        }
        /// <summary>
        /// 状态，初始为正常状况
        /// </summary>
        private StateEnum State = StateEnum.Normal;

        /// <summary>
        /// 鼠标是否点击了按钮
        /// </summary>
        private bool IsClick = false;


        public CircleButton_NDD()
        {
            InitializeComponent();
        }

        private void Circle_MouseEnter(object sender, MouseEventArgs e)
        {
           
            Circle.Background = ActiveColor;
        }

        private void Circle_MouseLeave(object sender, MouseEventArgs e)
        {
            //只有没有被点击才恢复到没被点击情况，显示自身状态
           if(!IsClick)
           {
                BackToNotClick();
           }
            
        }

        private void Circle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(!IsClick)
            {
                //调用回调事件
                if (ClickEvent != null)
                {
                    ClickEvent.Invoke(ButtonTag, this);
                }

                IsClick = true;
            }
        }

        /// <summary>
        /// 虚拟按钮点击事件
        /// </summary>
        public void VirtualButton_Click()
        {
            if(!IsClick)
            {
                //调用回调事件
                if (ClickEvent != null)
                {
                    ClickEvent.Invoke(ButtonTag, this);
                }

                IsClick = true;
            }
            Circle.Background = ActiveColor;
        }

        /// <summary>
        /// 返回没有被点击的状态
        /// </summary>
        public void BackToNormalCondition()
        {
            IsClick = false;
            BackToNotClick();
        }

      
        /// <summary>
        /// 返回没有被点击的状态，显示自身的状态
        /// </summary>
        private void BackToNotClick()
        {
            switch (State)
            {
                case StateEnum.Normal:
                    Circle.Background = NormalColor;
                    break;
                case StateEnum.Finish:
                    Circle.Background = FinishColor;
                    break;
                case StateEnum.Error:
                    Circle.Background = ErrorColor;
                    break;
            }
        }
    }
}
