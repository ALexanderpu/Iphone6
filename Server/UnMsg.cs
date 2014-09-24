using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Server
{
    class UnMsg //收件人信箱 要按编码方式往里面存
    {
        public String ReceivePhone { private set; get; }
        public int Id { private set; get; }
        //存的内容是编码  （0）短信Id+（1）发送方号码+（2）接收方号码+（3）日期+（4）时间+（5）内容+（6）发送方Id
        private List<String> MsgBox = new List<String>();
        public UnMsg(int id, String Phone) { Id = id; ReceivePhone = Phone; }

        public void SendUnMsg() 
        {
            Singlephone RPhone = Singlephone.GetSinglePhone(Id);//在队列中获得收件人信息
            //对于每一个短信
            for (int i = 0; i < MsgBox.Count; i++) 
            {
                //先发出去
                RPhone.Send(MsgBox[i]);
                //再发回执
                String[] Msgs = MsgBox[i].Split(' ');
                int SendId = int.Parse(Msgs[Msgs.Length - 1]);//最后是发送人的Id
                Singlephone SPhone = Singlephone.GetSinglePhone(SendId);
                if(SPhone != null)
                {
                    SPhone.Send("Info " + Msgs[2] + " DELAY"); //短信编号
                }
                Thread.Sleep(100);
            }
            MsgBox.Clear();
            FileStream file = new FileStream(RPhone.Id.ToString(), FileMode.Create);  //这个相当于清空吧
            file.Close();
        }

        public void InitUnMsg() 
        {
            FileStream file = new FileStream(Id.ToString(), FileMode.OpenOrCreate); //发件人的ID
            StreamReader s = new StreamReader(file);
            String SendMsg;
            while ((SendMsg = s.ReadLine()) != null)
            {
                MsgBox.Add(SendMsg);//加入未发送内存
            }
            file.Close();
            s.Close();
        }

        public void Add(String newMsg)
        {
            //内存中加入
            MsgBox.Add(newMsg);
            //文件中加入
            FileStream file;
            try
            {
                file = new FileStream(Id.ToString(), FileMode.Append, FileAccess.Write, FileShare.None);//已追加的形式打开  同时锁住
            }
            catch (Exception e)
            {
                Console.WriteLine("写入文件出现错误" + e);
                return;
            }
            StreamWriter sw = new StreamWriter(file);//打开后建立写入流
            sw.WriteLine(newMsg);
            sw.Close();
            file.Close();
        }
    }
}
