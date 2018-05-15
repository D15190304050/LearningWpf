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
using System.Net.Sockets;
using System.Windows.Forms;

namespace RemoteDesktop
{
    /// <summary>
    /// UserCommandSender.xaml 的交互逻辑
    /// </summary>
    public partial class UserCommandSender : Window
    {
        private Socket clientSocket;

        public UserCommandSender()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //clientSocket.Connect(IPAddress.Parse("172.21.228.124"), 16845);
        }

        private void cmdSendMouseCommand_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
