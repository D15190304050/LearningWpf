using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LabelImage
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public const double MaxZoomFactor = 3;
        public const double MinZoomFactor = 0.1;

        public static double CurrentZoomFactor { get; set; }

        static App()
        {
            App.CurrentZoomFactor = 1;

        }
    }
}
