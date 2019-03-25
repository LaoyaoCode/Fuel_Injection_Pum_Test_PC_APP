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
using MahApps.Metro.IconPacks;

namespace Fip.MControls
{
    /// <summary>
    /// IconButton.xaml 的交互逻辑
    /// </summary>
    public partial class IconButton : UserControl
    {
        public delegate void IconClickDel();

        public delegate void IconClick_ParaDel(IconButton which);

        public IconClickDel IconClick
        {
            get { return (IconClickDel)GetValue(IconClickProperty); }
            set { SetValue(IconClickProperty, value); }
        }

        public static readonly DependencyProperty IconClickProperty =
           DependencyProperty.Register("IconClick", typeof(IconClickDel), typeof(IconButton), new PropertyMetadata(null));

        /// <summary>
        /// 正常情况下的背景
        /// </summary>
        public SolidColorBrush NormalBackColor
        {
            get { return (SolidColorBrush)GetValue(NormalBackColorProperty); }
            set { SetValue(NormalBackColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NormalBackColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalBackColorProperty =
            DependencyProperty.Register("NormalBackColor", typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(null));


        /// <summary>
        /// 按钮图标
        /// </summary>
        public PackIconMaterialKind ButtonIcon
        {
            get { return (PackIconMaterialKind)GetValue(ButtonIconProperty); }
            set { SetValue(ButtonIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonIconProperty =
            DependencyProperty.Register("ButtonIcon", typeof(PackIconMaterialKind), typeof(IconButton), new PropertyMetadata(null));



        /// <summary>
        /// 图标颜色
        /// </summary>
        public SolidColorBrush IconColor
        {
            get { return (SolidColorBrush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register("IconColor", typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(null));




        public IconClick_ParaDel IconClick_ParaEvent
        {
            get { return (IconClick_ParaDel)GetValue(IconClick_ParaEventProperty); }
            set { SetValue(IconClick_ParaEventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconClick_ParaEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconClick_ParaEventProperty =
            DependencyProperty.Register("IconClick_ParaEvent", typeof(IconClick_ParaDel), typeof(IconButton), new PropertyMetadata(null));


        public IconButton()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IconClick != null)
            {
                IconClick.Invoke();
            }

            if(IconClick_ParaEvent != null)
            {
                IconClick_ParaEvent.Invoke(this);
            }
        }
    }
}
