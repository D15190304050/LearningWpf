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
using Microsoft.WindowsAPICodePack.Dialogs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using MessageBox = System.Windows.MessageBox;

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
        private string saveDirectory;

        public double CurrentBackgroundImagePixelWidth { get; private set; }

        public double CurrentBackgroundImagePixelHeight { get; private set; }

        public MainWindow()
        {
            trajectoryFileNames = null;
            currentTrajectoryIndex = 0;
            currentTrajectory = null;
            saveDirectory = null;

            InitializeComponent();
        }

        private void cmdResetBackgroundImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openImageDialog = new OpenFileDialog
            {
                Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|" +
                         "Windows Bitmap(*.bmp)|*.bmp|" +
                         "Windows Icon(*.ico)|*.ico|" +
                         "Graphics Interchange Format (*.gif)|(*.gif)|" +
                         "JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphics (*.png)|*.png|" +
                         "Tag Image File Format (*.tif)|*.tif;*.tiff",
                Title = "选择图片文件"
            };

            if (openImageDialog.ShowDialog() == true)
            {
                string fileName = openImageDialog.FileName;

                BitmapImage image = new BitmapImage(new Uri(fileName, UriKind.Absolute));
                backgroungImage.Source = image;

                this.CurrentBackgroundImagePixelWidth = image.PixelWidth;
                this.CurrentBackgroundImagePixelHeight = image.PixelHeight;
            }
        }

        private void cmdResetTrajectoryDataDirectory_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog openTrajectoryDataDirectoryDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "打开轨迹数据文件夹"
            };
            CommonFileDialogResult result = openTrajectoryDataDirectoryDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                string directoryName = openTrajectoryDataDirectoryDialog.FileName;

                trajectoryFileNames = Directory.GetFiles(directoryName);
                currentTrajectoryIndex = 0;

                // Clear the existing trajectory before showing another trajectory.
                imageCanvas.Children.Clear();
                currentTrajectory = new Trajectory(trajectoryFileNames[currentTrajectoryIndex]);
                currentTrajectory.Draw(imageCanvas, imageCanvas.ActualHeight * 16 / 9, imageCanvas.ActualHeight, this.CurrentBackgroundImagePixelWidth, this.CurrentBackgroundImagePixelHeight);
            }
        }

        private void cmdPreviousTrajectory_Click(object sender, RoutedEventArgs e)
        {
            if (currentTrajectoryIndex > 0)
            {
                // Clear the existing trajectory before showing another trajectory.
                imageCanvas.Children.Clear();
                currentTrajectoryIndex--;
                currentTrajectory = new Trajectory(trajectoryFileNames[currentTrajectoryIndex]);
                currentTrajectory.Draw(imageCanvas, imageCanvas.ActualHeight * 16 / 9, imageCanvas.ActualHeight, this.CurrentBackgroundImagePixelWidth, this.CurrentBackgroundImagePixelHeight);
            }
        }

        private void cmdNextTrajectory_Click(object sender, RoutedEventArgs e)
        {
            if (currentTrajectoryIndex < trajectoryFileNames.Length - 1)
            {
                // Clear the existing trajectory before showing another trajectory.
                imageCanvas.Children.Clear();
                currentTrajectoryIndex++;
                currentTrajectory = new Trajectory(trajectoryFileNames[currentTrajectoryIndex]);
                currentTrajectory.Draw(imageCanvas, imageCanvas.ActualHeight * 16 / 9, imageCanvas.ActualHeight, this.CurrentBackgroundImagePixelWidth, this.CurrentBackgroundImagePixelHeight);
            }
        }

        private void cmdSaveAnnotation_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(saveDirectory))
                MessageBox.Show("请先选择保存标注结果的文件夹");
            else
                SaveAnnotations();
        }

        private void cmdSetSavePath_Click(object sender, RoutedEventArgs e)
        {
            // Save current annotation result before start to annotate another dataset.
            if (!string.IsNullOrEmpty(saveDirectory))
                SaveAnnotations();

            CommonOpenFileDialog saveAnnotationDirectoryDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "选择保存标注结果的文件夹"
            };
            CommonFileDialogResult result = saveAnnotationDirectoryDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
                saveDirectory = saveAnnotationDirectoryDialog.FileName;
        }

        private void SaveAnnotations()
        {
            // Save as text file.
            string fileName = "Trajectory" + currentTrajectory.TrajectoryId + "Annotation.txt";
            fileName = System.IO.Path.Combine(saveDirectory, fileName);
            currentTrajectory.SaveAsText(fileName);

            // Save as JSON file.
            fileName = "Trajectory" + currentTrajectory.TrajectoryId + "Annotation.json";
            fileName = System.IO.Path.Combine(saveDirectory, fileName);
            currentTrajectory.SaveAsJson(fileName);

            // Save as XML file.
            fileName = "Trajectory" + currentTrajectory.TrajectoryId + "Annotation.xml";
            fileName = System.IO.Path.Combine(saveDirectory, fileName);
            currentTrajectory.SaveAsXml(fileName);
        }
    }
}
