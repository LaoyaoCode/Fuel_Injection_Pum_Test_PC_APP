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
using OfficeOpenXml;
using Fip.Code;
using System.IO;

namespace Fip
{
    /// <summary>
    /// HistoryDetailsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryDetailsWindow : Window
    {
        private int DeviceId = -1;
        private HistoryModel HModel = null;

        public HistoryDetailsWindow()
        {
            InitializeComponent();
        }

        public HistoryDetailsWindow(int id)
        {
            InitializeComponent();

            DeviceId = id;
            HistoryModel model = DBControler.UnityIns.GetHistoryTotalRecord(id);


            if (model == null)
            {
                BottomPart.Log("读取历史记录失败", LogMessage.LevelEnum.Error);
                this.window.Close();
                return;
            }

            HModel = model;

            EquCodeTB.Text = model.EquCode;
            EquTypeTB.Text = model.EquType;
            TemTB.Text = model.Tem.GetDisplayString();
            DateTB.Text = model.HDate + "  " + model.HTime;

            HistorySP.Children.Add(new HistoryDetailsLine(model.StartWork, "启动工况")) ;
            HistorySP.Children.Add(new HistoryDetailsLine(model.IdlingWork, "怠速工况"));
            HistorySP.Children.Add(new HistoryDetailsLine(model.IdlingBreak, "怠速断油"));
            HistorySP.Children.Add(new HistoryDetailsLine(model.ReviseBegin, "校正起作用"));
            HistorySP.Children.Add(new HistoryDetailsLine(model.ReviseWork, "校正工况"));
            HistorySP.Children.Add(new HistoryDetailsLine(model.ReviseEnd, "校正结束"));
            HistorySP.Children.Add(new HistoryDetailsLine(model.DemWork, "标定工况"));
            HistorySP.Children.Add(new HistoryDetailsLine(model.AdjWork, "调速工况"));
            HistorySP.Children.Add(new HistoryDetailsLine(model.HighBreak, "高速断油"));

            if(model.IsPass)
            {
                IsPassTB.Text = "合格";
            }
            else
            {
                IsPassTB.Text = "不合格";
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

        //导出为excel 按钮点击
        private void ETEButton_Click()
        {
            DialogBox.SaveFileDialog dialog = new DialogBox.SaveFileDialog();
            string filePath;

            dialog.Title = "选择导出文件保存路径";
            dialog.Filter = "xlsx File(*.xlsx)|*.xlsx";
            dialog.DefaultExt = "xlsx";
            //如果用户没有添加则自动添加扩展
            dialog.AddExtension = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExcelWorksheet workSheet;
                //文件路径
                filePath = dialog.FileName;

                using (var p = new ExcelPackage())
                {
                    workSheet = p.Workbook.Worksheets.Add("历史记录");

                    workSheet.Cells[1, 1].Value = "油泵编号";
                    workSheet.Cells[1, 2].Value = HModel.EquCode;

                    workSheet.Cells[1, 5].Value = "油泵型号";
                    workSheet.Cells[1, 6].Value = HModel.EquType;

                    workSheet.Cells[2, 1].Value = "油箱温度";
                    workSheet.Cells[2, 2].Value = HModel.Tem.GetDisplayString();

                    workSheet.Cells[2, 5].Value = "日期";
                    workSheet.Cells[2, 6].Value = HModel.HDate + "  " + HModel.HTime;

                    workSheet.Cells[4, 1].Value = "工况名称";
                    workSheet.Cells[4, 2].Value = "转速rpm";
                    workSheet.Cells[4, 3].Value = "油量";
                    workSheet.Cells[4, 4].Value = "喷油次数";
                    workSheet.Cells[4, 5].Value = "行程";
                    workSheet.Cells[4, 6].Value = "不均匀";

                    List<HParaModel> datas = new List<HParaModel>();

                    datas.Add(HModel.StartWork);
                    datas.Add(HModel.IdlingWork);
                    datas.Add(HModel.IdlingBreak);
                    datas.Add(HModel.ReviseBegin);
                    datas.Add(HModel.ReviseWork);
                    datas.Add(HModel.ReviseEnd);
                    datas.Add(HModel.DemWork);
                    datas.Add(HModel.AdjWork);
                    datas.Add(HModel.HighBreak);

                    workSheet.Cells[5, 1].Value = "启动工况";
                    workSheet.Cells[6, 1].Value = "怠速工况";
                    workSheet.Cells[7, 1].Value = "怠速断油";
                    workSheet.Cells[8, 1].Value = "校正起作用";
                    workSheet.Cells[9, 1].Value = "校正工况" ;
                    workSheet.Cells[10, 1].Value = "校正结束";
                    workSheet.Cells[11, 1].Value = "标定工况";
                    workSheet.Cells[12, 1].Value = "调速工况";
                    workSheet.Cells[13, 1].Value = "高速断油";

                    if (HModel.IsPass)
                    {
                        workSheet.Cells[15, 1].Value = "合格";
                    }
                    else
                    {
                        workSheet.Cells[15, 1].Value = "不合格";
                    }
                    

                    for (int cRow = 5; cRow <= 13; cRow++)
                    {
                        HParaModel data = datas[cRow - 5];

                        if (data.S_RotateSpeed > 0)
                        {
                            workSheet.Cells[cRow, 2].Value = data.S_RotateSpeed.ToString();
                        }
                        else
                        {
                            workSheet.Cells[cRow, 2].Value = "--";
                        }

                        if (data.R_InjectionQuantity > 0)
                        {
                            workSheet.Cells[cRow, 3].Value = Math.Round(data.R_InjectionQuantity, PathStaticCollection.Round_Number).ToString();
                        }
                        else
                        {
                            workSheet.Cells[cRow, 3].Value = "--";
                        }

                        if (data.S_InjectionTime > 0)
                        {
                            workSheet.Cells[cRow, 4].Value = data.S_InjectionTime.ToString();
                        }
                        else
                        {
                            workSheet.Cells[cRow, 4].Value = "--";
                        }

                        if (data.R_RackTravel > 0)
                        {
                            workSheet.Cells[cRow, 5].Value = Math.Round(data.R_RackTravel, PathStaticCollection.Round_Number).ToString();
                        }
                        else
                        {
                            workSheet.Cells[cRow, 5].Value = "--";
                        }

                        if (data.R_Asymmetry > 0)
                        {
                            workSheet.Cells[cRow, 6].Value = Math.Round(data.R_Asymmetry, PathStaticCollection.Round_Number).ToString();
                        }
                        else
                        {
                            workSheet.Cells[cRow, 6].Value = "--";
                        }

                    }

                    //保存数据
                    p.SaveAs(new FileInfo(filePath));
                }
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void CloseButton_Click()
        {
            this.window.Close();
        }

        /*
          public void SaveAsXLSX()
        {
            String fileName;
            int counterForRow = 2;
            ExcelWorksheet workSheet;

            if (UserPerferControler.UnityIns.GetExcelPath() == "NULL")
            {
                fileName = PathStaicCollection.DefaultExcelDir + "\\" + DateTime.Now.ToLongDateString() + "__" + DateTime.Now.ToLongTimeString().Replace(':','.') + ".xlsx";
            }
            else
            {
                fileName = UserPerferControler.UnityIns.GetExcelPath() + "\\" + DateTime.Now.ToLongDateString() + "__" + DateTime.Now.ToLongTimeString().Replace(':', '.') + ".xlsx";
            }

            using (var p = new ExcelPackage())
            {
                workSheet = p.Workbook.Worksheets.Add("数据");

                // Establish column headings in cells.
                workSheet.Cells[1, 1].Value = "器件ID";
                workSheet.Cells[1, 2].Value = "采集编号(0为最先采集的数据)";
                workSheet.Cells[1, 3].Value = "光照强度";
                workSheet.Cells[1, 4].Value = "电压";
                workSheet.Cells[1, 5].Value = "功率因素";
                workSheet.Cells[1, 6].Value = "光照平均值";
                workSheet.Cells[1, 7].Value = "Somethings1";
                workSheet.Cells[1, 8].Value = "Somethings2";

                for (int counterForDevice = 0; counterForDevice <= 19; counterForDevice++)
                {
                    for (int counterForData = 0; counterForData < AllDevicesDatas[counterForDevice].Count(); counterForData++)
                    {
                        workSheet.Cells[counterForRow, 1].Value = counterForDevice.ToString();
                        workSheet.Cells[counterForRow, 2].Value = counterForData.ToString();
                        workSheet.Cells[counterForRow, 3].Value = AllDevicesDatas[counterForDevice].LightIntensity[counterForData].ToString("0.00");
                        workSheet.Cells[counterForRow, 4].Value = AllDevicesDatas[counterForDevice].Voltage[counterForData].ToString("0.00");
                        workSheet.Cells[counterForRow, 5].Value = AllDevicesDatas[counterForDevice].PowerFactor[counterForData].ToString("0.00");
                        workSheet.Cells[counterForRow, 6].Value = AllDevicesDatas[counterForDevice].GetLightIntensityAverage().ToString("0.00");
                        workSheet.Cells[counterForRow, 7].Value = AllDevicesDatas[counterForDevice].Somethings1[counterForData].ToString("0.00");
                        workSheet.Cells[counterForRow, 8].Value = AllDevicesDatas[counterForDevice].Somethings2[counterForData].ToString("0.00");

                        counterForRow++;
                    }
                }

                p.SaveAs(new FileInfo(fileName));
            }  
        }
         
         
         
         
         */
    }
}
