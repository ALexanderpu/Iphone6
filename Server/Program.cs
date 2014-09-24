using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Server
{
    class Program
    {
        static TcpListener TcpServer;
        static void Main(string[] args)
        {
            //端口没有错误设置，必须输入正确
            #region 端口设置
            IPHostEntry ipe = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ServerIp = ipe.AddressList[1];
            Int32 Port;
            String temp = "13000";
            Port = int.Parse(temp);
            #endregion



            #region 初始化用户列表 和未读短信
            Singlephone.InitPhone();
            #endregion

            #region 开始服务
            NetworkStream stream = null;
            TcpServer = new TcpListener(ServerIp, Port);
            TcpServer.Start();
            Console.WriteLine("服务器开启成功~");
            Console.WriteLine("服务器IP:" + ServerIp + "   端口号" + Port);
            while (true)
            {
                //用来监听新用户的连接  并且用来产生阻塞
                TcpClient NewClient = TcpServer.AcceptTcpClient();
                //获得登陆信息
                stream = NewClient.GetStream();
                Byte[] bytes = new Byte[1024];
                String TempPhone;
                try
                {
                    int length = stream.Read(bytes, 0, bytes.Length);
                    TempPhone = System.Text.Encoding.UTF8.GetString(bytes, 0, length);
                }
                catch(Exception e)
                {
                    Console.WriteLine("登录出现问题");
                    continue;
                }
                bool flag = false;
                for (int i = 0; i < Singlephone.PhoneNum; i++)
                {
                    if (Singlephone.GetSinglePhone(i).UserPhone == TempPhone) //找到相同的
                    {
                        flag = true;
                        if (Singlephone.GetSinglePhone(i).GetStatus() == false)//表示曾经登陆过 
                        {
                           
                            Byte[] Msg = System.Text.Encoding.UTF8.GetBytes("Info -1 " + "LEGAL");
                            try
                            {
                                stream.Write(Msg, 0, Msg.Length);
                                Console.WriteLine("重新登录");
                                //先重新登录 再传入Client  
                                Singlephone.GetSinglePhone(i).Login(NewClient);  //重新登陆
                                //再开一个线程来处理和其交互消息
                                Thread NewThread = new Thread(Process);
                                NewThread.Start(Singlephone.GetSinglePhone(i));
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("连接出现问题");
                            }
                        }
                        else  //表示发生两个号码重复登陆 
                        {
                            Byte[] Msg = System.Text.Encoding.UTF8.GetBytes("Info -1 " + "LEGAL");
                            try
                            {
                                Console.WriteLine("号码重复非法");
                                stream.Write(Msg, 0, Msg.Length);
                                //可以允许
                                Singlephone.GetSinglePhone(i).Login(NewClient);
                                //再开一个线程来处理和其交互消息
                                Thread NewThread = new Thread(Process);
                                NewThread.Start(Singlephone.GetSinglePhone(i));
                            }
                            catch (Exception e) 
                            {
                                Console.WriteLine("连接出现问题");
                            }
                        }
                        break; //跳出循环
                    }
                }
                if (!flag) //如果没有以前记录
                {
                    Byte[] Msg = System.Text.Encoding.UTF8.GetBytes("Info -1 " + "LEGAL");
                    try
                    {
                        stream.Write(Msg, 0, Msg.Length);
                        //有了的话就新建立对象并进行登陆
                        Singlephone NewPhone = new Singlephone(NewClient, TempPhone);
                        //开线程处理
                        Thread NewThread = new Thread(Process);
                        NewThread.Start(NewPhone);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("连接出现问题");
                    }
                   
                }
            }
            #endregion
        }
        private static void Process(object Obj)
        {
            Singlephone Phone = (Singlephone)Obj;
            //线程内是处理客户消息的 chat~
            Chat NewChat = new Chat();
            NewChat.GetChat(Phone);
        }
    }
}
