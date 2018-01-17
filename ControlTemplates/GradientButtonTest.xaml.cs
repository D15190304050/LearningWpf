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
    /// GradientButtonTest.xaml 的交互逻辑
    /// </summary>
    public partial class GradientButtonTest : Window
    {
        ResourceDictionary blueTheme;
        ResourceDictionary greenTheme;

        public GradientButtonTest()
        {
            InitializeComponent();

            blueTheme = new ResourceDictionary();
            blueTheme.Source = new Uri("Resources/GradientButton.xaml", UriKind.Relative);
            greenTheme = new ResourceDictionary();
            greenTheme.Source = new Uri("Resources/GradientButtonVariant.xaml", UriKind.Relative);
        }

        private void Clicked(object sender, RoutedEventArgs e)
            => MessageBox.Show("You clicked: " + ((Button)sender).Name);

        private void chkGreen_Checked(object sender, RoutedEventArgs e)
            => this.Resources.MergedDictionaries[0] = greenTheme;

        private void chkGreen_Unchecked(object sender, RoutedEventArgs e)
            => this.Resources.MergedDictionaries[0] = blueTheme;
    }
}
