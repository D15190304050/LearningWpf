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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RemoteDesktop
{
    /// <summary>
    /// UserCommandSender.xaml 的交互逻辑
    /// </summary>
    public partial class UserCommandSender : Window
    {
        private const string MoveMouse = "Move Mouse";
        private const string MouseLeftClick = "Mouse Left Click";
        private const string MouseRightClick = "Mouse Right Click";

        private Socket clientSocket;
        private BinaryFormatter formatter;

        public UserCommandSender()
        {
            formatter = new BinaryFormatter();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(IPAddress.Parse("172.21.228.124"), 16845);
        }

        private void cmdSendMouseCommand_Click(object sender, RoutedEventArgs e)
        {
            UserMouseCommand mouseCommand = null;

            int mouseX = int.Parse(txtMouseX.Text);
            int mouseY = int.Parse(txtMouseY.Text);

            TextBlock txtMouseCommand = cbMouseCommand.SelectedItem as TextBlock;
            switch (txtMouseCommand.Text)
            {
                case MoveMouse:
                    mouseCommand = new UserMouseCommand(mouseX, mouseY, 0, MouseEventFlag.Move);
                    break;
                case MouseLeftClick:
                    mouseCommand = new UserMouseCommand(mouseX, mouseY, 0, MouseEventFlag.LeftDown | MouseEventFlag.LeftUp);
                    break;
                case MouseRightClick:
                    mouseCommand = new UserMouseCommand(mouseX, mouseY, 0, MouseEventFlag.RightDown | MouseEventFlag.RightUp);
                    break;
                default:
                    break;
            }

            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, mouseCommand);

            byte[] mouseCommandBytes = ms.GetBuffer();
            ms.Close();

            clientSocket.Send(mouseCommandBytes);
        }

        private void cmdSendKeyboardCommand_Click(object sender, RoutedEventArgs e)
        {
            string keysToSend = txtKeysToSend.Text;
            if (!string.IsNullOrEmpty(keysToSend))
            {
                // Send keys 1 by 1.
                //foreach (char c in keysToSend)
                //{
                //    UserKeyboardCommand keyboardCommand = new UserKeyboardCommand(c.ToString());

                //    MemoryStream ms = new MemoryStream();
                //    formatter = new BinaryFormatter();
                //    formatter.Serialize(ms, keyboardCommand);

                //    byte[] keyboardCommandBytes = ms.GetBuffer();
                //    ms.Close();

                //    clientSocket.Send(keyboardCommandBytes);
                //}

                //Send all keys together.
                UserKeyboardCommand keyboardCommand = new UserKeyboardCommand(keysToSend);
                MemoryStream ms = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, keyboardCommand);

                byte[] mouseCommandBytes = ms.GetBuffer();
                ms.Close();

                clientSocket.Send(mouseCommandBytes);

                FocusManager.SetFocusedElement(this, txtKeysToSend);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            clientSocket.Send(Encoding.UTF8.GetBytes("[<Close/>]"));
            clientSocket.Close();
        }
    }
}
