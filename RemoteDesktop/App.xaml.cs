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
        public const string CloseConnection = "[<Close/>]";

        public const int ServerPort = 16845;

        public const string ServerIPString = "172.21.228.70";
    }
}
