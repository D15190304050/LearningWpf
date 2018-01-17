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

namespace CustomElements.CustomControlsClient
{
    /// <summary>
    /// ColorPickerTwoWays.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPickerTwoWays : Window
    {
        public ColorPickerTwoWays()
        {
            InitializeComponent();
        }

        private void cmdSetRed_Click(object sender, RoutedEventArgs e)
        {
            colorPicker1.Red = 127;
        }

        private void cmdSetGreen_Click(object sender, RoutedEventArgs e)
        {
            colorPicker1.Green = 127;
        }

        private void cmdSetBlue_Click(object sender, RoutedEventArgs e)
        {
            colorPicker1.Blue = 127;
        }
    }
}
