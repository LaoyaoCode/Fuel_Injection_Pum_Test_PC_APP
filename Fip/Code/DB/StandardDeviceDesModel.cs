using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fip.Code.DB
{
    /// <summary>
    /// 器件标准测试参数描述模型
    /// </summary>
    public class StandardDeviceDesModel
    {
        /// <summary>
        /// 记录在表中的Id
        /// </summary>
        public int Id = -1;
        /// <summary>
        /// 启动工况要求描述
        /// </summary>
        public ParaModel StartWork = null;
        /// <summary>
        /// 怠速工况要求描述
        /// </summary>
        public ParaModel IdlingWork = null;
        /// <summary>
        /// 怠速断油要求描述
        /// </summary>
        public ParaModel IdlingBreak = null;
        /// <summary>
        /// 校正起作用要求描述
        /// </summary>
        public ParaModel ReviseBegin = null;
        /// <summary>
        /// 校正工况要求描述
        /// </summary>
        public ParaModel ReviseWork = null;
        /// <summary>
        /// 校正结束要求描述
        /// </summary>
        public ParaModel ReviseEnd = null;
        /// <summary>
        /// 标定工况要求描述
        /// </summary>
        public ParaModel DemWork = null;
        /// <summary>
        /// 调速工况要求描述
        /// </summary>
        public ParaModel AdjWork = null;
        /// <summary>
        /// 高速断油要求描述
        /// </summary>
        public ParaModel HighBreak = null;
        /// <summary>
        /// 测试燃油温度设定，由于所有过程设定温度相同，故而独立为一个参数，范围参数
        /// </summary>
        public RangeValue Tem = null;
        /// <summary>
        /// 油泵编号
        /// </summary>
        public String EquCode = null;
        /// <summary>
        /// 转化为16进制的图标文件内容（可为空，代表没有选择图标）取消图标
        /// </summary>
        //public String IconHex = null;
        /// <summary>
        /// 油泵型号
        /// </summary>
        public String EquType = null;

        //油泵编号 和 油泵型号 不能存在两条数据这两者都相同

        //分隔符
        private static readonly char Seperater = '|';
        private static readonly char Connect = '+';

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("ID" + Connect + Id.ToString() + Seperater);


            if (StartWork != null)
            {
                builder.Append("SW" + Connect + StartWork.ToString() + Seperater);
            }

            if (IdlingWork != null)
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

            if(AdjWork != null)
            {
                builder.Append("AW" + Connect + AdjWork.ToString() + Seperater);
            }

            if (HighBreak != null)
            {
                builder.Append("HB" + Connect + HighBreak.ToString() + Seperater);
            }


            if (Tem != null)
            {
                builder.Append("T" + Connect + Tem.ToString() + Seperater);
            }

            if (EquCode != null)
            {
                builder.Append("EC" + Connect + EquCode.ToString() + Seperater);
            }

            if (EquType != null)
            {
                builder.Append("ET" + Connect + EquType.ToString() + Seperater);
            }

            return builder.ToString();
        }

        public StandardDeviceDesModel()
        {

        }

        public StandardDeviceDesModel(string para)
        {
            string[] datas = para.Split(Seperater);

            for (int counter = 0; counter < datas.Length; counter++)
            {
                AnalyseData(datas[counter]);
            }
        }

        private void AnalyseData(string data)
        {
            string[] message = data.Split(Connect);

            switch (message[0])
            {
                case "ID":
                    this.Id = int.Parse(message[1]);
                    break;

                case "SW":
                    this.StartWork = new ParaModel(message[1]);
                    break;

                case "IW":
                    this.IdlingWork = new ParaModel(message[1]);
                    break;

                case "IB":
                    this.IdlingBreak = new ParaModel(message[1]);
                    break;

                case "RB":
                    this.ReviseBegin = new ParaModel(message[1]);
                    break;

                case "RW":
                    this.ReviseWork = new ParaModel(message[1]);
                    break;

                case "RE":
                    this.ReviseEnd = new ParaModel(message[1]);
                    break;

                case "DW":
                    this.DemWork = new ParaModel(message[1]);
                    break;

                case "AW":
                    this.AdjWork = new ParaModel(message[1]);
                    break;

                case "HB":
                    this.HighBreak = new ParaModel(message[1]);
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
            }

        }

        /// <summary>
        /// 历史测试记录是否符合要求
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsMatchRequire(HistoryModel model)
        {
            if(StartWork != null)
            {
                if(!StartWork.IsMatchTheRequire(model.StartWork))
                {
                    return false;
                }
            }


            if (IdlingWork != null)
            {
                if (!IdlingWork.IsMatchTheRequire(model.IdlingWork))
                {
                    return false;
                }
            }

            if (IdlingBreak != null)
            {
                if (!IdlingBreak.IsMatchTheRequire(model.IdlingBreak))
                {
                    return false;
                }
            }

            if (ReviseBegin != null)
            {
                if (!ReviseBegin.IsMatchTheRequire(model.ReviseBegin))
                {
                    return false;
                }
            }

            if (ReviseWork != null)
            {
                if (!ReviseWork.IsMatchTheRequire(model.ReviseWork))
                {
                    return false;
                }
            }

            if (ReviseEnd != null)
            {
                if (!ReviseEnd.IsMatchTheRequire(model.ReviseEnd))
                {
                    return false;
                }
            }

            if (DemWork != null)
            {
                if (!DemWork.IsMatchTheRequire(model.DemWork))
                {
                    return false;
                }
            }

            if (AdjWork != null)
            {
                if (!AdjWork.IsMatchTheRequire(model.AdjWork))
                {
                    return false;
                }
            }

            if (HighBreak != null)
            {
                if (!HighBreak.IsMatchTheRequire(model.HighBreak))
                {
                    return false;
                }
            }

            if (Tem != null)
            {
                if (!Tem.IsInRange(model.Tem))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
