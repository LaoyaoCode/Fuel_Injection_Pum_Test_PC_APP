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
        /// 油泵编号（唯一编号）（可以是中文加字符串）（UNIQUE）
        /// </summary>
        public String EquCode = null;
        /// <summary>
        /// 转化为16进制的图标文件内容（可为空，代表没有选择图标）
        /// </summary>
        public String IconHex = null;
        /// <summary>
        /// 油泵设备名
        /// </summary>
        public String Name = null;
    }
}
