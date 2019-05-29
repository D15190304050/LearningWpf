using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelImage
{
    public static class ApplicationContext
    {
        public const double MaxZoomFactor = 3;
        public const double MinZoomFactor = 0.1;
        public const int UIFontSizeDefaultValue = 16;
        public const int UIFontSizeMinValue = 1;
        public const int UIFontSizeMaxValue = 200;

        public static double CurrentZoomFactor { get; set; }
        public static Configuration Configuration { get; set; }

        static ApplicationContext()
        {
            ApplicationContext.CurrentZoomFactor = 1;

        }
    }
}
