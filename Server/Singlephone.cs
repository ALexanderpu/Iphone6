using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Server
{
    public class Singlephone
    {
        public static int PhoneNum = 0; //曾登陆的手机数量
        public String UserPhone { private set; get; } //当前连入手机的电话号码

        static ArrayList AllPhoneList = new ArrayList();
        public int Id { private set; get; }//用户在静态List中的位置
        public TcpClient client { private set; get; }

        NetworkStream stream = null;

        private bool Isonline;

        private Singlephone() 
        {
            Isonline = false;
            PhoneNum++;
        }
        public bool GetStatus() { return Isonline; }//返回是否登陆
        public void setUserPhone(String phone) { UserPhone = phone; }
        public Singlephone(TcpClient cl,String tempPhone) //初始化一个对象 新建一个连接 初始化就得登陆
        {
            client = cl;
            stream = client.GetStream();
            UserPhone = tempPhone;
            Id = PhoneNum++;
            AllPhoneList.Insert(Id, this);
            Isonline = true;
            Console.WriteLine(Id.ToString() + " " + UserPhone + " 已经登陆！");

            String Info = Id.ToString() + " " + UserPhone;
            FileStream file;
            try
            {
                file = new FileStream("User.txt", FileMode.Append, FileAccess.Write, FileShare.None);//已追加的形式打开  同时锁住
                StreamWriter sw = new StreamWriter(file);//打开后建立写入流
                sw.WriteLine(Info);
                sw.Close();
                file.Close();
                Console.WriteLine(Id.ToString() + " " + UserPhone + " 信息备份成功");

            }
            catch (Exception e)
            {
                Console.WriteLine("备份用户信息出现问题");
            }
        }

        public void Login(TcpClient cl)   
        {
            client = cl;
            stream = client.GetStream();
            Isonline = true;
            Console.WriteLine(Id.ToString() + " " + UserPhone + " 已经登陆！");
        }
        public void Logout()
        {
            Isonline = false;
            client = null;
            Console.WriteLine(PhoneNum.ToString() + " 已经离线！");
        }

        //传过来一个String  编码方式已经给定了
        public bool Send(String msg) 
        {
            Byte[] Msg = System.Text.Encoding.UTF8.GetBytes(msg); 
            try
            {
                stream.Write(Msg, 0, Msg.Length);
                Console.WriteLine("向客户端 " + UserPhone + " 发送：" + msg);
                Thread.Sleep(50);
                return true;
            }
            catch
            {
                Console.WriteLine("发送失败！");
                return false;
            }
        }

        //返回的是一个String  如果没有接收到就是null需要判断一下 已经有了在 GetChat()
        public String Receive()  
        {
            Byte[] bytes = new Byte[1024];
            String rec = null;
            try
            {
                int length = stream.Read(bytes, 0, bytes.Length);
                rec = System.Text.Encoding.UTF8.GetString(bytes, 0, length);
                if (rec.Length != 0)
                {
                    Console.WriteLine("接收到 " + UserPhone + " 传过来的数据 " + rec);
                }
            }
            catch (Exception e) //接收有问题就判断已经断线
            {
                Logout();
                Console.WriteLine(UserPhone + " 发生中断");
            }
            return rec;
        }

        public static Singlephone GetSinglePhone(int index) 
        {
            if (index >= AllPhoneList.Count)
            {
                Console.WriteLine("你查找的用户超出了列表范围");
                return null;
            }
            else 
                return AllPhoneList[index] as Singlephone;
        }


        public static void InitPhone()
        {
            FileStream file = new FileStream("User.txt", FileMode.OpenOrCreate);
            StreamReader s = new StreamReader(file);
            String list = null; 
            while((list = s.ReadLine())!= null)
            {
                String[] PhoneList = list.Split(' '); //以某一个字符分割
                int i = int.Parse(PhoneList[0]);  //第一个表示ID
                String phone = PhoneList[1];
                AllPhoneList.Insert(i, new Singlephone()); //进行插入操作
                Singlephone single = AllPhoneList[i] as Singlephone;
                single.Id = i;
                single.UserPhone = phone;
            }
            file.Close();
            s.Close();
        }
    }
}
