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
using System.Windows.Shapes;

namespace Fip.Dialog
{
    /// <summary>
    /// MessageDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MessageDialog : Window
    {
        private String Message = String.Empty;
        private bool HadCancelButton = false;

        public MessageDialog()
        {
            InitializeComponent();
        }

        public MessageDialog(String message , bool isSureDialog = false)
        {
            InitializeComponent();
            Message = message;
            HadCancelButton = isSureDialog;
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            ContentTB.Text = Message;

            if(HadCancelButton)
            {
                SureButtonGroup.Visibility = Visibility.Visible;
                MessageGroup.Visibility = Visibility.Hidden;
            }
            else
            {
                SureButtonGroup.Visibility = Visibility.Hidden;
                MessageGroup.Visibility = Visibility.Visible;
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Opacity = 1;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Opacity = 0.5;
        }

        private void CancelButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
        }

        private void OKButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
