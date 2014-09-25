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
    /// delete.xaml 的交互逻辑
    /// </summary>
    public partial class delete : Window
    {
        public detailForm detailsForm;
        public delete(detailForm detailsForm)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.detailsForm = detailsForm;
        }
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            ShortMessage shortMessage = (ShortMessage)detailsForm.dialogForm.messageList[detailsForm.dialogForm.Messagebox.SelectedIndex];
            detailsForm.dialogForm.mainForm.openForm.client.MyDBController.Delete(shortMessage.GetContent());
            //detailsForm.dialogForm.mainForm.client.MyDBController.Delete(GetContent(detailsForm.dialogForm.Messagebox.SelectedItem.ToString()));
            detailsForm.dialogForm.Messagebox.Items.RemoveAt(detailsForm.dialogForm.Messagebox.SelectedIndex);
            detailsForm.Close();
            this.Close();
        }
        private void no_Click(object sender, RoutedEventArgs e)
        {
            detailsForm.Show();
            this.Hide();

        }
    }
}
