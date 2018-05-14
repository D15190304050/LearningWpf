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
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Windows.Forms;

namespace RemoteDesktop
{
    /// <summary>
    /// ScreenCaptureReceiver.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenCaptureReceiver : Window
    {
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

        /// <summary>
        /// Initializes the window and other components for the client of remote desktop observation.
        /// </summary>
        public ScreenCaptureReceiver()
        {
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
        /// Starts to receive screen captures from remote server once the user clicked this button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdScreenCapture_Click(object sender, RoutedEventArgs e)
        {
            // Initialize the client socket and try to build a connection.
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(IPAddress.Parse("172.21.228.124"), 16845);

            // Start timer.
            timer.Start();

            // Disable this button once it is clicked.
            cmdScreenCapture.IsEnabled = false;
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

                // Send a character to signal the server that this client has received the screen capture correctly.
                clientSocket.Send(Encoding.UTF8.GetBytes("N"));

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
                System.Windows.MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            catch (NullReferenceException ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                timer.Close();
            }
        }

        /// <summary>
        /// Terminates the timer when closing this window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Close();
        }
    }
}
