using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Server
{
    class Chat
    {
        static ArrayList UnMessages = new ArrayList();
        public void GetChat(Singlephone phone) 
        {
            //登陆进来过后
            //（1）发送未发送的消息 查找收件人的未发送队列
            UnMessages.Insert(phone.Id, new UnMsg(phone.Id, phone.UserPhone));
            ((UnMsg)UnMessages[phone.Id]).InitUnMsg();// 从文件中读取出来
            if (UnMessages[phone.Id] != null)
                ((UnMsg)UnMessages[phone.Id]).SendUnMsg();

            //（2）处理接收到的短消息
            bool logoutflag = false;
            while(true)
            {
                if (logoutflag) { break; }
                String Msg = phone.Receive();
                if (Msg == "") { continue; }//空消息不处理
                if (Msg == null)//表示退出了
                {
                    phone.Logout();
                    break;
                }
                String[] Msgs = Msg.Split('\0');
                foreach (String msg in Msgs)  //对每一条客户端的消息
                {
                    //分割是用来定位发给谁的！！！！
                    String[] SegMent = msg.Split(' '); 
                     //编码方法 Id+发送方号码+接收方号码+日期+时间+内容
                    if (SegMent.Length < 6)//表示发送的编码有问题 
                    {
                        if (SegMent[0] == "logout") //如果是注销 进行判断
                        {
                            phone.Logout();
                            logoutflag = true;
                            break;
                        }
                        continue;
                    }
                       
                    //在这里实现定时短信发送，开一个线程实现
                    if (Convert.ToDateTime(SegMent[3] + " " + SegMent[4]).CompareTo(DateTime.Now) > 0)
                    {
                        Thread FurtherMsg = new Thread(SendFurtherMsg);
                        String Newmsg = msg + " " + phone.Id.ToString();
                        FurtherMsg.Start(Newmsg);
                        continue;
                    }
                    bool flag = false; //用来标志有没有 曾经登陆
                    for (int i = 0; i < Singlephone.PhoneNum; i++)
                    {
                        Singlephone ReceivePhone = Singlephone.GetSinglePhone(i);
                        //找到接收方
                        if (ReceivePhone.UserPhone == SegMent[2])
                        {
                            flag = true;
                            if (ReceivePhone.GetStatus() == true)
                            {
                                String SendMsg = msg;
                                if (ReceivePhone.Send(SendMsg))
                                {
                                    //向发送方返回短信状态 Info+Id+状态
                                    phone.Send("Info " + SegMent[2] + " SUCCESS");
                                }
                                else//表示发送有问题
                                {
                                    //向发送方返回短信状态
                                    phone.Send("Info " + SegMent[2] + " FAIL");
                                }
                            }
                            else //对方没有在线 加入发信箱
                            {
                                int Sid = ReceivePhone.Id;
                                String SendMsg = msg + " " + phone.Id.ToString();
                                ((UnMsg)UnMessages[Sid]).Add(SendMsg);
                            }
                            break;
                        }
                    }
                    if (!flag)  //没有注册 
                    {
                        Singlephone.GetSinglePhone(phone.Id).Send("Info " + SegMent[2] + " NOREGISTER");
                    }
                }
            }
        }

        private void SendFurtherMsg(object msg) 
        {
            //编码方法 Id+发送方号码+接收方号码+日期+时间+内容+发送方的Id  （多了一个）
            String[] Msgs = ((String)msg).Split(' ');
            DateTime SendTime = new DateTime();
            SendTime = DateTime.Parse(Msgs[3] + " " + Msgs[4]);
            while (SendTime.CompareTo(DateTime.Now) > 0)
            {
                Thread.Sleep(1000);//毫秒
            }
             //开始定位接收方
            bool flag = false;
            for (int i = 0; i < Singlephone.PhoneNum; i++)
            {
                Singlephone ReceivePhone = Singlephone.GetSinglePhone(i);
                //找到接收方
                if (ReceivePhone.UserPhone == Msgs[2])
                {
                    flag = true;
                    if (ReceivePhone.GetStatus() == true) //表示还在登陆
                    {
                        //发给收件方
                        String SendMsg = Msgs[0] + " " + Msgs[1] + " " + Msgs[2] + " " + Msgs[3] + " " + Msgs[4] + " " + Msgs[5];
                        ReceivePhone.Send(SendMsg);
                        //回执给发件方
                        int Sid = int.Parse(Msgs[Msgs.Length - 1]);
                        Singlephone.GetSinglePhone(Sid).Send("Info " + Msgs[2] + " SUCCESS");
                    }
                    else //对方没登陆 要缓存和回执 这里的回执一定要发件人登陆~
                    {
                        //缓存
                        int Id = ReceivePhone.Id;
                        ((UnMsg)UnMessages[Id]).Add((String)msg);
                        //回执
                        int id = int.Parse(Msgs[Msgs.Length - 1]);
                        Singlephone.GetSinglePhone(id).Send("Info " + Msgs[2] + " NOTONLINE");
                    }
                    break;
                }
            }
            if (!flag) 
            {
                int Sid = int.Parse(Msgs[Msgs.Length - 1]);
                Singlephone.GetSinglePhone(Sid).Send("Info " + Msgs[2] + " NOREGISTER");
            }

        }
    }
}
