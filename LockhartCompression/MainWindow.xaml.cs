using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.IO.Compression;
using System.Diagnostics;

namespace LockhartCompression
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private class TerminateInfo
        {
            public string EntirePath { get; private set; }

            public Process OpenProcess { get; private set; }

            public TerminateInfo(string entirePath, Process openProcess)
            {
                EntirePath = entirePath;
                OpenProcess = openProcess;
            }
        }

        private string startPath;
        private string zipPath;
        private string extractPath;
        private string startDirectory;
        private Dictionary<string, ZipArchiveEntry> items;
        private Stack<ZipArchive> zipArchiveTrace;

        /// <summary>
        /// Represents a package of compressed file in the zip archive format.
        /// Preserve this instance so that this program can visit ZipArchiveEntry instances inside without throwing exceptions.
        /// </summary>
        private ZipArchive archive;

        public MainWindow()
        {
            InitializeComponent();

            startPath = @"Q:\穆雨竹\Computer Science\Programming Language\C#\File IO";
            zipPath = @"Q:\穆雨竹\Computer Science\Programming Language\C#\File IO.zip";
            extractPath = @"Q:\穆雨竹\Computer Science\Programming Language\C#\extract\";
            startDirectory = @"Q:\穆雨竹\Computer Science\Programming Language\C#\";

            if (!File.Exists(zipPath))
                ZipFile.CreateFromDirectory(startPath, zipPath);

            items = new Dictionary<string, ZipArchiveEntry>();
            items.Add("..", null);
            archive = ZipFile.OpenRead(zipPath);
            foreach (ZipArchiveEntry entry in archive.Entries)
                items.Add(entry.ToString(), entry);

            contentList.ItemsSource = items.Keys;
        }

        private void ExtractTo(object sender, RoutedEventArgs e)
        {

        }

        private void ClickToExtract(object sender, RoutedEventArgs e)
        {
            string selection = contentList.SelectedItem == null ? "yes" : "no";
            currentDirectory.Text = selection;
        }

        private void Remove(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Clear the SelectedItems collection when user clicks on the TextBlock empty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void empty_MouseDown(object sender, MouseButtonEventArgs e)
        {
            contentList.SelectedItem = null;
        }

        private void contentList_DoubleClick(object sender, RoutedEventArgs e)
        {
            // Get the file name.
            ListBox contentList = (ListBox)sender;

            if (contentList.SelectedItem != null)
            {
                string selectedItemName = contentList.SelectedItem.ToString();
                OpenFile(selectedItemName);
            }
            else
                MessageBox.Show("Nothing selected...");
        }

        /// <summary>
        /// Open a file with the specified name.
        /// </summary>
        /// <param name="selectedItemName">The relevant path of the file from the extraction path.</param>
        private void OpenFile(string selectedItemName)
        {
            // Get the ZipArchiveEntry instance so that this program can extract content inside it.
            ZipArchiveEntry entry = items[selectedItemName];

            // Compute the entire path of the file.
            string entirePath = extractPath + selectedItemName;

            // Copy the contents from the ZipArchiveEntry instance to a new file with exact the same name.
            using (Stream entryFile = entry.Open())
            {
                // Exit if the file can't be read.
                if (entryFile.CanRead)
                {
                    // Create the file and close the FileStream if the file doesn't exist before.
                    if (!File.Exists(entirePath))
                        File.Create(entirePath).Close();

                    // Copy contents to target file.
                    using (Stream targetFile = new FileStream(entirePath, FileMode.Create))
                        entryFile.CopyTo(targetFile);
                }
            }

            // Start the Process to open the file.
            Process openProcess = Process.Start(entirePath);

            // Create an instance of The TerminateInfo class which is designed to pass the info
            // to delete the opened file when the openProcess exits.
            TerminateInfo terminate = new TerminateInfo(entirePath, openProcess);

            // Create a new thread to monitor the process and delete the opened file when the process exits.
            ThreadPool.QueueUserWorkItem((object o) =>
            {
                // Get the
                TerminateInfo terminateInfo = o as TerminateInfo;
                if ((terminateInfo == null) || (terminateInfo.OpenProcess == null))
                    return;

                // Do nothing if the process is running.
                while (!terminateInfo.OpenProcess.HasExited)
                    ;

                // Delete the file when the process exits.
                File.Delete(terminateInfo.EntirePath);
            }, terminate);
        }

        private void CreateDirectory(string fullPath)
        {

        }
    }
}