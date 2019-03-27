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

namespace Fip.Dialog.Tools
{
    /// <summary>
    /// SpecialPara_NDD.xaml 的交互逻辑
    /// </summary>
    public partial class SpecialPara_NDD : UserControl
    {
        public SpecialPara_NDD()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 值是否正确
        /// </summary>
        /// <returns></returns>
        public bool IsValueRight()
        {
            String code = EquCode_TB.Text.Trim() , type = EquType_TB.Text.Trim();
          
            if(String.IsNullOrEmpty(code) || String.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            if (String.IsNullOrEmpty(type) || String.IsNullOrWhiteSpace(type))
            {
                return false;
            }

            if(Tem_RV.IsValueRight())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
