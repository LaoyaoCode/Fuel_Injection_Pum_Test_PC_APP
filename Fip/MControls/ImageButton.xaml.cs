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
    /// ImageButton.xaml 的交互逻辑
    /// </summary>
    public partial class ImageButton : UserControl
    {
        public delegate void ClickDel();



        public ClickDel ClickEvent
        {
            get { return (ClickDel)GetValue(ClickEventProperty); }
            set { SetValue(ClickEventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClickEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClickEventProperty =
            DependencyProperty.Register("ClickEvent", typeof(ClickDel), typeof(ImageButton), new PropertyMetadata(null));



        public String ImageSource
        {
            get { return (String)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(String), typeof(ImageButton), new PropertyMetadata(String.Empty));




        public double NormalOp
        {
            get { return (double)GetValue(NormalOpProperty); }
            set { SetValue(NormalOpProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NormalOp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalOpProperty =
            DependencyProperty.Register("NormalOp", typeof(double), typeof(ImageButton), new PropertyMetadata(1.0));



        public ImageButton()
        {
            InitializeComponent();
          
        }

        private void ButtonImage_MouseEnter(object sender, MouseEventArgs e)
        {
            ButtonImage.Opacity = 1;
        }

        private void ButtonImage_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonImage.Opacity = NormalOp;
        }

        private void ButtonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(ClickEvent != null)
            {
                ClickEvent.Invoke();
            }
        }
    }
}
