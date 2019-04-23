using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Fip.Code.DB
{
    [Serializable]
    public class SDDAndXml
    {
        /// <summary>
        /// 启动工况要求描述
        /// </summary>
        public String StartWork = null;
        /// <summary>
        /// 怠速工况要求描述
        /// </summary>
        public String IdlingWork = null;
        /// <summary>
        /// 怠速断油要求描述
        /// </summary>
        public String IdlingBreak = null;
        /// <summary>
        /// 校正起作用要求描述
        /// </summary>
        public String ReviseBegin = null;
        /// <summary>
        /// 校正工况要求描述
        /// </summary>
        public String ReviseWork = null;
        /// <summary>
        /// 校正结束要求描述
        /// </summary>
        public String ReviseEnd = null;
        /// <summary>
        /// 标定工况要求描述
        /// </summary>
        public String DemWork = null;
        /// <summary>
        /// 调速工况要求描述
        /// </summary>
        public String AdjWork = null;
        /// <summary>
        /// 高速断油要求描述
        /// </summary>
        public String HighBreak = null;
        /// <summary>
        /// 测试燃油温度设定，由于所有过程设定温度相同，故而独立为一个参数，范围参数
        /// </summary>
        public String Tem = null;
        /// <summary>
        /// 油泵编号
        /// </summary>
        public String EquCode = null;
        /// <summary>
        /// 油泵型号
        /// </summary>
        public String EquType = null;

        public SDDAndXml()
        {

        }

        public SDDAndXml(StandardDeviceDesModel model)
        {
            
            StartWork = model.StartWork.ToString();
            IdlingWork = model.IdlingWork.ToString();
            IdlingBreak = model.IdlingBreak.ToString();
            ReviseBegin = model.ReviseBegin.ToString();
            ReviseWork = model.ReviseWork.ToString();
            ReviseEnd = model.ReviseEnd.ToString();
            DemWork = model.DemWork.ToString();
            AdjWork = model.AdjWork.ToString();
            HighBreak = model.HighBreak.ToString();
            Tem = model.Tem.ToString();
            EquCode = model.EquCode ;
            EquType = model.EquType;
        }

        /// <summary>
        /// 将信息保存到xml文件中
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="models">标准信息描述数组</param>
        public static void SaveToXml(String fileName, List<StandardDeviceDesModel> models)
        {
            List<SDDAndXml> list = new List<SDDAndXml>();

            for(int counter = 0; counter < models.Count; counter++)
            {
                list.Add(new SDDAndXml(models[counter]));
            }

            
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<SDDAndXml>));

            using (Stream fStream = new FileStream(fileName,
            FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xmlFormat.Serialize(fStream, list);
            }

        }

        /// <summary>
        /// 从XML文件中获取所有的标准信息描述
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static List<StandardDeviceDesModel> GetSDDFromXml(String fileName)
        {
            List<StandardDeviceDesModel> models = new List<StandardDeviceDesModel>();
            List<SDDAndXml> list = null;

            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<SDDAndXml>));

            using (Stream fStream = new FileStream(fileName,
            FileMode.Open, FileAccess.Read, FileShare.None))
            {
                list = (List<SDDAndXml>)xmlFormat.Deserialize(fStream);
            }

            for(int counter = 0; counter < list.Count; counter++)
            {
                models.Add(list[counter].ToSDD());
            }

            return models;

        }

        public StandardDeviceDesModel ToSDD()
        {
            StandardDeviceDesModel model = new StandardDeviceDesModel();
            model.StartWork = new ParaModel(StartWork.ToString());
            model.IdlingWork = new ParaModel(IdlingWork.ToString());
            model.IdlingBreak = new ParaModel(IdlingBreak.ToString());
            model.ReviseBegin = new ParaModel(ReviseBegin.ToString());
            model.ReviseWork = new ParaModel(ReviseWork.ToString());
            model.ReviseEnd = new ParaModel(ReviseEnd.ToString());
            model.DemWork = new ParaModel(DemWork.ToString());
            model.AdjWork = new ParaModel(AdjWork.ToString());
            model.HighBreak = new ParaModel(HighBreak.ToString());
            model.Tem = new RangeValue(Tem.ToString());
            model.EquCode = EquCode;
            model.EquType = EquType;

            return model;
        }
    }
}
