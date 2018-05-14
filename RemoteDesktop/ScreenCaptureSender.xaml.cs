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
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace RemoteDesktop
{
    /// <summary>
    /// ScreenCapture.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenCaptureSender : Window
    {
        /// <summary>
        /// Socket for this window to receive connection request.
        /// </summary>
        private Socket serverSocket;

        /// <summary>
        /// Socket for communication with client.
        /// </summary>
        private Socket clientSocket;

        /// <summary>
        /// A Timer to invoke TimeElapsed event.
        /// </summary>
        private System.Timers.Timer timer;

        /// <summary>
        /// Initializes the window for this window.
        /// </summary>
        public ScreenCaptureSender()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Click event handler for the capture button. Click this button will start to wait for connection from client, and screen captures will be sent once the connection is built.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdScreenCapture_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                InitializeSockets();
                InitializeTimer();
            });
            t.Start();
        }

        /// <summary>
        /// A test method here for testing the screen capture function as a standalong application.
        /// </summary>
        private void CaptureScreen()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), bmp.Size);
            BitmapImage screenCapture = BitmapToBitmapImage(bmp);

            bmp.Dispose();
            graphics.Dispose();

            imgScreenCapture.Source = screenCapture;
        }

        /// <summary>
        /// Convert Bitmap to BitmapImage.
        /// </summary>
        /// <param name="bitmap">The Bitmap instance that needs conversion.</param>
        /// <returns>A BitmapImage Instance converted from the specified Bitmap instance.</returns>
        private static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

        /// <summary>
        /// Initializes sockets for communication with client.
        /// </summary>
        private void InitializeSockets()
        {
            IPAddress ip = IPAddress.Parse("172.21.228.124");
            IPEndPoint localPoint = new IPEndPoint(ip, 16845);

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(localPoint);
            serverSocket.Listen(5);

            clientSocket = serverSocket.Accept();
        }

        /// <summary>
        /// Initializes timer.
        /// </summary>
        private void InitializeTimer()
        {
            timer = new System.Timers.Timer(100);
            timer.Elapsed += ElapsedHandler;
            timer.Start();
        }

        /// <summary>
        /// Sends screen captures to client when Elapsed event occured.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElapsedHandler(object sender, ElapsedEventArgs e)
        {
            // Get byte array of current screen capture.
            byte[] imageBytes = GetCurrentScreenCapture();

            // Send screen capture.
            clientSocket.Send(imageBytes);

            // Wait for client signal.
            clientSocket.Receive(new byte[5]);
        }

        /// <summary>
        /// Capture the current screen and returns its byte array representation.
        /// </summary>
        /// <returns>The byte array representation of curren screen capture.</returns>
        private byte[] GetCurrentScreenCapture()
        {
            // Initialize a Bitmap that has the exact size as user's screen.
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            // Initialize a graphics for this bitmap.
            Graphics graphics = Graphics.FromImage(bmp);

            // Capture current screen.
            graphics.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), bmp.Size);

            // Initialize a MemoryStream for serialize the bitmap.
            // Using MemoryStream here because it is faster than File I/O.
            MemoryStream ms = new MemoryStream();

            // Serialize contens of the bitmap to the MemoryStream.
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            // Get byte array representation of the bitmap.
            byte[] contentBytes = ms.GetBuffer();

            // Close the MemoryStream.
            ms.Close();

            // Close bitmap and graphics.
            bmp.Dispose();
            graphics.Dispose();

            // Returns the byte array representation of current screen capture.
            return contentBytes;
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
