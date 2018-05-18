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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace RemoteDesktop
{
    /// <summary>
    /// UserCommandExecutor.xaml 的交互逻辑
    /// </summary>
    public partial class UserCommandExecutor : Window
    {
        private const int ReceiveBufferSize = 1024 * 2;

        private Socket clientSocket;
        private byte[] receiveBuffer;
        private string clientCommand;

        public UserCommandExecutor()
        {
            InitializeComponent();
        }

        private void ExecuteCommand()
        {
            int receivedLength = clientSocket.Receive(receiveBuffer);
            clientCommand = Encoding.UTF8.GetString(receiveBuffer).Substring(0, receivedLength);
            if (clientCommand == "[<Close/>]")
                return;

            MemoryStream ms = new MemoryStream(receiveBuffer, 0, receivedLength);
            BinaryFormatter formatter = new BinaryFormatter();

            //ms.Position = 0;
            for (int i = 1; ms.Position < ms.Length; i++)
            {
                UserCommandBase userCommand = (UserCommandBase)(formatter.Deserialize(ms));
                userCommand.Execute();

                ms.Position = i * 512;

                this.Dispatcher.Invoke(() => txtCommandExecutor.AppendText(userCommand.ToString() + "\n"));
            }
        }

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse("172.21.228.124"), 16845);
                Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(ip);
                serverSocket.Listen(5);

                clientSocket = serverSocket.Accept();
                receiveBuffer = new byte[ReceiveBufferSize];

                for (; ; )
                {
                    ExecuteCommand();
                    if (clientCommand == "[<Close/>]")
                        return;
                }
            });
            t.Start();
        }
    }
}
