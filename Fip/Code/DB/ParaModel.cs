using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fip.Code.DB
{
    /// <summary>
    /// 参数模型
    /// </summary>
    public class ParaModel
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
        /// 喷油量(为范围) ML
        /// </summary>
        public RangeValue R_InjectionQuantity = null;
        /// <summary>
        /// 需要接受的值 RECIEVE
        /// 齿杆行程 mm(为范围值)
        /// </summary>
        public RangeValue R_RackTravel = null;
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

        public ParaModel()
        {

        }

        public ParaModel(String para)
        {
            String[] data = para.Split(PARA_LINK);

            for(int counter = 0; counter < data.Length; counter++)
            {
                if(data[counter] != null && data[counter].Length != 0)
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

            switch(data[0])
            {
                case "SRS":
                    S_RotateSpeed = int.Parse(data[1]);
                    break;
                case "SIT":
                    S_InjectionTime = int.Parse(data[1]);
                    break;
                case "RIQ":
                    R_InjectionQuantity = new RangeValue(data[1]);
                    break;
                case "RRT":
                    R_RackTravel = new RangeValue(data[1]);
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

            if(S_RotateSpeed > 0)
            {
                builder.Append("SRS" + NAME_VALUE_LINK + S_RotateSpeed + PARA_LINK);
            }

            if(S_InjectionTime > 0)
            {
                builder.Append("SIT" + NAME_VALUE_LINK + S_InjectionTime + PARA_LINK);
            }

            if(R_InjectionQuantity != null)
            {
                builder.Append("RIQ" + NAME_VALUE_LINK + R_InjectionQuantity.ToString() + PARA_LINK);
            }

            if(R_RackTravel != null)
            {
                builder.Append("RRT" + NAME_VALUE_LINK + R_RackTravel.ToString() + PARA_LINK);
            }

            if(R_Asymmetry > 0)
            {
                builder.Append("RA" + NAME_VALUE_LINK + R_Asymmetry.ToString() + PARA_LINK);
            }

            return builder.ToString();
        }
    }

    /// <summary>
    /// 范围数据
    /// </summary>
    public class RangeValue
    {
        private float _Min = 0;
        private float _Max = float.MaxValue;
        /// <summary>
        /// 无限字符
        /// 为 大写的 O
        /// </summary>
        public const char INFINITY = 'I';
        /// <summary>
        /// 是否是正无限
        /// </summary>
        private bool IsINFINITY = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="isInfinity">是否达到了正无限</param>
        public RangeValue(float min , float max , bool isInfinity = false)
        {
            IsINFINITY = isInfinity;

            _Max = max;
            if(IsINFINITY)
            {
                _Max = float.MaxValue;
            }

            _Min = min;
        }

        /// <summary>
        /// 通过描述字符串来生成对象
        /// (min-max)
        /// </summary>
        /// <param name="desString"></param>
        public RangeValue(String desString)
        {
            desString = desString.Substring(1, desString.Length - 2);
            String[] values = desString.Split('-');
            _Min = float.Parse(values[0]);
           
            if(values[1] == "I" )
            {
                IsINFINITY = true;
                _Max = float.MaxValue;
            }
            else
            {
                _Max = float.Parse(values[1]);
            }
        }

        /// <summary>
        /// 转化为可保存或显示的字符
        /// (min-max)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('(');
            builder.Append(_Min);
            builder.Append('-');

            //如果为正无穷则直接转化为字符无穷标识
            if(IsINFINITY)
            {
                builder.Append("I");
            }
            else
            {
                builder.Append(_Max);
            }
            
            builder.Append(')');

            return builder.ToString();
        }

        /// <summary>
        /// 是否在范围之中
        /// </summary>
        /// <param name="number">需要测试的数字</param>
        /// <returns></returns>
        public bool IsInRange(float number)
        {
            if(number>= _Min && number<= _Max)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否是正无穷
        /// </summary>
        /// <returns></returns>
        public bool IsInfinity()
        {
            return IsINFINITY;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <returns></returns>
        public float GetMin()
        {
            return _Min;
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        public float GetMax()
        {
            return _Max;
        }

        /// <summary>
        /// 获取用于显示的字符串
        /// </summary>
        /// <returns></returns>
        public String GetDisplayString()
        {
            if(IsINFINITY)
            {
                return Math.Round(_Min, PathStaticCollection.Round_Number).ToString() + " - " + "∞";
            }
            else
            {
                return Math.Round(_Min, PathStaticCollection.Round_Number).ToString() + " - " + Math.Round(_Max, PathStaticCollection.Round_Number).ToString();
            }
        }
    }


}
