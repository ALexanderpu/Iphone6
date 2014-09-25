using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace iphone6
{
    /// <summary>
    /// SearchForm.xaml 的交互逻辑
    /// </summary>
    public partial class SearchForm : Window
    {
        Main mainForm;

        public SearchForm(Main mainForm, string Sch)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            //this.ControlBox = false;
            this.mainForm = mainForm;
            //this.StartPosition = FormStartPosition.Manual;
            //this.Location = new Point(482, 142);
            //this.button2.Enabled = false;
            ArrayList searchMsgList = mainForm.openForm.client.MyDBController.GetMessageListByKeyWord(Sch);
            for (int i = 0; i < searchMsgList.Count; i++)
            {
                ShortMessage msg = (ShortMessage)searchMsgList[i];
                listBox1.Items.Add(msg.GetContent());
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            mainForm.Show();
            mainForm.UpdateMainForm();
            this.Close();
            //this.Visible = false;
            //MainForm NewForm = new MainForm(mainForm.openForm.client);
            //NewForm.Show();
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBox1.SelectedIndex != -1)
            {
                mainForm.openForm.client.MyDBController.Delete(listBox1.SelectedItem.ToString());
                this.listBox1.Items.RemoveAt(listBox1.SelectedIndex);//删除项【数据库数据删除待写
                //this.button2.Enabled = false;
            }
        }

    }
}
