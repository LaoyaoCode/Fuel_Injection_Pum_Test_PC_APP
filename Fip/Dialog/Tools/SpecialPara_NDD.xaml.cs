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
        /// 设置输入的信息
        /// </summary>
        /// <param name="euqCode"></param>
        /// <param name="equType"></param>
        /// <param name="tem"></param>
        public void SetMessage(String euqCode , String equType , RangeValue tem)
        {
            EquCode_TB.Text = euqCode;
            EquType_TB.Text = equType;
            
            if(tem.IsInfinity())
            {
                Tem_RV.SetRangeValue_Infinity(tem.GetMin().ToString());
            }
            else
            {
                Tem_RV.SetRangeValue(tem.GetMax().ToString(), tem.GetMin().ToString());
            }
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

        public String GetEquCode()
        {
            String code = EquCode_TB.Text.Trim();
            if (String.IsNullOrEmpty(code) || String.IsNullOrWhiteSpace(code))
            {
                return String.Empty;
            }
            else
            {
                return code;
            }
               
        }

        public String GetEquType()
        {
            String type = EquType_TB.Text.Trim();

            if (String.IsNullOrEmpty(type) || String.IsNullOrWhiteSpace(type))
            {
                return String.Empty;
            }
            else
            {
                return type;
            }
        }

        public RangeValue GetTem()
        {
            if(!Tem_RV.IsValueRight())
            {
                return null;
            }
            else
            {
                return Tem_RV.GetRangeValue();
            }
        }
    }
}
