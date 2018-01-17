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

namespace ControlTemplates
{
    /// <summary>
    /// SimpleCustomButton.xaml 的交互逻辑
    /// </summary>
    public partial class SimpleCustomButton : Window
    {
        public SimpleCustomButton()
        {
            InitializeComponent();
        }

        private void cmd_Clicked(object sender, RoutedEventArgs e)
            => MessageBox.Show("You clicked" + ((Button)e.OriginalSource).Name);
    }
}
