using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;

namespace iphone6
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        MessageBox messageBox = new MessageBox();
        public Main mainForm;
        public Client client;
        public int IsLegal = -1;
        public Login()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        public bool IsNumber(string oText)         //判断输入的手机号码是否正确
        {
            for (int i = 0; i < oText.Length; i++)
            {
                if (oText[i] < '0' || oText[i] > '9')
                    return false;
            }
            return true;
        }
        public bool IsRightIP(string ip)               //判断输入的IP号码是否正确
        {
            if (Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
            {
                string[] ips = ip.Split('.');
                if (ips.Length == 4 || ips.Length == 6)
                {
                    if (System.Int32.Parse(ips[0]) < 256 && System.Int32.Parse(ips[1]) < 256 & System.Int32.Parse(ips[2]) < 256 & System.Int32.Parse(ips[3]) < 256)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string ph_number = PH.Text;
            string se_IP = SIP.Text;
            string temp = Port.Text;
            if (temp == "")
            {
                messageBox.Show("输入不正确，请重新输入。");

                return;
            }
            Int32 po_number = Int32.Parse(temp);
            if (IsNumber(ph_number) == false || ph_number.Length != 11)
            {
                messageBox.Show("输入不正确，请重新输入。");
                return;
            }

            else if (IsRightIP(se_IP) == false)
            {
                messageBox.Show("输入不正确，请重新输入。");
                return;
            }

            else if (po_number < 0 || po_number > 65535)
            {
                messageBox.Show("输入不正确，请重新输入。");
                return;
            }
            else
            {
                client = new Client(this, ph_number, se_IP, po_number);
                if (client.CreateConnection() != -1)
                {
                    client.Login();
                ////K: if (this.IsLegal == -1)
                ////    {
                ////        Thread.Sleep(300);
                ////        goto K;
                ////    }
                //    else if (this.IsLegal == 1)
                //    {
                        OpenMainForm();
                        this.IsLegal = -1;
                    //}

                }

            }

        }
        public void OpenMainForm()
        {
            mainForm = new Main(this);
            mainForm.Show();
            this.Hide();
            IsLegal = -1;
        }
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
