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
        /// <summary>
        /// Initializes the launcher window.
        /// </summary>
        public RemoteDesktopLauncher()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Starts the remote desktop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdConnect_Click(object sender, RoutedEventArgs e)
        {
            // Start to connect to server if the user provides a correct IP address, otherwise, show error message.
            if (IPAddress.TryParse(txtRemoteIP.Text, out IPAddress targetIpAddress))
            {
                RemoteDesktopClient client = new RemoteDesktopClient(targetIpAddress);
                client.ShowDialog();
            }
            else
                MessageBox.Show("Please enter the correct IP address.");
        }

        /// <summary>
        /// Loads the server IP address of test environment stored in the App.xaml.cs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtRemoteIP.Text = App.ServerIPString;
        }
    }
}
