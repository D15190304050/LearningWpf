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
    /// ColorPickerUserControlTest.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPickerUserControlTest : Window
    {
        public ColorPickerUserControlTest()
        {
            InitializeComponent();
        }

        

        private void cmdGet_Click(object sender, RoutedEventArgs e)
            => MessageBox.Show(colorPicker.Color.ToString());

        private void cmdReset_Click(object sender, RoutedEventArgs e)
            => colorPicker.Color = Colors.Beige;

        private void colorPicker_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (lblColor != null)
                lblColor.Text = "The new color is " + e.NewValue.ToString();
        }
    }
}
