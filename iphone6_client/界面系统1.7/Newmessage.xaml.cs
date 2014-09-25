using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;


namespace iphone6
{
    /// <summary>
    /// Newmessage.xaml 的交互逻辑
    /// </summary>
    public partial class Newmessage : Window
    {
        Main mainForm;
        MessageBox messageBox = new MessageBox();
        public Newmessage(Main mainForm)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //this.GetTime.Enabled = false;
            this.mainForm = mainForm;
            ArrayList contactList = mainForm.openForm.client.MyDBController.GetContactList();
            for (int i = 0; i < contactList.Count; i++)
            {
                LinknumberBox.Items.Add(contactList[i]);
            }
            this.textBox1.SelectionStart = this.textBox1.Text.Length;
            //this.textBox1.ScrollToCaret();
        }

        private void send_message_Click(object sender, RoutedEventArgs e)
        {
            ShortMessage shortMessage;
            if (NewmessageBox.Text == "")
            {
                messageBox.Show("不能发送空消息！");
                //this.StatusBar.Content = "不能发送空消息！";
                //MessageBox.Show("不能发送空消息！");
                return;
            }
            string phNum = textBox1.Text;
            
            String[] cmd = textBox1.Text.Split(' ');
            if (Timebox.IsChecked == true)
            {
                //DateTime time = DateTime.Parse(GetTime.SelectedDate.ToString() + ' ' + hour.Text + ' ' + min.Text + sec.Text);
                DateTime time;
                if(int.Parse(hour.Text)<0||int.Parse(hour.Text)>24||int.Parse(min.Text)<0||int.Parse(min.Text)>60||int.Parse(sec.Text)<0||int.Parse(sec.Text)>60)
                {
                    messageBox.Show("输入时间有误");
                    return;
                }
                try
                {
                    time = (DateTime)GetTime.SelectedDate;
                }
                catch 
                {
                    messageBox.Show("请输入日期！");
                    return;
                }
                time = time.AddHours((double.Parse(hour.Text)));
                time = time.AddMinutes((double.Parse(min.Text)));
                time = time.AddSeconds((double.Parse(sec.Text)));
                if (time < DateTime.Now)
                {
                    messageBox.Show("请选择将来的时间");
                    return;
                }
                else
                {
                    //ArrayList PhoneNumbers = new ArrayList();
                    for (int i = 0; i < cmd.Length; i++)
                    {
                        if (cmd[i].Length != 11)
                        {
                            if (cmd[i] == "")
                            {
                                continue;
                            }
                            //NumberForm numForm = new NumberForm();
                            messageBox.Show("号码格式错误！");
                            return;
                            //numForm.Show();
                        }
                        shortMessage = new ShortMessage(mainForm.openForm.client.GetPhoneNumber(), cmd[i], time, NewmessageBox.Text);
                        mainForm.openForm.client.MyDBController.Insert(shortMessage);
                        shortMessage.SetId(mainForm.openForm.client.MyDBController.GetMessageId(shortMessage));
                        mainForm.openForm.client.Send(shortMessage);
                    }
                    mainForm.UpdateMainForm();
                }
            }
            else
                for (int i = 0; i < cmd.Length; i++)
                {
                    if (cmd[i].Length != 11)
                    {
                        if(cmd[i] == "")
                        {
                            continue;
                        }
                        //NumberForm numForm = new NumberForm();
                        messageBox.Show("号码格式错误！");
                        return;
                        //numForm.Show();
                    }
                    shortMessage = new ShortMessage(mainForm.openForm.client.GetPhoneNumber(), cmd[i], DateTime.Now, NewmessageBox.Text);
                    mainForm.openForm.client.MyDBController.Insert(shortMessage);
                    shortMessage.SetId(mainForm.openForm.client.MyDBController.GetMessageId(shortMessage));
                    mainForm.openForm.client.Send(shortMessage);
                    mainForm.UpdateMainForm();
                }
            NewmessageBox.Clear();
            if (cmd.Length == 1)
            {
                DialogForm dialogForm = new DialogForm(mainForm,cmd[0]);
                dialogForm.Show();
                this.Hide();
            }
            
        }
        private void back_Click(object sender, RoutedEventArgs e)
        { 
            mainForm.Show();
            mainForm.UpdateMainForm();
            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //if (this.Timebox.IsChecked == false)
            //    this.GetTime.Enable = false;
            //else
            //    this.GetTime.Enabled = true;
        }

        private void LinknumberBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (textBox1.Text.Length == 0 || textBox1.Text[textBox1.Text.Length - 1].Equals(' '))
                textBox1.Text += LinknumberBox.SelectedItem.ToString() + " ";
            else
                textBox1.Text += " " + LinknumberBox.Text + " ";
        }

        private void NewmessageBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DateTimePickerObj_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
