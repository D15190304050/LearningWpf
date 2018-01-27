using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AdvancedDataBinding
{
    public class PriceToBackgroundConverter : IValueConverter
    {
        public decimal MinPriceToHighlight { get; set; }
        public Brush HighlightBrush { get; set; }
        public Brush DefaultBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal price = (decimal)value;
            if (price >= this.MinPriceToHighlight)
                return this.HighlightBrush;
            else
                return this.DefaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
