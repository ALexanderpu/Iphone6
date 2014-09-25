using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace iphone6
{
    /// <summary>
    /// Search.xaml 的交互逻辑
    /// </summary>
    public partial class DialogForm : Window
    {
        public String PhoneNumber;
        public Main mainForm;
        public ArrayList messageList;
        public MessageBox messageBox = new MessageBox(); 
        public DialogForm(Main mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            PhoneNumber = (String)mainForm.TempArrayList[mainForm.listBox1.SelectedIndex];
            //this.StartPosition = FormStartPosition.Manual;
            //this.Location = new Point(482, 142);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.SomeoneNum.Content = mainForm.listBox1.SelectedItem.ToString();
            UpdateDialogForm();
            
        }
        public DialogForm(Main mainForm, String phoneNumber)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            PhoneNumber = phoneNumber ;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //this.Location = new Point(482, 142);
            this.SomeoneNum.Content = phoneNumber;
            UpdateDialogForm();
        }
        public void UpdateDialogForm()
        {
            showMessage();
        }
        private void showMessage()
        {
            this.Messagebox.Items.Clear();
            messageList = mainForm.openForm.client.MyDBController.GetMessageListByPhoneNumber(this.PhoneNumber);
            if (messageList.Count == 0)
            {

            }
            for (int i = 0; i < messageList.Count; i++)
            {
                ShortMessage msg = (ShortMessage)messageList[i];
                //if(msg.GetContent().Length<999)//如果长度小于70字
                //{
                Messagebox.Items.Add("发送者：" + msg.GetSendNumber() + " 接收者：" + msg.GetReceiveNumber() + " 时间：" + msg.GetTime() + " 内容：" + msg.GetContent());
                //}
                //else
                //{
                ///MessageBox.Show("信息过长，无法显示！");
                //}
                //else
                //{
                //    int length = 70;
                //    int temp;
                //    for (int j = 0; j*length < msg.GetContent().Length; j++)
                //    {
                //        temp = j * length;
                //        if (msg.GetContent().Length - j * length <length)
                //            Messagebox.Items.Add("(" + (j + 1) + ")" + "发送者：" + msg.GetSendNumber() + " 接收者：" + msg.GetReceiveNumber() + " 时间：" + msg.GetTime() + " 内容：" + msg.GetContent().Substring(j * length, msg.GetContent().Length - j*length));
                //        else
                //            Messagebox.Items.Add("(" + (j+1) + ")" + "发送者：" + msg.GetSendNumber() + " 接收者：" + msg.GetReceiveNumber() + " 时间：" + msg.GetTime() + " 内容：" + msg.GetContent().Substring(j*length, 70));
                //    }
                //}
            }
            //SendMessage(Messagebox.Handle, SB_BOTTOM, SB_LINEDOWN, 0);
            //Messagebox.SelectedIndex = Messagebox.Items.Count - 1;
            //Messagebox.ClearSelected();
        }

        private void send_message_Click(object sender, RoutedEventArgs e)
        {
            if (ReplyBox.Text == "")
            {
                //this.StatusBar.Content = "不能发送空消息！";
                messageBox.Show("不能发送空消息！");
                return;
            }
            ShortMessage shortMessage = new ShortMessage(mainForm.openForm.client.GetPhoneNumber(), this.PhoneNumber, DateTime.Now, ReplyBox.Text);
            mainForm.openForm.client.MyDBController.Insert(shortMessage);
            shortMessage.SetId(mainForm.openForm.client.MyDBController.GetMessageId(shortMessage));
            mainForm.openForm.client.Send(shortMessage);

            //--------界面-----------
            //Messagebox.Items.Add(ReplyBox.Text);
            this.showMessage();
            ReplyBox.Clear();
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            mainForm.Show();
            mainForm.UpdateMainForm();
            this.Hide();
            this.Close();
        }

        private void Messagebox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Messagebox.SelectedIndex != -1)
            {
                detailForm detailsForm = new detailForm(this);
                detailsForm.Show();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeleteContact deleteContact = new DeleteContact(this);
            deleteContact.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(name.Text == "")
            {
                messageBox.Show("请输入昵称");
            }
            else
            {
                this.SomeoneNum.Content = name.Text;
                this.mainForm.openForm.client.MyDBController.AddName(PhoneNumber, name.Text);
            }
        }
    }
}
