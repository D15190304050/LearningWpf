using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net;

namespace TestConsole
{
    public class Program
    {
        public static int Main(string[] args)
        {
            IPAddress[] ips = Dns.GetHostAddresses("localhost");
            foreach (IPAddress ip in ips)
                Console.WriteLine(ip);

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}
