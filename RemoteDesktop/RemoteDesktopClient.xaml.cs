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
using System.Drawing;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace RemoteDesktop
{
    /// <summary>
    /// RemoteDesktopClient.xaml 的交互逻辑
    /// </summary>
    public partial class RemoteDesktopClient : Window
    {
        private IPAddress targetIPAddress;

        /// <summary>
        /// A PictureBox for showing screen captures.
        /// </summary>
        /// <remarks>
        /// Using a WindowsForms control here is more effective since the byte array received from socket can be converted to Bitmap, which is exact the image source for the PictureBox.
        /// Than the transform from Bitmap to BitmapImage is unnecessary.
        /// </remarks>
        private PictureBox pb;

        /// <summary>
        /// A timer for invoke Elapsed event.
        /// </summary>
        private System.Timers.Timer timer;

        /// <summary>
        /// Socket for this window to communicate with remote server.
        /// </summary>
        private Socket clientSocket;

        /// <summary>
        /// The buffer for receiving bytes from socket.
        /// </summary>
        private byte[] receiveBuffer;

        private int serverScreenWidth;
        private int serverScreenHeight;

        private bool receiving;
        private BinaryFormatter formatter;

        public RemoteDesktopClient(IPAddress targetIPAddress)
        {
            this.targetIPAddress = targetIPAddress;
            receiving = true;
            formatter = new BinaryFormatter();

            InitializeComponent();

            // Get the PictureBox control.
            pb = (PictureBox)wfh.Child;

            // Set timer.
            timer = new System.Timers.Timer(100);
            timer.Elapsed += (o, e) => ReceiveScreenCapture();

            // Initialize the buffer for receiving bytes from socket.
            receiveBuffer = new byte[1024 * 1024 * 10];
        }

        /// <summary>
        /// Starts to receive screen captures from remote server once this windows is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize the client socket and try to build a connection.
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(targetIPAddress, App.ServerPort);

            //// Start timer.
            //timer.Start();

            Thread t = new Thread(() =>
            {
                ParseWidthAndHeight();

                while (receiving)
                    ReceiveScreenCapture();
            });

            t.Start();
        }

        /// <summary>
        /// Receives screen captures of remote server and shows them on the window.
        /// </summary>
        private void ReceiveScreenCapture()
        {
            try
            {
                // Receive the screen capture from socket.
                int receivedLength = clientSocket.Receive(receiveBuffer);

                // Initialize the MemoryStream to extract contents of receive buffer.
                MemoryStream ms = new MemoryStream();

                // Get effective contents in the receive buffer.
                ms.Write(receiveBuffer, 0, receivedLength);

                // Create a Bitmap from the MemoryStream.
                Bitmap screenCapture = new Bitmap(ms);

                // Close the MemoryStream.
                ms.Close();

                // Show the received screen capture.
                pb.Image = screenCapture;
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            catch (IOException ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            catch (ArgumentException ex)
            {
                //System.Windows.MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            catch (NullReferenceException ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                timer.Close();
            }
        }

        private void ParseWidthAndHeight()
        {
            int receivedLength = clientSocket.Receive(receiveBuffer);
            string widthAndHeight = Encoding.UTF8.GetString(receiveBuffer).Substring(0, receivedLength);
            string[] values = widthAndHeight.Split('\n');

            serverScreenWidth = int.Parse(values[0]);
            serverScreenHeight = int.Parse(values[1]);
        }

        /// <summary>
        /// Terminates the timer when closing this window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            receiving = false;
            clientSocket.Send(Encoding.UTF8.GetBytes("[<Close/>]"));
            clientSocket.Close();
        }

        private void PictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Debug.WriteLine($"Mouse Down at: ({e.X},{e.Y})");

            if ((e.X < 0) || (e.Y < 0))
                return;

            UserMouseCommand mouseCommand = null;

            if (e.Button == MouseButtons.Left)
                mouseCommand = new UserMouseCommand(e.X, e.Y, 0, MouseEventFlag.LeftDown);
            else if (e.Button == MouseButtons.Right)
                mouseCommand = new UserMouseCommand(e.X, e.Y, 0, MouseEventFlag.RightDown);

            // This null check is unnecessary in formal edition, only useful when developing.
            if (mouseCommand != null)
            {
                MemoryStream ms = new MemoryStream();

                formatter.Serialize(ms, mouseCommand);

                byte[] mouseCommandBytes = ms.GetBuffer();
                ms.Close();

                clientSocket.Send(mouseCommandBytes);
            }
        }

        private void PictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Debug.WriteLine($"Mouse Up at: ({e.X},{e.Y})");

            UserMouseCommand mouseCommand = null;

            int mouseX = Math.Abs(e.X);
            int mouseY = Math.Abs(e.Y);

            if (e.Button == MouseButtons.Left)
                mouseCommand = new UserMouseCommand(mouseX, mouseY, 0, MouseEventFlag.LeftUp);
            else if (e.Button == MouseButtons.Right)
                mouseCommand = new UserMouseCommand(mouseX, mouseY, 0, MouseEventFlag.RightUp);

            // This null check is unnecessary in formal edition, only useful when developing.
            if (mouseCommand != null)
            {
                MemoryStream ms = new MemoryStream();

                formatter.Serialize(ms, mouseCommand);

                byte[] mouseCommandBytes = ms.GetBuffer();
                ms.Close();

                clientSocket.Send(mouseCommandBytes);
            }

            //System.Windows.MessageBox.Show($"({e.X},{e.Y})");
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Key key = e.Key;
            Debug.WriteLine(key + " (" + e.SystemKey + ")");

            UserKeyboardCommand keyboardCommand = null;

            switch (key)
            {
                case Key.A:
                case Key.B:
                case Key.C:
                case Key.D:
                case Key.E:
                case Key.F:
                case Key.G:
                case Key.H:
                case Key.I:
                case Key.J:
                case Key.K:
                case Key.L:
                case Key.M:
                case Key.N:
                case Key.O:
                case Key.P:
                case Key.Q:
                case Key.R:
                case Key.S:
                case Key.T:
                case Key.U:
                case Key.V:
                case Key.W:
                case Key.X:
                case Key.Y:
                case Key.Z:
                    keyboardCommand = new UserKeyboardCommand(e.Key.ToString());
                    break;

                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                case Key.Add:
                case Key.Subtract:
                case Key.Multiply:
                case Key.Divide:
                case Key.F1:
                case Key.F2:
                case Key.F3:
                case Key.F4:
                case Key.F5:
                case Key.F6:
                case Key.F7:
                case Key.F8:
                case Key.F9:
                case Key.F10:
                case Key.F11:
                case Key.F12:
                case Key.F13:
                case Key.F14:
                case Key.F15:
                case Key.F16:
                case Key.Tab:
                case Key.Enter:
                case Key.Space:
                case Key.Delete:
                case Key.Home:
                case Key.NumLock:
                    keyboardCommand = new UserKeyboardCommand("{" + e.Key.ToString().ToUpper() + "}");
                    break;

                case Key.Back:
                    keyboardCommand = new UserKeyboardCommand("{BACKSPACE}");
                    break;

                case Key.Escape:
                    keyboardCommand = new UserKeyboardCommand("{ESC}");
                    break;

                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                    keyboardCommand = new UserKeyboardCommand(e.Key.ToString()[1].ToString());
                    break;

                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    keyboardCommand = new UserKeyboardCommand(e.Key.ToString()[5].ToString());
                    break;

                case Key.LeftCtrl:
                case Key.RightCtrl:
                    keyboardCommand = new UserKeyboardCommand("^");
                    break;

                case Key.LeftShift:
                case Key.RightShift:
                    keyboardCommand = new UserKeyboardCommand("+");
                    break;

                case Key.LeftAlt:
                case Key.RightAlt:
                    keyboardCommand = new UserKeyboardCommand("%");
                    break;

                case Key.LWin:
                case Key.RWin:
                    keyboardCommand = new UserKeyboardCommand("^{ESC}");
                    break;
            }

            ModifierKeys modifier = e.KeyboardDevice.Modifiers;


            if (modifier == (ModifierKeys.Control | ModifierKeys.Shift))
                keyboardCommand = new UserKeyboardCommand("^+");
            else if (modifier == (ModifierKeys.Control | ModifierKeys.Alt))
                keyboardCommand = new UserKeyboardCommand("^%");
            else if (modifier == (ModifierKeys.Alt | ModifierKeys.Shift))
                keyboardCommand = new UserKeyboardCommand("%^");
            else if ((keyboardCommand != null) &&
                     (modifier == ModifierKeys.Shift) &&
                     (key != Key.LeftShift) &&
                     (key != Key.RightShift))
                keyboardCommand = new UserKeyboardCommand("+" + keyboardCommand.Key);
            else if ((keyboardCommand != null) &&
                     (modifier == ModifierKeys.Control) &&
                     (key != Key.LeftCtrl) &&
                     (key != Key.RightCtrl))
                keyboardCommand = new UserKeyboardCommand("^" + keyboardCommand.Key);
            else if ((keyboardCommand != null) &&
                     (modifier == ModifierKeys.Alt) &&
                     (key != Key.LeftAlt) &&
                     (key != Key.RightAlt))
                keyboardCommand = new UserKeyboardCommand("%" + keyboardCommand.Key);

            if (keyboardCommand != null)
            {
                MemoryStream ms = new MemoryStream();

                formatter.Serialize(ms, keyboardCommand);

                byte[] keyboardCommandBytes = ms.GetBuffer();
                ms.Close();

                clientSocket.Send(keyboardCommandBytes);
            }
        }
    }
}
