using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.IO;
using System.Globalization;

namespace AdvancedDataBinding
{
    public class ImagePathConverter : IValueConverter
    {
        private string imageDirectory;
        public string ImageDirectory
        {
            get { return imageDirectory; }
            set { imageDirectory = value; }
        }

        public ImagePathConverter()
        {
            imageDirectory = Directory.GetCurrentDirectory();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagePath = Path.Combine(imageDirectory, (string)value);
            return new BitmapImage(new Uri(imagePath));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("This method or operation is not implemented");
        }
    }
}
