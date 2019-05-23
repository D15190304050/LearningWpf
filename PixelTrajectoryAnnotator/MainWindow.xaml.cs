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
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace PixelTrajectoryAnnotator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] trajectoryFileNames;
        private int currentTrajectoryIndex;
        private Trajectory currentTrajectory;

        public MainWindow()
        {
            trajectoryFileNames = null;
            currentTrajectoryIndex = 0;
            currentTrajectory = null;

            InitializeComponent();
        }

        private void cmdResetBackgroundImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openImageDialog = new OpenFileDialog();
            openImageDialog.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|" +
            "Windows Bitmap(*.bmp)|*.bmp|" +
            "Windows Icon(*.ico)|*.ico|" +
            "Graphics Interchange Format (*.gif)|(*.gif)|" +
            "JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg|" +
            "Portable Network Graphics (*.png)|*.png|" +
            "Tag Image File Format (*.tif)|*.tif;*.tiff";
            openImageDialog.Title = "选择图片文件";

            if (openImageDialog.ShowDialog() == true)
            {
                string fileName = openImageDialog.FileName;

                backgroungImage.Source = new BitmapImage(new Uri(fileName, UriKind.Absolute));
            }
        }

        private void cmdResetTrajectoryDataDirectory_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openTrajectoryDataDirectoryDialog = new FolderBrowserDialog();
            DialogResult result = openTrajectoryDataDirectoryDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
            {
                string directoryName = openTrajectoryDataDirectoryDialog.SelectedPath;
                System.Windows.MessageBox.Show(directoryName);

                trajectoryFileNames = Directory.GetFiles(directoryName);
                currentTrajectoryIndex = 0;
            }
        }

        private void cmdPreviousTrajectory_Click(object sender, RoutedEventArgs e)
        {
            if (currentTrajectoryIndex > 0)
            {
                currentTrajectoryIndex--;
                currentTrajectory = new Trajectory(trajectoryFileNames[currentTrajectoryIndex]);
                currentTrajectory.Draw(imageCanvas, imageCanvas.ActualHeight * 16 / 9, imageCanvas.ActualHeight);
            }
        }

        private void cmdNextTrajectory_Click(object sender, RoutedEventArgs e)
        {
            if (currentTrajectoryIndex < trajectoryFileNames.Length - 1)
            {
                currentTrajectoryIndex++;
                currentTrajectory = new Trajectory(trajectoryFileNames[currentTrajectoryIndex]);
                currentTrajectory.Draw(imageCanvas, imageCanvas.ActualHeight * 16 / 9, imageCanvas.ActualHeight);
            }
        }

        private void cmdSaveAnnotation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSetSavePath_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveAnnotations()
        {
            
        }
    }
}
