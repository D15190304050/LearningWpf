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
using Microsoft.Win32;
using System.IO;

namespace MusicPlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private LinkedList<Song> songs;
        private MediaPlayer musicPlayer;

        public MainWindow()
        {
            songs = new LinkedList<Song>();
            musicPlayer = new MediaPlayer();

            InitializeComponent();
        }

        private void cmdAddMusic_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                String fullFilePath = openFileDialog.FileName;
                songs.AddLast(new Song(fullFilePath));
                lstSongs.Items.Refresh();
            }
        }

        private void cmdPlay_Click(object sender, RoutedEventArgs e)
        {
            Song selectedSong = lstSongs.SelectedItem as Song;
            if (selectedSong != null)
            {
                musicPlayer.Open(new Uri(selectedSong.FullFilePath, UriKind.Absolute));
                musicPlayer.Play();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lstSongs.ItemsSource = songs;
        }
    }
}
