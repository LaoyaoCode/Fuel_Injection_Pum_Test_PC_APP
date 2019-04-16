using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fip.Code.DB
{
    public class HistoryModel
    {
        /// <summary>
        /// 记录在表中的Id
        /// </summary>
        public int Id = -1;
        /// <summary>
        /// 启动工况数据
        /// </summary>
        public HParaModel StartWork = null;
        /// <summary>
        /// 怠速工况数据
        /// </summary>
        public HParaModel IdlingWork = null;
        /// <summary>
        /// 怠速断油数据
        /// </summary>
        public HParaModel IdlingBreak = null;
        /// <summary>
        /// 校正起作用数据
        /// </summary>
        public HParaModel ReviseBegin = null;
        /// <summary>
        /// 校正工况数据
        /// </summary>
        public HParaModel ReviseWork = null;
        /// <summary>
        /// 校正结束数据
        /// </summary>
        public HParaModel ReviseEnd = null;
        /// <summary>
        /// 标定工况数据
        /// </summary>
        public HParaModel DemWork = null;
        /// <summary>
        /// 调速工况数据
        /// </summary>
        public HParaModel AdjWork = null;
        /// <summary>
        /// 高速断油数据
        /// </summary>
        public HParaModel HighBreak = null;
        /// <summary>
        /// 测试燃油温度设定，由于所有过程设定温度相同，故而独立为一个参数，范围参数
        /// </summary>
        public RangeValue Tem = null;
        /// <summary>
        /// 油泵编号
        /// </summary>
        public String EquCode = null;
        /// <summary>
        /// 油泵型号
        /// </summary>
        public String EquType = null;

        /// <summary>
        /// 测试结果得到的日期
        /// </summary>
        public String HDate = null;
        /// <summary>
        /// 测试结果得到的时间
        /// </summary>
        public String HTime = null;
        /// <summary>
        /// 测试结果是否符合要求
        /// </summary>
        public bool IsPass = false;

        //分隔符
        private static readonly char Seperater = '|';
        private static readonly char Connect = '+';

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if(StartWork != null)
            {
                builder.Append("SW" + Connect + StartWork.ToString() + Seperater);
            }
            
            if(IdlingWork != null)
            {
                builder.Append("IW" + Connect + IdlingWork.ToString() + Seperater);
            }

            if (IdlingBreak != null)
            {
                builder.Append("IB" + Connect + IdlingBreak.ToString() + Seperater);
            }


            if (ReviseBegin != null)
            {
                builder.Append("RB" + Connect + ReviseBegin.ToString() + Seperater);
            }


            if (ReviseWork != null)
            {
                builder.Append("RW" + Connect + ReviseWork.ToString() + Seperater);
            }


            if (ReviseEnd != null)
            {
                builder.Append("RE" + Connect + ReviseEnd.ToString() + Seperater);
            }

            if (DemWork != null)
            {
                builder.Append("DW" + Connect + DemWork.ToString() + Seperater);
            }


            if (HighBreak != null)
            {
                builder.Append("HB" + Connect + HighBreak.ToString() + Seperater);
            }


            if (Tem != null)
            {
                builder.Append("T" + Connect + Tem.ToString() + Seperater);
            }

            if(EquCode != null)
            {
                builder.Append("EC" + Connect + EquCode.ToString() + Seperater);
            }

            if (EquType != null)
            {
                builder.Append("ET" + Connect + EquType.ToString() + Seperater);
            }

            if (HDate != null)
            {
                builder.Append("HD" + Connect + HDate.ToString() + Seperater);
            }

            if (HTime != null)
            {
                builder.Append("HT" + Connect + HTime.ToString() + Seperater);
            }

            builder.Append("IP" + Connect + IsPass.ToString());

            return builder.ToString();
        }

        public HistoryModel()
        {

        }

        public HistoryModel(string para)
        {
            string[] datas = para.Split(Seperater);

            for(int counter = 0; counter < datas.Length; counter++)
            {
                AnalyseData(datas[counter]);
            }
        }

        private void AnalyseData(string data)
        {
            string[] message = data.Split(Connect);

            switch (message[0])
            {
                case "SW":
                    this.StartWork = new HParaModel(message[1]);
                    break;

                case "IW":
                    this.IdlingWork = new HParaModel(message[1]);
                    break;

                case "IB":
                    this.IdlingBreak = new HParaModel(message[1]);
                    break;

                case "RB":
                    this.ReviseBegin = new HParaModel(message[1]);
                    break;

                case "RW":
                    this.ReviseWork = new HParaModel(message[1]);
                    break;

                case "RE":
                    this.ReviseEnd = new HParaModel(message[1]);
                    break;

                case "DW":
                    this.DemWork = new HParaModel(message[1]);
                    break;

                case "HB":
                    this.HighBreak = new HParaModel(message[1]);
                    break;

                case "T":
                    this.Tem = new RangeValue(message[1]);
                    break;

                case "EC":
                    this.EquCode = message[1];
                    break;

                case "ET":
                    this.EquType = message[1];
                    break;

                case "HD":
                    this.HDate = message[1];
                    break;

                case "HT":
                    this.HTime = message[1];
                    break;

                case "IP":
                    this.IsPass = bool.Parse(message[1]);
                    break;
            }
            
        }
    }
}
