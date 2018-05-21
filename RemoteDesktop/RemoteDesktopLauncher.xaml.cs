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
using System.Net;

namespace RemoteDesktop
{
    /// <summary>
    /// RemoteDesktopClient.xaml 的交互逻辑
    /// </summary>
    public partial class RemoteDesktopLauncher : Window
    {
        public RemoteDesktopLauncher()
        {
            InitializeComponent();
        }

        private void cmdConnect_Click(object sender, RoutedEventArgs e)
        {
            if (IPAddress.TryParse(txtRemoteIP.Text, out IPAddress targetIpAddress))
            {
                RemoteDesktopClient client = new RemoteDesktopClient(targetIpAddress);
                client.ShowDialog();
            }
            else
                MessageBox.Show("Please enter the correct IP address.");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtRemoteIP.Text = App.ServerIPString;
        }
    }
}
