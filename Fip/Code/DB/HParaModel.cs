using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fip.Code.DB
{
    public class HParaModel
    {
        /// <summary>
        /// 设定值 SET 
        /// 转速，只能为整数 转/分 rpm
        /// </summary>
        public int S_RotateSpeed = -1;

        /// <summary>
        /// 设定值 SET
        /// 喷油次数，只能为整数
        /// </summary>
        public int S_InjectionTime = -1;

        /// <summary>
        /// 需要接受的值 RECIEVE
        /// 喷油量 ML
        /// </summary>
        public float R_InjectionQuantity = -1;
        /// <summary>
        /// 需要接受的值 RECIEVE
        /// 齿杆行程 mm(为范围值)
        /// </summary>
        public float R_RackTravel = -1;
        /// <summary>
        /// 需要接受的值 RECIEVE
        /// 不均匀度，只能为一个数
        /// </summary>
        public float R_Asymmetry = -1;

        /// <summary>
        /// 名字和值之间的连接符
        /// </summary>
        private const char NAME_VALUE_LINK = '%';
        /// <summary>
        /// 参数之间的连接符
        /// </summary>
        private const char PARA_LINK = '/';

        public HParaModel()
        {

        }

        public HParaModel(String para)
        {
            String[] data = para.Split(PARA_LINK);

            for (int counter = 0; counter < data.Length; counter++)
            {
                if (data[counter] != null && data[counter].Length != 0)
                {
                    AnalyseString(data[counter]);
                }
            }
        }

        /// <summary>
        /// 分析字符串
        /// </summary>
        /// <param name="para"></param>
        private void AnalyseString(String para)
        {
            String[] data = para.Split(NAME_VALUE_LINK);

            switch (data[0])
            {
                case "SRS":
                    S_RotateSpeed = int.Parse(data[1]);
                    break;
                case "SIT":
                    S_InjectionTime = int.Parse(data[1]);
                    break;
                case "RIQ":
                    R_InjectionQuantity = float.Parse(data[1]); 
                    break;
                case "RRT":
                    R_RackTravel = float.Parse(data[1]); 
                    break;
                case "RA":
                    R_Asymmetry = float.Parse(data[1]);
                    break;
            }
        }

        /// <summary>
        /// 转化为可保存的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (S_RotateSpeed > 0)
            {
                builder.Append("SRS" + NAME_VALUE_LINK + S_RotateSpeed + PARA_LINK);
            }

            if (S_InjectionTime > 0)
            {
                builder.Append("SIT" + NAME_VALUE_LINK + S_InjectionTime + PARA_LINK);
            }

            if (R_InjectionQuantity > 0)
            {
                builder.Append("RIQ" + NAME_VALUE_LINK + R_InjectionQuantity.ToString() + PARA_LINK);
            }

            if (R_RackTravel > 0)
            {
                builder.Append("RRT" + NAME_VALUE_LINK + R_RackTravel.ToString() + PARA_LINK);
            }

            if (R_Asymmetry > 0)
            {
                builder.Append("RA" + NAME_VALUE_LINK + R_Asymmetry.ToString() + PARA_LINK);
            }

            return builder.ToString();
        }
    }
}
