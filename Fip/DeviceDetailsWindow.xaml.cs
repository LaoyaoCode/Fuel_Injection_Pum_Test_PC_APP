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
using Fip.Code.DB;
using Fip.MControls;
using DialogBox = System.Windows.Forms;

namespace Fip
{
    /// <summary>
    /// DeviceDetailsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceDetailsWindow : Window
    {
        private int _ID = -1;

        public DeviceDetailsWindow(int id)
        {
            InitializeComponent();
            _ID = id;
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            StandardDeviceDesModel model = DBControler.UnityIns.GetSSDesTotalRecord(_ID);
            //无法读取数据，故而直接报错
            if(model == null)
            {
                BottomPart.Log("读取数据出现错误!", LogMessage.LevelEnum.Error);
                this.window.Close();
                return;
            }
            else
            {
                //设置信息
                EquCode_TB.Text = model.EquCode;
                EquType_TB.Text = model.EquType;
                Tem_TB.Text = model.Tem.GetDisplayString();

                StartWork_Part.SetMessage(model.StartWork);
                IdlingWork_Part.SetMessage(model.IdlingWork);
                IdlingBreak_Part.SetMessage(model.IdlingBreak);
                ReviseBegin_Part.SetMessage(model.ReviseBegin);
                ReviseWork_Part.SetMessage(model.ReviseWork);
                ReviseEnd_Part.SetMessage(model.ReviseEnd);
                DemWork_Part.SetMessage(model.DemWork);
                AdjWork_Part.SetMessage(model.AdjWork);
                HighBreak_Part.SetMessage(model.HighBreak);
            }
        }

        private void ETXButton_Click()
        {
            DialogBox.SaveFileDialog dialog = new DialogBox.SaveFileDialog();
            string filePath;

            dialog.Title = "选择导出文件保存路径";
            dialog.Filter = "xml File(*.xml)|*.xml";
            dialog.DefaultExt = "xml";
            //如果用户没有添加则自动添加扩展
            dialog.AddExtension = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //文件路径
                filePath = dialog.FileName;
                StandardDeviceDesModel model = DBControler.UnityIns.GetSSDesTotalRecord(_ID);
                List<StandardDeviceDesModel> list = new List<StandardDeviceDesModel>();
                list.Add(model);

                SDDAndXml.SaveToXml(filePath, list);
                BottomPart.Log("导出成功", LogMessage.LevelEnum.Important);
            }

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
            this.window.Close();
        }
    }
}
