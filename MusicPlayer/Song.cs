using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;

namespace MusicPlayer
{
    public class Song
    {
        public string FullFilePath { get; }
        public string FileName { get; }

        public string Time { get; }

        public Song(string fullFilePath)
        {
            this.FullFilePath = fullFilePath;
            FileInfo file = new FileInfo(fullFilePath);
            this.FileName = file.Name;
            MediaPlayer mp = new MediaPlayer();
            mp.Open(new Uri(fullFilePath, UriKind.Absolute));
            Time = mp.NaturalDuration.ToString();
        }
    }
}
