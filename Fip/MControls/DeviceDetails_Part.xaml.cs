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
    /// DeviceDetails_Part.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceDetails_Part : UserControl
    {


        public String HeaderName
        {
            get { return (String)GetValue(HeaderNameProperty); }
            set { SetValue(HeaderNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderNameProperty =
            DependencyProperty.Register("HeaderName", typeof(String), typeof(DeviceDetails_Part), new PropertyMetadata(String.Empty));

        public DeviceDetails_Part()
        {
            InitializeComponent();
        }
       
        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="model"></param>
        public void SetMessage(ParaModel model)
        {
            if (model != null)
            {
                S_RotateSpeed_TB.Text = model.S_RotateSpeed.ToString();

                if (model.S_InjectionTime > 0)
                {
                    S_InjectionTime_TB.Text = model.S_InjectionTime.ToString();
                    ((Grid)S_InjectionTime_TB.Parent).Visibility = Visibility.Visible;
                }

                if (model.R_InjectionQuantity != null)
                {
                    R_InjectionQuantity_TB.Text = model.R_InjectionQuantity.GetDisplayString();
                    ((Grid)R_InjectionQuantity_TB.Parent).Visibility = Visibility.Visible;
                }

                if (model.R_RackTravel != null)
                {
                    R_RackTravel_TB.Text = model.R_RackTravel.GetDisplayString();
                    ((Grid)R_RackTravel_TB.Parent).Visibility = Visibility.Visible;
                }

                if (model.R_Asymmetry > 0)
                {
                    R_Asymmetry_TB.Text = Math.Round(model.R_Asymmetry, PathStaticCollection.Round_Number).ToString();
                    ((Grid)R_Asymmetry_TB.Parent).Visibility = Visibility.Visible;
                }
            }
        }
    }
}
