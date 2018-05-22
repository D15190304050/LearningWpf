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
using System.Timers;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace RemoteDesktop
{
    /// <summary>
    /// RemoteDesktopServer.xaml 的交互逻辑
    /// </summary>
    public partial class RemoteDesktopServer : Window
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
        /// The string representation of the received bytes array. It may contains a user command of close this socket.
        /// </summary>
        private string clientCommand;

        /// <summary>
        /// The buffer for receiving contents sent by the client.
        /// </summary>
        private byte[] receiveBuffer;

        /// <summary>
        /// A binary formatter for serializing and deserializing C# objects.
        /// </summary>
        private BinaryFormatter formatter;

        /// <summary>
        /// Initializes the window for remote desktop server.
        /// </summary>
        public RemoteDesktopServer()
        {
            receiveBuffer = new byte[1024];
            formatter = new BinaryFormatter();
            InitializeComponent();
        }

        /// <summary>
        /// Click event handler for the capture button. Click this button will start to wait for connection from client, and screen captures will be sent once the connection is built.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            // Initialize the thread for capturing screen.
            Thread tSendScreenCapture = new Thread(() =>
            {
                // Initialize socket.
                InitializeSockets();

                // Send the size of the server screen through client socket.
                SendScreenSize();

                // Initialize the thread for receiving command from client.
                // This thread initialization operation must be put here because the thread for capturing screen will initialize the client socket after the start of this thread.
                // So, put it here makes the sequence correct.
                Thread tExecuteCommand = new Thread(() =>
                {
                    for (; ; )
                    {
                        ExecuteCommand();
                        if (clientCommand == App.CloseConnection)
                            return;
                    }
                });
                tExecuteCommand.Start();

                InitializeTimer();
            });
            tSendScreenCapture.Start();
        }

        /// <summary>
        /// Sends the size of the server secreen through client socket.
        /// </summary>
        private void SendScreenSize()
        {
            // The 2nd new line character is only used to seperate the information of screen size and the potential 1st screen capture.
            string serverScreenSize = Screen.PrimaryScreen.Bounds.Width + "\n" + Screen.PrimaryScreen.Bounds.Height + "\n";
            clientSocket.Send(Encoding.UTF8.GetBytes(serverScreenSize));
        }

        /// <summary>
        /// Initializes sockets for communication with client.
        /// </summary>
        private void InitializeSockets()
        {
            // Try to get the real IP address of the server, the following IP address is just the IP address for this machine in the test environment.
            IPAddress ip = IPAddress.Parse(App.ServerIPString);
            IPEndPoint localPoint = new IPEndPoint(ip, App.ServerPort);

            // Initialize the server socket.
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(localPoint);
            serverSocket.Listen(5);

            // Wait for connection from the client.
            clientSocket = serverSocket.Accept();
        }

        /// <summary>
        /// Initializes timer.
        /// </summary>
        private void InitializeTimer()
        {
            // Send screen capture for every 100ms. I.E. send 10 images per second.
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

            try
            {
                // Send screen capture.
                clientSocket.Send(imageBytes);
            }
            catch (SocketException)
            {
                SocketExceptionHandler();
            }
            catch (ObjectDisposedException)
            {
                SocketExceptionHandler();
            }
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
            // Close the timer and sockets when closing this window.
            serverSocket.Close();
            timer.Close();
            clientSocket.Close();
        }

        /// <summary>
        /// Executes the command received from the client.
        /// </summary>
        private void ExecuteCommand()
        {
            try
            {
                // Receive the serialized user command.
                int receivedLength = clientSocket.Receive(receiveBuffer);

                // Try to view the bytes as a command string.
                clientCommand = Encoding.UTF8.GetString(receiveBuffer).Substring(0, receivedLength);
                if (clientCommand == "[<Close/>]")
                {
                    clientSocket.Close();
                    timer.Stop();
                    return;
                }

                // Put the bytes into a memory stream.
                MemoryStream ms = new MemoryStream(receiveBuffer, 0, receivedLength);

                // Execute the user command.
                // Note that for each UserCommand object, it needs a length of 512 B.
                for (int i = 1; ms.Position < ms.Length; i++)
                {
                    // Deserialize the next object in the memory stream.
                    UserCommandBase userCommand = (UserCommandBase)(formatter.Deserialize(ms));

                    // Execute the user command sent by the client.
                    userCommand.Execute();

                    // Reset the position of the memory stream.
                    ms.Position = i * 512;
                }
            }
            catch (SocketException)
            {
                SocketExceptionHandler();
            }
        }

        /// <summary>
        /// Releases resources when the client closes the socket.
        /// </summary>
        private void SocketExceptionHandler()
        {
            clientSocket.Close();
            timer.Stop();
        }
    }
}
