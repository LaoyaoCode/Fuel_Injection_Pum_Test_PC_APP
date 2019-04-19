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
using Fip.Code.DB;

namespace Fip.MControls
{
    /// <summary>
    /// HistoryShortLine.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryShortLine : UserControl
    {
        private int HId = -1;
        public delegate void RemoveDel(HistoryShortLine which);
        private event RemoveDel RemoveEvent;


        public HistoryShortLine()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取历史记录ID
        /// </summary>
        /// <returns></returns>
        public int GetHId()
        {
            return HId;
        }

        public HistoryShortLine(HistoryModel model, RemoveDel remove)
        {
            InitializeComponent();
            HId = model.Id;
            RemoveEvent += remove;

            EquTypeTB.Text = model.EquType;
            TimeAndDateTB.Text = model.HDate + "  " + model.HTime;

            if (model.IsPass)
            {
                PassIcon.Visibility = Visibility.Visible;
            }
        }

        private void RootGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            RootGrid.Background = (SolidColorBrush)FindResource("SelectedColor");
            DetailButton.Visibility = Visibility.Visible;
            RemoveButton.Visibility = Visibility.Visible;
        }

        private void RootGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            RootGrid.Background = (SolidColorBrush)FindResource("AppBackBrush");
            DetailButton.Visibility = Visibility.Collapsed;
            RemoveButton.Visibility = Visibility.Collapsed;
        }

       
        private void SeeDetailButton_Click()
        {
            HistoryDetailsWindow window = new HistoryDetailsWindow(HId);
            window.Show();
        }

        private void DeleteButton_Click()
        {
            RemoveEvent.Invoke(this);
        }
    }
}
