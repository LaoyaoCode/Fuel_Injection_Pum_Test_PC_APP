﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Fip.Code.Trans
{
    public abstract class ITrans
    {
        /// <summary>
        /// 指令枚举,不能超过255个
        /// </summary>
        public enum CommandEnum
        {
            NONE ,
            /// <summary>
            /// 回复错误，另一方接受错误数据
            /// </summary>
            ANSWER_FAILED ,
            /// <summary>
            /// 回复正确，另一方接收数据正确
            /// </summary>
            ANSWER_SUCCESS ,
        }

        public delegate void ResultDel(bool result , String data , CommandEnum command = CommandEnum.NONE);
        private ResultDel GetMessageEvent;
        /// <summary>
        /// 等待回复时间
        /// </summary>
        private static readonly int WAIT_ANSWER_TIME_MS = 100 ;
        /// <summary>
        /// 最大连接时间,MS计算
        /// </summary>
        private static readonly int CONNECT_MAX_TIME_MS = 3000;
        /// <summary>
        /// 最大重发次数
        /// </summary>
        private static readonly int MAX_RESEND_TIME = 5;

        /// <summary>
        /// 获取的数据列表
        /// </summary>
        private List<byte> RecieveDatas = new List<byte>();

        /// <summary>
        /// 解析数据返回的字典的key
        /// </summary>
        private static readonly String ANALYSE_KEY_COMMAND = "COMMAND";
        private static readonly String ANALYSE_KEY_DATA = "DATA";
        /// <summary>
        /// 互斥对象，用于阻塞发送线程直到收到返回的数据判断是否接受完全
        /// </summary>
        private Mutex AnswerMutex = new Mutex();
        /// <summary>
        /// 答复是否正确
        /// </summary>
        private bool IsAnwerRight = false;

        /// <summary>
        /// 异步连接设备
        /// </summary>
        /// <param name="del">结果代理函数</param>
        /// <returns>是否连接成功</returns>
        public void ConnectDeviceAutoAsync(ResultDel del)
        {

        }

        public async void SendMeesageAsync(String content , CommandEnum command , ResultDel del)
        {
            byte[] datas = Encoding.UTF8.GetBytes(content);
            byte[] message = new byte[datas.Length + 12];
            byte verify = 0x00;


            //填装数据头
            for(int counter = 0; counter < 4; counter++)
            {
                message[counter] = 0xff;
            }
            //填装命令
            message[4] = (byte)command;
            verify ^= message[4];

            //填装数据长度
            message[5] = (byte)(datas.Length / 256);
            message[6] = (byte)(datas.Length % 256);
            verify ^= message[5];
            verify ^= message[6];

            //填装数据
            for (int counter = 7; counter < message.Length - 5; counter++ )
            {
                message[counter] = datas[counter - 7];
                verify ^= message[counter];
            }

            message[message.Length - -5] = verify;
            //填装数据尾部
            for (int counter = message.Length - 1; counter >= message.Length - 4; counter--)
            {
                message[counter] = 0x00;
            }

            //开始异步操作
            await Task.Run(new Action(()=>
            {
                for(int counter = 0; counter < MAX_RESEND_TIME; counter++)
                {
                    //如果发送成功
                    if (SendMessage_Real(message))
                    {
                        //如果是接受数据失败或者接收数据成功的指令，则对面不需要返回是否接受成功
                        if (command == CommandEnum.ANSWER_FAILED || command == CommandEnum.ANSWER_SUCCESS)
                        {
                            return;
                        }


                        //阻塞线程，知道收到返回的数据
                        if (AnswerMutex.WaitOne(WAIT_ANSWER_TIME_MS))
                        {
                            //回信为成功接受
                            if(IsAnwerRight)
                            {
                                del.Invoke(true, "SUCCESS", CommandEnum.NONE);
                                break;
                            }
                            //回信为接受失败，则重复发送
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            //超时没有回复，则视为直接失去了连接
                            del.Invoke(false, "ANSWER_OUT_OF_TIME", CommandEnum.NONE);
                            break;
                        }
                    }
                    //发送数据失败
                    else
                    {
                        del.Invoke(false, "SNED_FAILED", CommandEnum.NONE);
                        break;
                    }
                }
                
            })) ;
        }

        /// <summary>
        /// 同步发送数据,真正处理发送数据的地方，只需要将字节全部发送过去
        /// </summary>
        /// <param name="message">字符数据</param>
        /// <returns>是否传输成功，数据是否发送完全</returns>
        protected abstract bool SendMessage_Real(byte[] message);

        /// <summary>
        /// 增加获取数据时候的代理
        /// </summary>
        /// <param name="del"></param>
        public void Add_GetMeeageDel(ResultDel del)
        {
            GetMessageEvent += del;
        }

        /// <summary>
        /// 增加数据，子代在获得数据之后只需要将数据作为参数传递到此函数就好
        /// </summary>
        /// <param name="data"></param>
        protected void Add_Data(byte[] data)
        {
            //添加数据
            RecieveDatas.AddRange(data);
            //检查数据
            CheckDatas();
        }

        /// <summary>
        /// 检查数据是否已经接受完全，即是否已经接受到了数据头和数据尾，如果是则开始分析数据
        /// </summary>
        private void CheckDatas()
        {
            Dictionary<String, String> result = null;

            //长度不够
            if (RecieveDatas.Count < 12)
            {
                return;
            }
            //检查头
            for(int counter = 0; counter < 4; counter++)
            {
                if (RecieveDatas[counter] != 255)
                {
                    return;
                }
            }

            //检查尾
            for(int counter = RecieveDatas.Count - 1; counter >= RecieveDatas.Count - 4; counter--)
            {
                if (RecieveDatas[counter] != 0)
                {
                    return;
                }
            }

            result = AnalysisData(RecieveDatas.ToArray());
            RecieveDatas.Clear();

            //接受错误数据
            if(result == null)
            {
                //回复接受失败
                SendMeesageAsync("", CommandEnum.ANSWER_FAILED, null);
            }
            //接受到了数据
            else
            {
                //如果接收到是对方的回信指令，则无需回信
                //命令为接收数据成功
                if((CommandEnum)int.Parse(result[ANALYSE_KEY_COMMAND]) == CommandEnum.ANSWER_SUCCESS)
                {
                    IsAnwerRight = true;
                    AnswerMutex.ReleaseMutex();
                }
                //命令为接受数据失败
                else if((CommandEnum)int.Parse(result[ANALYSE_KEY_COMMAND]) == CommandEnum.ANSWER_FAILED)
                {
                    IsAnwerRight = false;
                    AnswerMutex.ReleaseMutex();
                }
                //正常指令
                else
                {
                    //接受数据成功，则直接调用代理函数
                    GetMessageEvent.Invoke(true, result[ANALYSE_KEY_DATA], (CommandEnum)int.Parse(result[ANALYSE_KEY_COMMAND]));
                    //回复接受成功
                    SendMeesageAsync("", CommandEnum.ANSWER_SUCCESS, null);
                }
            }
        }

        /// <summary>
        /// 在检测到数据头和数据尾之后，开始调用处理数据函数
        /// </summary>
        /// <returns>校验数据是否成功,成功返回内容和命令 ， 不正确返回null</returns>
        private Dictionary<String , String> AnalysisData(byte[] datas)
        {
            //命令类型
            byte command = datas[4];
            byte verify = 0;
            Dictionary<String, String> result = new Dictionary<string, string>();

            //校验数据
            for (int counter = 5; counter < datas.Length - 5; counter++)
            {
                verify ^= datas[counter];
            }

            //如果校验失败
            if( !(verify == datas[datas.Length - 5]) )
            {
                return null;
            }

            result.Add(ANALYSE_KEY_COMMAND, command.ToString());
            result.Add(ANALYSE_KEY_DATA, Encoding.UTF8.GetString(datas, 7, datas[5] * 256 + datas[6]));

            return result;
        }
    }
}
