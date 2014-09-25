using System;
using System.Collections.Generic;
using System.Linq;
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

namespace iphone6
{
    /// <summary>
    /// detailForm.xaml 的交互逻辑
    /// </summary>
    public partial class detailForm : Window
    {
        public DialogForm dialogForm;

        public detailForm(DialogForm dialogForm)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //this.ControlBox = false;
            //this.StartPosition = FormStartPosition.Manual;
            //this.Location = new Point(482, 218);
            this.dialogForm = dialogForm;
            label1.Text = this.dialogForm.Messagebox.SelectedItem.ToString();
        }
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            delete delectForm = new delete(this);
            delectForm.Show();
            this.Hide();
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            dialogForm.Show();
            dialogForm.UpdateDialogForm();
            this.Close();
        }
    }
}
