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
        public ParaModel StartWork = null;
        /// <summary>
        /// 怠速工况数据
        /// </summary>
        public ParaModel IdlingWork = null;
        /// <summary>
        /// 怠速断油数据
        /// </summary>
        public ParaModel IdlingBreak = null;
        /// <summary>
        /// 校正起作用数据
        /// </summary>
        public ParaModel ReviseBegin = null;
        /// <summary>
        /// 校正工况数据
        /// </summary>
        public ParaModel ReviseWork = null;
        /// <summary>
        /// 校正结束数据
        /// </summary>
        public ParaModel ReviseEnd = null;
        /// <summary>
        /// 标定工况数据
        /// </summary>
        public ParaModel DemWork = null;
        /// <summary>
        /// 调速工况数据
        /// </summary>
        public ParaModel AdjWork = null;
        /// <summary>
        /// 高速断油数据
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
    }
}
