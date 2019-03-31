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
    /// DeviceShortMessageLine.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceShortMessageLine : UserControl
    {
        /// <summary>
        /// 是否已经选择这个器件
        /// </summary>
        private bool IsSelected = false;
        /// <summary>
        /// 是否正在测试
        /// </summary>
        private bool Testing = false;
        /// <summary>
        /// 器件ID
        /// </summary>
        private int DeviceID = 0;
        /// <summary>
        /// 被选择代理事件
        /// </summary>
        /// <param name="line"></param>
        public delegate void SelectDel(DeviceShortMessageLine line);

        private SelectDel SelectEvent = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">器件id</param>
        /// <param name="equType">油泵型号</param>
        /// <param name="del">被点击选择代理事件</param>
        public DeviceShortMessageLine(int id ,String equType , SelectDel del = null)
        {
            InitializeComponent();
            DeviceID = id;

            SelectEvent = del;
            DeviceName.Text = equType;
            InitialTB.Text = equType.ToCharArray()[0].ToString();
        }

        private void UserControl_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsSelected = true;
            //执行代理事件
            if (SelectEvent != null)
            {
                SelectEvent.Invoke(this);
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            //只有在没有测试的时候才可以删除和修改
            if (!Testing)
            {
                ModifyButton.Visibility = Visibility.Visible;
                RemoveButton.Visibility = Visibility.Visible;
            }
            DetailButton.Visibility = Visibility.Visible;

            //改变背景颜色
            RootGrid.Background = (SolidColorBrush)FindResource("SelectedColor");
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            ModifyButton.Visibility = Visibility.Collapsed;
            RemoveButton.Visibility = Visibility.Collapsed;
            DetailButton.Visibility = Visibility.Collapsed;

            //只有没有被选中的时候才会变回正常颜色
            if (!IsSelected)
            {
                RootGrid.Background = (SolidColorBrush)FindResource("AppBackBrush");
            }
        }

        /// <summary>
        /// 离开被选中状态
        /// </summary>
        public void Selected_Cancel()
        {
            IsSelected = false;
            //恢复背景
            RootGrid.Background = (SolidColorBrush)FindResource("AppBackBrush");
        }

        /// <summary>
        /// 离开正在测试状态
        /// </summary>
        public void Testing_Cancel()
        {
            Testing = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

       
    }
}
