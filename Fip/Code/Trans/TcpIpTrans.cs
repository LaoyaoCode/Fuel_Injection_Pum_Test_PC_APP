using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Fip.Code.Trans
{
    /// <summary>
    /// 使用TCP IP来实现数据交流
    /// </summary>
    public class TcpIpTrans : ITrans
    {
        private static readonly IPAddress ServerIP = IPAddress.Parse("127.0.00.1");
        private static readonly int ServerPort = 22333;
        /// <summary>
        /// 是否已经连接完成
        /// </summary>
        private bool IsConnected = false;
        /// <summary>
        /// 接收数据线程
        /// </summary>
        private Thread R_Thread = null;
        /// <summary>
        /// TCP套接字
        /// </summary>
        private Socket TCPSocket = null;

        public TcpIpTrans()
        {
            
        }

        public override void ConnectToDeviceAsync(ResultDel del)
        {
            IPEndPoint endPoint = new IPEndPoint(ServerIP, ServerPort);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //尝试连接器件，通过TCP/IP模拟
                socket.Connect(endPoint);
            }
            catch
            {
                socket.Close();
                del.Invoke(false, "连接测试台失败，请检查线路是否连接正确!");
                return;
            }

            TCPSocket = socket;
            IsConnected = true;
            R_Thread = new Thread(new ThreadStart(RecieveDatas));
            R_Thread.Start();

            //发送指令，要求器件发送自身信息回来
            SendMeesageAsync("连接并且获取信息", CommandEnum.CONNECT, del);
        }

        protected override bool SendMessage_Real(byte[] message)
        {
            //没有正常连接，则直接错误
            if(!IsConnected)
            {
                return false;
            }

            try
            {
                TCPSocket.Send(message, message.Length, 0);
                return true;
            }
            catch
            {
                //发送失败，直接视为失去连接
                //失去连接，则关闭套接字，然后调用失去连接函数
                TCPSocket.Close();
                IsConnected = false;
                LostConnect();
                return false;
            }
        }


        private void RecieveDatas()
        {
            byte[] buffer = new byte[1024];

            while(true)
            {
                try
                {
                    int size = TCPSocket.Receive(buffer, buffer.Length , 0);
                    
                    if(size > 0 )
                    {
                        byte[] data = new byte[size];
                        for(int counter = 0; counter < size; counter++)
                        {
                            data[counter] = buffer[counter];
                        }

                        Add_Data(data);
                    }
                }
                catch(Exception e)
                {
                    //失去连接，则关闭套接字，然后调用失去连接函数，跳出无限循环
                    TCPSocket.Close();
                    IsConnected = false;
                    LostConnect();
                    break;
                }
            }
        }

        public override void Close()
        {
            if(TCPSocket != null)
            {
                //关闭套接字
                TCPSocket.Close();
            }
        }
    }
}
