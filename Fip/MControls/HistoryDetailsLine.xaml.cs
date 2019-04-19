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
using Fip.Code;

namespace Fip.MControls
{
    /// <summary>
    /// HistoryDetailsLine.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryDetailsLine : UserControl
    {
        public HistoryDetailsLine()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">数据</param>
        /// <param name="header">工况名称</param>
        public HistoryDetailsLine(HParaModel model , String header)
        {
            InitializeComponent();

            Header.Text = header;

            if(model.S_RotateSpeed > 0)
            {
                S_RotateSpeedTB.Text = model.S_RotateSpeed.ToString();
            }

            if(model.S_InjectionTime > 0)
            {
                S_InjectionTimeTB.Text = model.S_InjectionTime.ToString();
            }

            if(model.R_RackTravel > 0)
            {
                R_RackTravelTB.Text = Math.Round(model.R_RackTravel, PathStaticCollection.Round_Number).ToString();
            }

            if(model.R_InjectionQuantity > 0 )
            {
                R_InjectionQuantityTB.Text = Math.Round(model.R_InjectionQuantity, PathStaticCollection.Round_Number).ToString();
            }

            if(model.R_Asymmetry > 0)
            {
                R_AsymmetryTB.Text = Math.Round(model.R_Asymmetry, PathStaticCollection.Round_Number).ToString();
            }
        }
    }
}
