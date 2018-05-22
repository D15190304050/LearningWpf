using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RemoteDesktop
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The command for closing the client socket.
        /// </summary>
        public const string CloseConnection = "[<Close/>]";

        /// <summary>
        /// The port for remote desktop connection.
        /// </summary>
        public const int ServerPort = 16845;

        /// <summary>
        /// The IP address of the server in the test environment.
        /// </summary>
        public const string ServerIPString = "172.21.228.70";
    }
}
