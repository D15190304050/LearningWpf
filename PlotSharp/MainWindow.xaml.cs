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
using System.Windows.Markup;
using OxyPlot;

namespace PlotSharp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += LoaddHandler;
        }

        private void LoaddHandler(object sender, RoutedEventArgs e)
        {
            Line line = new Line();
            line.StrokeThickness = 4;
            line.Stroke = Brushes.Brown;
            line.X1 = 0;
            line.Y1 = 0;
            line.X2 = canvas.ActualWidth;
            line.Y2 = canvas.ActualHeight;
            IAddChild container = canvas;
            container.AddChild(line);
        }
    }
}
