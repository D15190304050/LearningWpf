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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

namespace SortingVisualization
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> algorithmNames;

        public MainWindow()
        {
            algorithmNames = new Dictionary<string, string>
            {
                { "Selection Sort", "SelectionSort"}
            };

            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button) e.OriginalSource;
            string windowName = cmd.Content.ToString();
            windowName = algorithmNames[windowName];

            Type type = this.GetType();
            Assembly assembly = type.Assembly;
            Window window = (Window)assembly.CreateInstance(type.Namespace + "." + windowName);

            window?.ShowDialog();
        }
    }
}
