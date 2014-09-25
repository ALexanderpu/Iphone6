using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Main : Window
    {
        public Login openForm;
        public DialogForm dialogForm;
        public ArrayList TempArrayList;
        //this.Dispatcher
        public Main(Login openForm)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //this.ControlBox = false;
            this.openForm = openForm;
            //this.StatusBar = openForm.StatusBar;
            //System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            //this.StartPosition = FormStartPosition.Manual;
            //this.Location = new Point(482, 142);
            this.MyNumber.Content = openForm.client.GetPhoneNumber();
            UpdateMainForm();
        }

        /*private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Newmessage nmassage = new Newmessage();
            nmassage.Show();
            this.Close();
        }*/
        public void UpdateMainForm()
        {
            String name;
            this.listBox1.Items.Clear();
            TempArrayList = openForm.client.MyDBController.GetContactList();
            for (int i = 0; i < TempArrayList.Count; i++)
            {
                name = openForm.client.MyDBController.GetName(TempArrayList[i].ToString());
                if (name!="")
                {
                    listBox1.Items.Add(name);
                }
                else
                    listBox1.Items.Add(TempArrayList[i]);
            }
        }



        private void shutdown_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mwindow = new MainWindow();
            mwindow.Show();
            this.Close();
        }
        private void search_Click(object sender, RoutedEventArgs e)
        {
            string Sch;
            Sch = SearchBox.Text;
            SearchForm NewForm = new SearchForm(this, Sch);
            NewForm.Show();
            this.Hide();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Newmessage nmassage = new Newmessage(this);
            nmassage.Show();
            this.Hide();

        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = null;
            if (listBox1.SelectedItem != null)
                str = listBox1.SelectedItem.ToString();
            bool btn = false;
            if (str != null)
                btn = true;
            if (btn == true)
            {
                dialogForm = new DialogForm(this);
                dialogForm.Show();
                this.Hide();
            }
            listBox1.SelectedItem = null;
        }
    }
}
