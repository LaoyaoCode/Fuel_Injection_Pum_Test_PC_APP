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
using Fip.Dialog;
using Fip.Code.DB;

namespace Fip.MControls
{
    /// <summary>
    /// DeviceSMessageControler.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceSMessageControler : UserControl
    {
        private DeviceShortMessageLine NowUseDevice = null;

        public DeviceSMessageControler()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(StandardDeviceDesModel model in DBControler.UnityIns.GetSSDesAllShortRecord())
            {
                AddDSMessage(model.Id, model.EquType);
            }
        }

        /// <summary>
        /// 添加器件信息(显示部分，不包含数据库操作)
        /// </summary>
        /// <param name="id">信息Id</param>
        private void AddDSMessage(int id , String equType)
        {
            DeviceShortMessageLine line = new DeviceShortMessageLine(id, equType,(DeviceShortMessageLine which) =>
            {
                //之前的器件取消选择状态
                if (NowUseDevice != null)
                {
                    NowUseDevice.Selected_Cancel();
                }

                NowUseDevice = which;
            });

            DeviceSMesageContainer.Children.Add(line);
        }


        private void AddNewDeviceButton_Click()
        {
            NewDeviceDialog dialog = new NewDeviceDialog();
            dialog.Owner = MainWindow.MWindow;

            //完成输入工作
            if(dialog.ShowDialog().Value)
            {

            }
        }
    }
}
