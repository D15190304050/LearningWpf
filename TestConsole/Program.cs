using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace TestConsole
{
    public class Program
    {
        public static int Main(string[] args)
        {
            int[] data = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            foreach (int i in data)
                formatter.Serialize(ms, i);

            ms.Position = 0;
            while (ms.Position < ms.Length)
                Console.WriteLine(formatter.Deserialize(ms));

            ms.SetLength(0);
            ms.Close();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}
