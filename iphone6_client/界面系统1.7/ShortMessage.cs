using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iphone6
{
    public class ShortMessage
    {
        //标记这条短信
        private int Id;
        public void SetId(int id) { Id = id; }
        public int GetId() { return Id; }
        //发送方
        private string SendNumber;
        public String GetSendNumber()
        {
            return SendNumber;
        }
        public void SetSendNumber(string sendNumber) { SendNumber = sendNumber; }
        //接收方
        private string ReceiveNumber;
        public String GetReceiveNumber()
        {
            return ReceiveNumber;
        }
        public void SetReceiveNumber(string receiveNumber) { ReceiveNumber = receiveNumber; }
        //短信内容
        private string Content;
        private DateTime Time;
        public ShortMessage()
        {

        }
        public ShortMessage(String sendNumber, String receiveNumber, DateTime time, String content)//构造函数要进行初始化信息 
        {
            SendNumber = sendNumber;
            ReceiveNumber = receiveNumber;
            Time = time;
            Content = content;
        }
        public void SetContent(string content)//写入短信内容
        {
            Content = content + '\0';
        }
        public String GetContent()//读取短信内容 
        {
            String[] Msgs = Content.Split('\0');
            return Msgs[0];
            
        }
        public void SetTime(DateTime time)
        {
            Time = time;
        }
        public DateTime GetTime()
        {
            return Time;
        }

    }
}