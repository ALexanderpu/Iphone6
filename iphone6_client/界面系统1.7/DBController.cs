using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iphone6
{
    public class DBController
    {
        SqlConnection conn { set; get; }
        String connString = @"server = ASUS;integrated security = true;database = MsgSimulator";
        String sql { set; get; }
        SqlCommand cmd { set; get; }
        SqlDataReader reader { set; get; }
        public String Owner;

        public DBController(String owner)
        {
            Owner = owner;
            ConnectToDB();
        }
        public void ConnectToDB()
        {
            conn = new SqlConnection(connString);
            conn.Open();
        }
        public void AddName(String num,String name)
        {
            reader.Close();
            //加号码
            sql = @"update Contact set Name = ('" + name + "') where Owner = '"+Owner+"' and PhoneNumber = '"+num+"'";
            cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

        }
        public String GetName(String num)
        {
            reader.Close();
            String result = "";
            sql = @"select Name from Contact where Owner = '" + Owner + " 'and  PhoneNumber = '" + num + "' ";
            cmd = new SqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader[0].ToString() == "")
                {
                    return "";
                }
                result = (String)reader[0];
                reader.Close();
                return result;
            }
            return result;
        }
        public void AddContact(String phoneNumber)
        {
            if(phoneNumber == "")
            {
                return;
            }
            sql = @"select * from Contact where PhoneNumber = '" + phoneNumber + "' and Owner = '" + Owner + "'";
            cmd = new SqlCommand(sql, conn);
            if (cmd.ExecuteScalar() != null)//如果该联系人已存在于联系人列表
            {
                return;
            }
            else
            {
                sql = @"insert into Contact(Owner,PhoneNumber,Name) values('" + Owner + "' , '" + phoneNumber + "','')";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
        public int GetMessageId(ShortMessage shortMessage)
        {
            int result = -1;
            sql = @"select Id from ShortMessage where Owner = '" + Owner + " 'and  SendNumber = '" + shortMessage.GetSendNumber() + "' and ReceiveNumber = '" + shortMessage.GetReceiveNumber() + "' and Time = '" + shortMessage.GetTime().ToString("yyyy/MM/dd HH:mm:ss") + "' and Content = '" + shortMessage.GetContent() + "'";
            cmd = new SqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result = (int)reader[0];
                reader.Close();
                return result;
            }
            return result;

        }
        public void Insert(ShortMessage msg)
        {
            reader.Close();
            String contactPhoneNumber;//对方联系人
            //sql = @"insert into [ShortMessage] (Time) values('"+msg.GetTime().ToString()+"')";
            if (msg.GetContent().Length < 70)//如果长度小于70字
            {
                sql = @"insert into ShortMessage(Owner, SendNumber, ReceiveNumber, Time, Content) values('" + Owner + "' , '" + msg.GetSendNumber() + "','" + msg.GetReceiveNumber() + "','" + msg.GetTime().ToString("yyyy/MM/dd HH:mm:ss") + "','" + msg.GetContent() + "')";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            else
            {
                int length = 70;
                int temp;
                for (int j = 0; j * length < msg.GetContent().Length; j++)
                {
                    temp = j * length;
                    if (msg.GetContent().Length - j * length < length)
                    {
                        sql = @"insert into ShortMessage(Owner, SendNumber, ReceiveNumber, Time, Content) values('" + Owner + "' , '" + msg.GetSendNumber() + "','" + msg.GetReceiveNumber() + "','" + msg.GetTime().ToString("yyyy/MM/dd HH:mm:ss") + "','" + " (" + (j + 1) + ") " + msg.GetContent().Substring(j * length, msg.GetContent().Length - j * length) + "')";
                        cmd = new SqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        sql = @"insert into ShortMessage(Owner, SendNumber, ReceiveNumber, Time, Content) values('" + Owner + "' , '" + msg.GetSendNumber() + "','" + msg.GetReceiveNumber() + "','" + msg.GetTime().ToString("yyyy/MM/dd HH:mm:ss") + "','" + " (" + (j + 1) + ") " + msg.GetContent().Substring(j * length, 70) + "')";
                        cmd = new SqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            if (msg.GetReceiveNumber() != Owner)//如果收件人不是我
            {
                contactPhoneNumber = msg.GetReceiveNumber();
            }
            else
            {
                contactPhoneNumber = msg.GetSendNumber();
            }
            AddContact(contactPhoneNumber);

        }
        public ArrayList GetContactList()
        {
            try
            {
                reader.Close();
            }
            catch { }
            ArrayList result = new ArrayList();
            sql = @"select * from Contact where Owner = '" + Owner + "'";
            cmd = new SqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(reader[2].ToString().Trim());
            }
            reader.Close();
            return result;
        }
        public ArrayList GetMessageListByPhoneNumber(String phoneNumber)
        {
            ArrayList result = new ArrayList();
            sql = @"select * from ShortMessage where Owner = '" + Owner + " 'and ( SendNumber = '" + phoneNumber + "' or ReceiveNumber = '" + phoneNumber + "' )";
            reader.Close();
            cmd = new SqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ShortMessage msg = new ShortMessage();
                msg.SetId((int)reader[0]);
                msg.SetSendNumber(reader[2].ToString().Trim());
                msg.SetReceiveNumber(reader[3].ToString().Trim());
                msg.SetTime(DateTime.Parse(reader[4].ToString().Trim()));
                msg.SetContent(reader[5].ToString().Trim());
                result.Add(msg);
            }
            reader.Close();
            return result;
        }
        public ArrayList GetMessageListByKeyWord(String keyWord)
        {
            reader.Close();
            ArrayList result = new ArrayList();
            sql = @"select * from ShortMessage where Owner = '" + Owner + "' and  content like '%" + keyWord + "%'";
            cmd = new SqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ShortMessage msg = new ShortMessage();
                msg.SetId((int)reader[0]);
                msg.SetSendNumber(reader[2].ToString().Trim());
                msg.SetReceiveNumber(reader[3].ToString().Trim());
                //msg.SetTime(DateTime.Parse(reader[4].ToString().Trim()));
                msg.SetContent(reader[5].ToString().Trim());
                result.Add(msg);
            }
            reader.Close();
            return result;

        }
        //public ArrayList Search(String str)
        //{

        //    ArrayList result = new ArrayList();
        //    sql = @"select * from ShortMessage where content like '%" + str + "%'";
        //    cmd = new SqlCommand(sql, conn);
        //    reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        ShortMessage msg = new ShortMessage();
        //        //Console.WriteLine("{0}",reader[0]);
        //        msg.SetId((int)reader[0]);
        //        msg.SetSendNumber(reader[1].ToString().Trim());
        //        msg.SetTime(DateTime.Parse(reader[2].ToString().Trim()));
        //        msg.SetContent(reader[3].ToString().Trim());
        //        result.Add(msg);
        //    }
        //    return result;
        //}
        public void Delete(String str)
        {
            sql = @"delete from ShortMessage where Owner = '" + Owner + "' and content like '%" + str + "%'";
            cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
        public void DeleteContact(String str)
        {
            sql = @"delete from Contact where Owner = '" + Owner + "' and PhoneNumber like '%" + str + "%'";
            cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}
