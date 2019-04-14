using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Common;
using System.IO;
using Fip.Code;

namespace Fip.Code.DB
{
    public class DBControler
    {
        /// <summary>
        /// 唯一实例
        /// </summary>
        public static DBControler UnityIns = null;

        /// <summary>
        /// 连接字符串
        /// </summary>
        private static String ConnectingString =
            "Data Source =" + PathStaticCollection.RecordDBPath + ";" + "Version=3";

        private const String SDDesTableName = "SDDes";


        private const String HistoryTableName = "History";

        /// <summary>
        /// 检查创建字符串
        /// </summary>
        private static String CreateSDDesTableSqlStatement =
            "CREATE TABLE IF NOT EXISTS " + SDDesTableName
            + "("
            + "Id INTEGER PRIMARY KEY AUTOINCREMENT,"
            + "StartWork TEXT NOT NULL,"
            + "IdlingWork TEXT NOT NULL,"
            + "IdlingBreak TEXT NOT NULL,"
            + "ReviseBegin TEXT NOT NULL,"
            + "ReviseWork TEXT NOT NULL,"
            + "ReviseEnd TEXT NOT NULL,"
            + "DemWork TEXT NOT NULL,"
            + "AdjWork TEXT NOT NULL,"
            + "HighBreak TEXT NOT NULL,"
            + "Tem TEXT NOT NULL,"
            + "EquCode TEXT NOT NULL,"
            //+ "IconHex TEXT," 取消图标
            + "EquType TEXT NOT NULL"
            + ");";

        /// <summary>
        /// 检查历史记录表创建语句
        /// </summary>
        private static String CreateHistoryTableSqlStatement =
            "CREATE TABLE IF NOT EXISTS " + HistoryTableName
            + "("
            + "Id INTEGER PRIMARY KEY AUTOINCREMENT,"
            + "StartWork TEXT NOT NULL,"
            + "IdlingWork TEXT NOT NULL,"
            + "IdlingBreak TEXT NOT NULL,"
            + "ReviseBegin TEXT NOT NULL,"
            + "ReviseWork TEXT NOT NULL,"
            + "ReviseEnd TEXT NOT NULL,"
            + "DemWork TEXT NOT NULL,"
            + "AdjWork TEXT NOT NULL,"
            + "HighBreak TEXT NOT NULL,"
            + "Tem TEXT NOT NULL,"
            + "EquCode TEXT NOT NULL,"
            + "EquType TEXT NOT NULL,"
            + "HDate TEXT NOT NULL,"
            + "HTime TEXT NOT NULL"
            + ");";

        public DBControler()
        {
            CheckAndInit();
            UnityIns = this;
        }

        private void CheckAndInit()
        {
            //检查数据文件夹是否存在
            DirectoryInfo datasDir = new DirectoryInfo(PathStaticCollection.DatasDirPath);
            //不存在则创建
            if (!datasDir.Exists)
            {
                datasDir.Create();
            }

            //检查标准描述数据库表是否存在，如不存在，则直接创建并且初始化
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                sqlConnect.Open();

                //检查标准器件测试数据表存在
                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = CreateSDDesTableSqlStatement;
                command.ExecuteNonQuery();
            }

            //检查历史数据库via哦是否存在，如不存在，则直接创建并且初始化
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                sqlConnect.Open();

                //检查标准器件测试数据表存在
                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = CreateHistoryTableSqlStatement;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 根据Id获取一行所有的信息
        /// 测试标准器件参数表
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>null 代表没有此行或者数据收到损坏</returns>
        public StandardDeviceDesModel GetSSDesTotalRecord(int id)
        {
            StandardDeviceDesModel record = new StandardDeviceDesModel();

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                sqlConnect.Open();

                String commandString = String.Format("SELECT * FROM " + SDDesTableName + " WHERE Id='{0}'",id);
                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = commandString;

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try
                        {
                            //读取所有的信息
                            record.Id = id;
                            record.StartWork = new ParaModel(reader["StartWork"].ToString());
                            record.IdlingWork = new ParaModel(reader["IdlingWork"].ToString());
                            record.IdlingBreak = new ParaModel(reader["IdlingBreak"].ToString());
                            record.ReviseBegin = new ParaModel(reader["ReviseBegin"].ToString());
                            record.ReviseWork = new ParaModel(reader["ReviseWork"].ToString());
                            record.ReviseEnd = new ParaModel(reader["ReviseEnd"].ToString());
                            record.DemWork = new ParaModel(reader["DemWork"].ToString());
                            record.AdjWork = new ParaModel(reader["AdjWork"].ToString());
                            record.HighBreak = new ParaModel(reader["HighBreak"].ToString());
                            record.Tem = new RangeValue(reader["Tem"].ToString());
                            record.EquCode = reader["EquCode"].ToString();
                            //record.IconHex = reader["IconHex"].ToString();
                            record.EquType = reader["EquType"].ToString();
                        }
                        catch (Exception)
                        {
                            record = null;
                        }
                    }
                    else
                    {
                        record = null;
                    }
                }

                return record;
            }
        }

        /// <summary>
        /// 获取所有的简短信息
        /// 测试标准器件参数表
        /// </summary>
        /// <returns></returns>
        public List<StandardDeviceDesModel> GetSSDesAllShortRecord()
        {
            List<StandardDeviceDesModel> datas = new List<StandardDeviceDesModel>();

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                String commandString = "SELECT Id,EquType,EquCode FROM " + SDDesTableName;

                sqlConnect.Open();

                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = commandString;


                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StandardDeviceDesModel record = new StandardDeviceDesModel();

                        try
                        {
                            //获取用于显示的简短数据
                            record.Id = int.Parse(reader["Id"].ToString());
                            //record.IconHex = reader["IconHex"].ToString();
                            record.EquType = reader["EquType"].ToString();
                            record.EquCode = reader["EquCode"].ToString();

                            datas.Add(record);
                        }
                        catch (Exception)
                        {


                        }

                    }
                }

                return datas;
            }

        }

        /// <summary>
        /// 插入一条新的记录
        /// 测试标准器件参数表
        /// </summary>
        /// <param name="model">需要添加的新模型</param>
        /// <returns>插入表中自动生成的Id,-1代表失败</returns>
        public int AddSSDesRecord(StandardDeviceDesModel model)
        {
            int id = -1;
            string sql = string.Format("Insert Into " + SDDesTableName +
                "(StartWork,IdlingWork, IdlingBreak, ReviseBegin,ReviseWork, ReviseEnd,DemWork,AdjWork,HighBreak,Tem,EquCode,EquType) Values" +
                "('{0}', '{1}', '{2}', '{3}','{4}', '{5}', '{6}', '{7}','{8}', '{9}','{10}','{11}')",
                model.StartWork,model.IdlingWork, model.IdlingBreak, model.ReviseBegin,model.ReviseWork, model.ReviseEnd, model.DemWork,
                model.AdjWork, model.HighBreak, model.Tem, model.EquCode, /*model.IconHex,*/ model.EquType);

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                //打开连接
                sqlConnect.Open();

                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = sql;

                //如果成功插入则代表成功
                if (command.ExecuteNonQuery() == 1)
                {
                    String getIdCommandString = "SELECT last_insert_rowid() FROM " + SDDesTableName;

                    SQLiteCommand getIdCommand = sqlConnect.CreateCommand();
                    getIdCommand.CommandText = getIdCommandString;

                    //读取刚刚插入的ID，赋值给模型
                    using (SQLiteDataReader reader = getIdCommand.ExecuteReader())
                    {
                        reader.Read();
                        id = reader.GetInt32(0);
                    }
                }
            }

            return id;
        }

        /// <summary>
        /// 修改数据
        /// 测试标准器件参数表
        /// </summary>
        /// <param name="model">需要修改的数据，必须要有所有的数据齐全，未修改的数据也需要添加上去</param>
        /// <returns>代表是否修改成功</returns>
        public bool ModifySSDesRecord(StandardDeviceDesModel model)
        {
            bool isSuccess = false;

            string sql = string.Format("UPDATE " + SDDesTableName + " SET StartWork = '{1}',IdlingWork='{2}',IdlingBreak='{3}'," +
                "ReviseBegin='{4}',ReviseWork='{5}',ReviseEnd='{6}',DemWork='{7}',AdjWork='{8}',HighBreak='{9}',Tem='{10}'," +
                "EquCode='{11}',EquType='{12}' WHERE Id = '{0}'", model.Id, model.StartWork,model.IdlingWork,
                model.IdlingBreak, model.ReviseBegin, model.ReviseWork, model.ReviseEnd, model.DemWork,
                model.AdjWork, model.HighBreak, model.Tem, model.EquCode, /*model.IconHex,*/ model.EquType);

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                //打开连接
                sqlConnect.Open();

                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = sql;

                //如果成功插入则代表成功
                if (command.ExecuteNonQuery() == 1)
                {
                    isSuccess = true;
                }
            }

            return isSuccess;
        }

        /// <summary>
        /// 删除数据
        /// 测试标准器件参数表
        /// </summary>
        /// <param name="id">需要删除数据的id</param>
        /// <returns>代表是否修改成功</returns>
        public bool DeleteSSDesRecord(int id)
        {
            bool isSuccess = false;
            String sql = String.Format("DELETE FROM " + SDDesTableName + " WHERE Id='{0}'",id);

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                //打开连接
                sqlConnect.Open();

                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = sql;

                //如果成功插入则代表成功
                if (command.ExecuteNonQuery() == 1)
                {
                    isSuccess = true;
                }
            }

            return isSuccess;
        }

        /// <summary>
        /// 检查是否重复 ， 即 油泵编号 和 油泵型号 都相同
        /// 测试标准器件参数表
        /// </summary>
        /// <returns></returns>
        public bool CheckSSDesRepetition(String code , String type , out int id)
        {
            bool result = false;
            id = -1;

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                String commandString = String.Format("SELECT Id FROM " + SDDesTableName + " WHERE EquCode='{0}' AND EquType='{1}'", code,type);
                sqlConnect.Open();
                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = commandString;


                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    //如果查询到了相同的行，则返回true
                    if(reader.HasRows)
                    {
                        reader.Read();
                        id = int.Parse(reader["Id"].ToString());
                        result =  true;
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// 获取所有的简短信息(未测试)
        /// 历史记录表
        /// </summary>
        /// <returns></returns>
        public List<HistoryModel> GetHistoryAllShortRecord()
        {
            List<HistoryModel> datas = new List<HistoryModel>();

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                String commandString = "SELECT Id,EquType,HDate,HTime FROM " + HistoryTableName;

                sqlConnect.Open();

                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = commandString;


                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        HistoryModel record = new HistoryModel();

                        try
                        {
                            //获取用于显示的简短数据
                            record.Id = int.Parse(reader["Id"].ToString());
                            record.EquType = reader["EquType"].ToString();
                            record.HDate = reader["HDate"].ToString();
                            record.HTime = reader["HTime"].ToString();
                            datas.Add(record);
                        }
                        catch (Exception)
                        {


                        }

                    }
                }

                return datas;
            }

        }

        /// <summary>
        /// 根据Id获取一行所有的信息(未测试)
        /// 历史记录表
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>null 代表没有此行或者数据收到损坏</returns>
        public HistoryModel GetHistoryTotalRecord(int id)
        {
            HistoryModel record = new HistoryModel();

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                sqlConnect.Open();

                String commandString = String.Format("SELECT * FROM " + HistoryTableName + " WHERE Id='{0}'", id);
                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = commandString;

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try
                        {
                            //读取所有的信息
                            record.Id = id;
                            record.StartWork = new ParaModel(reader["StartWork"].ToString());
                            record.IdlingWork = new ParaModel(reader["IdlingWork"].ToString());
                            record.IdlingBreak = new ParaModel(reader["IdlingBreak"].ToString());
                            record.ReviseBegin = new ParaModel(reader["ReviseBegin"].ToString());
                            record.ReviseWork = new ParaModel(reader["ReviseWork"].ToString());
                            record.ReviseEnd = new ParaModel(reader["ReviseEnd"].ToString());
                            record.DemWork = new ParaModel(reader["DemWork"].ToString());
                            record.AdjWork = new ParaModel(reader["AdjWork"].ToString());
                            record.HighBreak = new ParaModel(reader["HighBreak"].ToString());
                            record.Tem = new RangeValue(reader["Tem"].ToString());
                            record.EquCode = reader["EquCode"].ToString();
                           
                            record.EquType = reader["EquType"].ToString();
                            record.HDate = reader["HDate"].ToString();
                            record.HTime = reader["HTime"].ToString();
                        }
                        catch (Exception)
                        {
                            record = null;
                        }
                    }
                    else
                    {
                        record = null;
                    }
                }

                return record;
            }
        }

        /// <summary>
        /// 插入一条新的记录(未测试)
        /// 历史记录表
        /// </summary>
        /// <param name="model">需要添加的新模型</param>
        /// <returns>插入表中自动生成的Id,-1代表失败</returns>
        public int AddHistoryRecord(HistoryModel model)
        {
            int id = -1;
            string sql = string.Format("Insert Into " + HistoryTableName +
                "(StartWork,IdlingWork, IdlingBreak, ReviseBegin,ReviseWork, ReviseEnd,DemWork,AdjWork,HighBreak,Tem,EquCode,EquType,HDate,HTime) Values" +
                "('{0}', '{1}', '{2}', '{3}','{4}', '{5}', '{6}', '{7}','{8}', '{9}','{10}','{11}','{12}','{13}')",
                model.StartWork, model.IdlingWork, model.IdlingBreak, model.ReviseBegin, model.ReviseWork, model.ReviseEnd, model.DemWork,
                model.AdjWork, model.HighBreak, model.Tem, model.EquCode, /*model.IconHex,*/ model.EquType , model.HDate , model.HTime);

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                //打开连接
                sqlConnect.Open();

                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = sql;

                //如果成功插入则代表成功
                if (command.ExecuteNonQuery() == 1)
                {
                    String getIdCommandString = "SELECT last_insert_rowid() FROM " + HistoryTableName;

                    SQLiteCommand getIdCommand = sqlConnect.CreateCommand();
                    getIdCommand.CommandText = getIdCommandString;

                    //读取刚刚插入的ID，赋值给模型
                    using (SQLiteDataReader reader = getIdCommand.ExecuteReader())
                    {
                        reader.Read();
                        id = reader.GetInt32(0);
                    }
                }
            }

            return id;
        }

        /// <summary>
        /// 删除数据(未测试)
        /// 历史记录表
        /// </summary>
        /// <param name="id">需要删除数据的id</param>
        /// <returns>代表是否修改成功</returns>
        public bool DeleteHistoryRecord(int id)
        {
            bool isSuccess = false;
            String sql = String.Format("DELETE FROM " + HistoryTableName + " WHERE Id='{0}'", id);

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                //打开连接
                sqlConnect.Open();

                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = sql;

                //如果成功插入则代表成功
                if (command.ExecuteNonQuery() == 1)
                {
                    isSuccess = true;
                }
            }

            return isSuccess;
        }
    }
}
