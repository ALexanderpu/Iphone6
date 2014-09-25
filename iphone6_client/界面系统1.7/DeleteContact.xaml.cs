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
    /// DeleteContact.xaml 的交互逻辑
    /// </summary>
    public partial class DeleteContact : Window
    {
        DialogForm dialogForm;
        ShortMessage msg;
        public DeleteContact(DialogForm dialogForm)
        {
            InitializeComponent();
            this.dialogForm = dialogForm;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dialogForm.messageList.Count; i++)
            {
                msg = (ShortMessage)dialogForm.messageList[i];
                dialogForm.mainForm.openForm.client.MyDBController.Delete(msg.GetContent());
            }
            dialogForm.mainForm.openForm.client.MyDBController.DeleteContact(dialogForm.PhoneNumber);
            dialogForm.UpdateDialogForm();
            dialogForm.mainForm.UpdateMainForm();
            dialogForm.mainForm.Show();
            this.Close();
        }
        private void no_Click(object sender, RoutedEventArgs e)
        {
            dialogForm.Show();
            this.Close();
        }
    }
}
