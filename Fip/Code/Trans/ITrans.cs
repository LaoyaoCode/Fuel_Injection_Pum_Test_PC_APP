using System;
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
            /// <summary>
            /// 连接命令，需要平台返回自身数据
            /// </summary>
            CONNECT ,
            /// <summary>
            /// 设置指令命令
            /// </summary>
            SET_PARA,
            /// <summary>
            /// 返回结果命令
            /// </summary>
            BACK_RESULT,
        }

       

        public delegate void ResultDel(bool result , String data , CommandEnum command = CommandEnum.NONE);
        private event ResultDel GetMessageEvent;

        public delegate void LostConnectDel();
        /// <summary>
        /// 失去连接代理函数
        /// </summary>
        private event LostConnectDel LostConnectEvent;

        /// <summary>
        /// 最大重发次数
        /// </summary>
        //private static readonly int MAX_RESEND_TIME = 5;

        /// <summary>
        /// 获取的数据列表
        /// </summary>
        private List<byte> RecieveDatas = new List<byte>();

        /// <summary>
        /// 解析数据返回的字典的key
        /// </summary>
        private static readonly String ANALYSE_KEY_COMMAND = "COMMAND";
        private static readonly String ANALYSE_KEY_DATA = "DATA";
        
        //发送的数据，缓存
        private ResultDel SendMDel = null;
        private String SendContent = null;
        private CommandEnum SendCommand = CommandEnum.NONE;
        /// <summary>
        /// 重新发送的次数
        /// </summary>
        private int ResendTimes = 0;
        /// <summary>
        /// 最大重发次数
        /// </summary>
        private int MAX_RESEND = 5;

        /// <summary>
        /// 是否已经连接上了
        /// </summary>
        private bool IsConnected = false;

        public ITrans()
        {
            
        }

        /// <summary>
        /// 增加失去连接代理
        /// </summary>
        /// <param name="del"></param>
        public void Add_LostConnectDel(LostConnectDel del)
        {
            LostConnectEvent += del;
        }


        /// <summary>
        /// 失去连接
        /// </summary>
        protected void LostConnect()
        {
            LostConnectEvent.Invoke();
        }
        /// <summary>
        /// 连接到设备函数
        /// </summary>
        /// <returns>null成功，有信息则是错误信息</returns>
        public abstract void ConnectToDeviceAsync(ResultDel del);

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

            message[message.Length - 5] = verify;
            //填装数据尾部
            for (int counter = message.Length - 1; counter >= message.Length - 4; counter--)
            {
                message[counter] = 0x00;
            }

            //开始异步操作
            await Task.Run(new Action(()=>
            {
                SendMDel = del;
                SendContent = content;
                SendCommand = command;
                ResendTimes = 0;

                lock(this)
                {
                    //如果发送成功
                    if (SendMessage_Real(message))
                    {
                        //那么接受数据时会处理此处的发送代理
                    }
                    //发送数据失败
                    else
                    {
                        if (del != null)
                        {
                            del.Invoke(false, "SNED_FAILED", CommandEnum.NONE);
                        }
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
                if ((CommandEnum)int.Parse(result[ANALYSE_KEY_COMMAND]) == CommandEnum.ANSWER_SUCCESS)
                {
                    SendMDel.Invoke(true, "SUCCESS", CommandEnum.NONE);
                }
                //命令为接受数据失败
                else if((CommandEnum)int.Parse(result[ANALYSE_KEY_COMMAND]) == CommandEnum.ANSWER_FAILED)
                {
                    //SendMDel.Invoke(false, "ANSWER_OUT_OF_TIME", CommandEnum.NONE);
                    //如果重发次数小于最大次数
                    if(ResendTimes < MAX_RESEND)
                    {
                        //重新发送数据
                        SendMeesageAsync(SendContent, SendCommand, SendMDel);
                        //重发次数自增
                        ResendTimes++;
                    }
                    else
                    {
                        //执行代理事件，发送讯息，标识失败，已经达到了最大重发次数
                        SendMDel.Invoke(false, "SEND_FAILED_MAX_RESEND", CommandEnum.NONE);
                    }
                    
                }
                //正常指令
                else
                {
                    //回复接受成功
                    SendMeesageAsync("", CommandEnum.ANSWER_SUCCESS, null);
                    //接受数据成功，则直接调用代理函数
                    GetMessageEvent.Invoke(true, result[ANALYSE_KEY_DATA], (CommandEnum)int.Parse(result[ANALYSE_KEY_COMMAND]));
                    
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
            for (int counter = 4; counter < datas.Length - 5; counter++)
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
