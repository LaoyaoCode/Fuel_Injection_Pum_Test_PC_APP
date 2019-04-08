using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fip.Code
{
    public static class PathStaticCollection
    {
        /// EXE文件运行文件夹，附带分隔符
        /// </summary>
        public static string RootOfExePath = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 数据文件夹路径 , 没有分隔符
        /// </summary>
        public static String DatasDirPath = RootOfExePath + "Datas";
        /// <summary>
        /// 数据记录数据表路径
        /// </summary>
        public static String RecordDBPath = DatasDirPath + "\\Records.s3db";

        /// <summary>
        /// 浮点数据显示保留小数位数
        /// </summary>
        public static int Round_Number = 3;
    }
}
