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
    /// Number.xaml 的交互逻辑
    /// </summary>
    public partial class Number : Window
    {
        public Number()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            //Main main = new Main();
            //main.Show();
            //this.Close();
        }
    }
}
