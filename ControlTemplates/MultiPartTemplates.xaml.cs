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
    /// MultiPartTemplates.xaml 的交互逻辑
    /// </summary>
    public partial class MultiPartTemplates : Window
    {
        private string[] contents;

        public MultiPartTemplates()
        {
            contents = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve" };

            InitializeComponent();
            lb.ItemsSource = contents;
        }
    }
}
