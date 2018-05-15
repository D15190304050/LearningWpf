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

namespace RemoteDesktop
{
    /// <summary>
    /// UserCommandExecutor.xaml 的交互逻辑
    /// </summary>
    public partial class UserCommandExecutor : Window
    {
        private const int ReceiveBufferSize = 1024;

        private Socket clientSocket;
        private byte[] receiveBuffer;

        public UserCommandExecutor()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("172.21.228.124"), 16845);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ip);
            serverSocket.Listen(5);

            clientSocket = serverSocket.Accept();
            receiveBuffer = new byte[ReceiveBufferSize];
        }

        private void ExecuteCommand()
        {
            int receivedLength = clientSocket.Receive(receiveBuffer);
            //clientSocket
        }
    }
}
