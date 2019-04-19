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
using System.Windows.Media.Animation;
using Fip.MControls;
using Fip.Code.DB;
using Fip.Code.Trans;
using System.Threading;
using Fip.Code.Trans;
using Fip.Dialog;

namespace Fip
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static  MainWindow MWindow = null;

        public MainWindow()
        {
            InitializeComponent();
            MWindow = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                new DBControler();
                BottomPart.Log("数据库检查完成", LogMessage.LevelEnum.Normal);
            }
            catch (Exception)
            {
                //数据库连接失败，直接返回
                BottomPart.Log("数据库连接失败", LogMessage.LevelEnum.Error);
                return;
            }

            //实例化传输方式，这里作为测试实例化的是TCP/IP传输方式，但是使用的都是ITRANS抽象类
            new TcpIpTrans();

            List<HistoryModel> history = DBControler.UnityIns.GetHistoryAllShortRecord();
            for(int counter = 0; counter < history.Count; counter++)
            {
                AddAHistoryLine(history[counter]);
            }
        }

        public void AddAHistoryLine(HistoryModel model)
        {
            HistorySP.Children.Insert(0,new HistoryShortLine(model, (which) =>
            {
                MessageDialog mDialog = new MessageDialog(String.Format("油泵型号 : {0}\n记录时间 : {1}\n是否确认删除该历史记录?", which.EquTypeTB.Text, which.TimeAndDateTB.Text), true);

                if(mDialog.ShowDialog().Value)
                {
                    //确认需要删除
                    if(DBControler.UnityIns.DeleteHistoryRecord(which.GetHId()))
                    {
                        //成功删除
                        HistorySP.Children.Remove(which);
                    }
                    else
                    {
                        BottomPart.Log("删除历史记录失败", LogMessage.LevelEnum.Error);
                    }
                }
            }));
        }

        /// <summary>
        /// 关闭窗口按钮被点击
        /// </summary>
        private void CloseButton_Click()
        {
            if(ITrans.UnityIns != null)
            {
                ITrans.UnityIns.Close();
            }

            Application.Current.Shutdown();
        }

        /// <summary>
        /// 最小化窗口被点击
        /// </summary>
        //最小化按钮
        private void MinButton_Click()
        {
            this.WindowState = WindowState.Minimized;
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
        /// 窗口状态变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                BeginStoryboard((Storyboard)this.Resources["MaxAnimation"]);
            }
        }


    }
}
