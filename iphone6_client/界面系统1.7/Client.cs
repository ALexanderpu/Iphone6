using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using Microsoft.Win32;

namespace iphone6
{
    public class Client
    {
        //System.Windows.MessageBox.Show
        MessageBox messageBox = new MessageBox();
        public int ShutDown = 0;
        public Thread recvThread;
        private String PhoneNumber;// 客户端的电话号码
        private NetworkStream stream;// 客户端的流
        private Int32 Port;// 服务器的端口号
        private String ServerIp;// 服务器的IP
        private ShortMessage Message;// 短消息容器
        private ArrayList Result;// 查找结果存储 
        public DBController MyDBController;// 数据库操作工具

        public Login openForm;
        public String GetPhoneNumber()
        {
            return PhoneNumber;
        }
        public Client()
        {
            //MyDBController = new DBController(phoneNumber);
        }
        public void SetStream(NetworkStream stream)
        {
            this.stream = stream;
        }
        public NetworkStream GetStream()
        {
            return stream;
        }
        ~Client()
        { }


        public Client(Login openForm, String phoneNumber, String serverIp, Int32 port)
        {
            this.openForm = openForm;
            PhoneNumber = phoneNumber;
            ServerIp = serverIp;
            Port = port;
            MyDBController = new DBController(phoneNumber);
        }
        public int CreateConnection()
        {
            try
            {
                TcpClient client = new TcpClient(ServerIp, Port);
                stream = client.GetStream();
                recvThread = new Thread(Receive);
                recvThread.IsBackground = true;
                recvThread.Start();
                recvThread.IsBackground = true;
                return 1;
            }
            catch (ArgumentNullException e)
            {
                messageBox.Show(e.ToString());
                return -1;
            }
            catch (SocketException e)
            {
                messageBox.Show(e.ToString());
                return -1;
            }
            finally
            {
                //return;
            }
        }
        public void Login()
        {
            SendMyPhoneNumber();
        }
        public void Send(String info)
        {
            Byte[] Data = new Byte[100000];
            Data = System.Text.Encoding.UTF8.GetBytes(info);
            stream.Write(Data, 0, Data.Length);
        }
        public void Send(ShortMessage message)
        {
            Byte[] Data = new Byte[100000];
            Data = ConvertShortMessageToByte(message);
            stream.Write(Data, 0, Data.Length);
            //System.Threading.Thread.Sleep(5000);
        }
        //public String ReceiveInfo()
        //{
        //    Byte[] Data = new byte[1000];
        //    int Len;
        //    try
        //    {
        //        Len = stream.Read(Data, 0, Data.Length);
        //        String ResponseData = System.Text.Encoding.UTF8.GetString(Data, 0, Len);
        //        string[] cmd = ResponseData.Split(' ');
        //        if (cmd[0] == "Info")//如果读到服务器回执 id(string) (SUCCESS　ＦＡＩＬ　ＮＯＲＥＧＩＳＴＥＲ　ＮＯＴＯＮＬＩＮＥ　ＤＥＬＡＹ)
        //        {
        //            //if (cmd[2] == "SUCCESS")
        //            //{
        //            //    MessageBox.Show("发送成功");
        //            //}
        //            //if (cmd[2] == "FAIL")
        //            //{
        //            //    MessageBox.Show("发送失败");
        //            //}
        //            //if (cmd[2] == "NOREGISTER")
        //            //{
        //            //    MessageBox.Show("发送号码为空号");
        //            //}
        //            //if (cmd[2] == "NOTONLINE")
        //            //{
        //            //    MessageBox.Show("对方已关机");
        //            //}
        //            //if (cmd[2] == "DELAY")
        //            //{
        //            //    MessageBox.Show("已设置定时发送");
        //            //}
        //            //if (cmd[2] == "ILLEGAL")
        //            //{
        //            //    MessageBox.Show("禁止重复登录！");
        //            //}
        //            //this.openForm.IsLegal = false;
        //            return cmd[2];
        //        }
        //        //else
        //        //{
        //        //    ShortMessage receiveMessage = new ShortMessage();
        //        //    receiveMessage = ConvertByteToShortMessage(Data, Len);
        //        //    MyDBController.Insert(receiveMessage);
        //        //    openForm.NewForm.UpdateMainForm();
        //        //    try
        //        //    {
        //        //        openForm.NewForm.form.UpdateDialogForm();
        //        //    }
        //        //    catch
        //        //    {

        //        //    }
        //        //}
        //        //return "";
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show("服务器崩溃");
        //    }
        //    return "";
        //}
        public delegate void DeleFunc(String str);
        public void Func(String str)
        {
            MessageBox messageBox = new MessageBox();
            messageBox.Show(str);
        }
        public void Receive()
        {

            //System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
             //                                           new DeleFunc(Func));
            //接收服务器的信息
            while (ShutDown == 0)
            {
                Byte[] Data = new byte[100000];
                int Len;
                try
                {
                    Len = stream.Read(Data, 0, Data.Length);
                }
                catch (Exception e)
                {
                    messageBox.Dispatcher.Invoke((Action)delegate { messageBox.Show("服务器崩溃"); });
                    System.Threading.Thread.Sleep(3000);
                    Environment.Exit(0);
                    //messageBox.Show("服务器崩溃");
                    break;
                }
                String ResponseData = System.Text.Encoding.UTF8.GetString(Data, 0, Len);
                string[] cmd = ResponseData.Split(' ');
                if (cmd[0] == "Info")//如果读到服务器回执 id(string) (SUCCESS　ＦＡＩＬ　ＮＯＲＥＧＩＳＴＥＲ　ＮＯＴＯＮＬＩＮＥ　ＤＥＬＡＹ)
                {
                    if (cmd[2] == "SUCCESS")
                    {
                        messageBox.Dispatcher.Invoke((Action)delegate { messageBox.Show("你发送给 " + cmd[1] + " 的短信已经安全送达"); });
                    }
                    if (cmd[2] == "FAIL")
                    {
                        messageBox.Dispatcher.Invoke((Action)delegate { messageBox.Show("发送失败"); });

                        //messageBox.Show("发送失败");
                    }
                    if (cmd[2] == "NOREGISTER")
                    {
                        //openForm.mainForm.dialogForm.StatusBar.Text = "发送号码为空号";
                        messageBox.Dispatcher.Invoke((Action)delegate { messageBox.Show("发送号码为空号"); });
                        //messageBox.Show("发送号码为空号");
                    }
                    if (cmd[2] == "NOTONLINE")
                    {
                        messageBox.Dispatcher.Invoke((Action)delegate { messageBox.Show("向 " + cmd[1] + " 发送的短信因对方已经关机尚未送达"); });
                        //messageBox.Show("对方已关机");
                    }
                    if (cmd[2] == "DELAY")
                    {
                        messageBox.Dispatcher.Invoke((Action)delegate { messageBox.Show("发送给 " + cmd[1] + " 的短信经过延迟已经安全送达"); });
                        //messageBox.Show("已设置定时发送");
                    }
                    //if (cmd[2] == "ILLEGAL")
                    //{
                    //    messageBox.Dispatcher.Invoke((Action)delegate { messageBox.Show("发送成功"); });
                    //    messageBox.Show("禁止重复登录！");

                    //    this.openForm.IsLegal = 0;
                    //}
                    if (cmd[2] == "LEGAL")
                    {
                        //openForm.OpenMainForm();
                        this.openForm.IsLegal = 1;
                    }
                    //else
                }
                else
                {
                    
                    ShortMessage receiveMessage = new ShortMessage();
                    receiveMessage = ConvertByteToShortMessage(Data, Len);
                    MyDBController.Insert(receiveMessage);
                    messageBox.Dispatcher.Invoke((Action)delegate { messageBox.Show("您有新短消息！    发送者：" + receiveMessage.GetSendNumber() + " 接收者：" + receiveMessage.GetReceiveNumber() + " 时间：" + receiveMessage.GetTime() + " 内容：" + receiveMessage.GetContent()); });
                    //Dispatcher.Invoke((Action)delegate
                    //{
                    openForm.Dispatcher.Invoke((Action)delegate
                    {
                        openForm.mainForm.UpdateMainForm();
                        try
                        {
                            openForm.mainForm.dialogForm.UpdateDialogForm();
                        }
                        catch
                        {

                        }
                    });
                   //})
                    
                }



            }
            //messageBox.Show("hehe");
            recvThread.Abort();
        }
        private void SendMyPhoneNumber()
        {
            //处理输出信息
            Byte[] data = new byte[100000];
            data = System.Text.Encoding.UTF8.GetBytes(PhoneNumber);
            //输出信息
            try
            {
                stream.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {

            }
            //Console.WriteLine("Sent: {0}", message);

            String responseData = String.Empty;
        }
        private ShortMessage ConvertByteToShortMessage(Byte[] data, int len)
        {
            String Time;
            ShortMessage Message = new ShortMessage();
            String MessageContent = "";
            String ResponseData = System.Text.Encoding.UTF8.GetString(data, 0, len);
            String[] Cmd = ResponseData.Split(' ');
            if(Cmd[0] == "")
            {
                messageBox.Dispatcher.Invoke((Action)delegate { messageBox.Show("通讯故障，服务器传入空串。"); });
                System.Threading.Thread.Sleep(3000);
                Environment.Exit(0);
            }
            Message.SetId((int.Parse(Cmd[0])));
            Message.SetSendNumber(Cmd[1]);
            Message.SetReceiveNumber(Cmd[2]);
            Time = Convert.ToDateTime(Cmd[3]) + " " + Convert.ToDateTime(Cmd[4]);
            Message.SetTime(Convert.ToDateTime(Cmd[3] + " " + Cmd[4]));
            for (int i = 5; i < Cmd.Length - 1; i++)
            {
                MessageContent += Cmd[i];
                MessageContent += " ";
            }
            MessageContent += Cmd[Cmd.Length - 1];
            Message.SetContent(MessageContent);
            return Message;
        }
        private Byte[] ConvertShortMessageToByte(ShortMessage shortMessage)
        {
            String Message;
            Message = (int)shortMessage.GetId() + " " + shortMessage.GetSendNumber() + " " + shortMessage.GetReceiveNumber() + " " + shortMessage.GetTime().ToString("yyyy/MM/dd HH:mm:ss") + " " + shortMessage.GetContent();
            Byte[] data = new byte[100000];
            data = System.Text.Encoding.UTF8.GetBytes(Message + '\0');
            return data;
        }

    }
}