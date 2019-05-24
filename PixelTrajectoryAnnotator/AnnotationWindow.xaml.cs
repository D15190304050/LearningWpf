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
using System.Windows.Shapes;

namespace PixelTrajectoryAnnotator
{
    /// <summary>
    /// AnnotationWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AnnotationWindow : Window
    {
        private readonly TrajectorySegment trajectorySegment;

        public AnnotationWindow(TrajectorySegment trajectorySegment)
        {
            this.trajectorySegment = trajectorySegment;

            InitializeComponent();
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            trajectorySegment.AnnotationInfo = txtAnnotation.Text;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, txtAnnotation);
        }
    }
}
