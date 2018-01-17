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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace VttToSrt
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            // Clear contents in the text box.
            txtPath.Text = "";
        }

        private void cmdTransit_Click(object sender, RoutedEventArgs e)
        {
            // Get the path.
            string path = txtPath.Text;

            // Initialize an instance of the directory that stores the original vtt files.
            DirectoryInfo originalDirectory = new DirectoryInfo(path);

            // Make the transiftion if the directory is correct.
            // Report errors otherwise.
            if (originalDirectory.Exists)
            {
                // Initialize an instance of the directory that will store subtitles.
                string subtitlePath = System.IO.Path.Combine(path, "subtitles");
                DirectoryInfo subtitleDirectory = new DirectoryInfo(subtitlePath);

                // Create the path to store subtitles if it doesn't exist now.
                if (!subtitleDirectory.Exists)
                    subtitleDirectory.Create();

                // Traverse all .vtt files in the original directory.
                foreach (FileInfo subtitleFile in originalDirectory.GetFiles("*.vtt"))
                {
                    // Extract contents in the subtitle file.
                    FileStream fs = subtitleFile.OpenRead();
                    StreamReader reader = new StreamReader(fs);
                    string subtitleContents = reader.ReadToEnd();
                    reader.Close();
                    fs.Close();

                    // Remove the first 6 letters.
                    subtitleContents = subtitleContents.Substring(6, subtitleContents.Length - 6);

                    // Get the name of the subtitle file.
                    // The FileInfo.Name property returns the full name of the file including the filename extension.
                    // So, we need to use String.Substirng() method to the the file name without the filename extension.
                    string subtitleFileName = subtitleFile.Name;
                    subtitleFileName = subtitleFileName.Substring(0, subtitleFileName.Length - 4);

                    // Rename the subtitle file end with ".srt".
                    subtitleFileName += ".srt";

                    // Get the path to store the .srt subtitle.
                    string subtitleFullPath = System.IO.Path.Combine(subtitlePath, subtitleFileName);

                    // Write subtitle contents to the .srt subtitle file.
                    fs = File.Create(subtitleFullPath);
                    StreamWriter writer = new StreamWriter(fs);
                    writer.Write(subtitleContents);
                    writer.Close();
                    fs.Close();
                }

                MessageBox.Show("Mession Success");
            }
            else
                MessageBox.Show("No such directory");
        }
    }
}
