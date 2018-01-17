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

namespace AdvancedAnimations
{
    /// <summary>
    /// FrameBasedAnimation.xaml 的交互逻辑
    /// </summary>
    public partial class FrameBasedAnimation : Window
    {
        private bool rendering;
        private List<EllipseInfo> ellipses;
        private double accelerationY;
        private int minStartSpeed;
        private int maxStartSpeed;
        private double speedRatio;
        private int minEllipses;
        private int maxEllipses;
        private int ellipseRadius;

        public FrameBasedAnimation()
        {
            rendering = false;
            ellipses = new List<EllipseInfo>();
            accelerationY = 0.1;
            minStartSpeed = 1;
            maxStartSpeed = 50;
            speedRatio = 0.1;
            minEllipses = 20;
            maxEllipses = 100;
            ellipseRadius = 10;

            InitializeComponent();
        }

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            if (!rendering)
            {
                ellipses.Clear();
                canvas.Children.Clear();

                CompositionTarget.Rendering += RenderFrame;
                rendering = true;
            }
        }

        private void cmdStop_Click(object sender, RoutedEventArgs e)
        {
            StopRendering();
        }

        private void StopRendering()
        {
            CompositionTarget.Rendering -= RenderFrame;
            rendering = false;
        }

        private void RenderFrame(object sender, EventArgs e)
        {
            if (ellipses.Count == 0)
            {
                // Animation just started. Create the ellipses.
                int halfCanvasWidth = (int)canvas.ActualWidth / 2;

                Random rand = new Random();
                int ellipseCount = rand.Next(minEllipses, maxEllipses + 1);
                for (int i = 0; i < ellipseCount; i++)
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Fill = Brushes.LimeGreen;
                    ellipse.Width = ellipseRadius;
                    ellipse.Height = ellipseRadius;
                    Canvas.SetLeft(ellipse, halfCanvasWidth + rand.Next(-halfCanvasWidth, halfCanvasWidth));
                    Canvas.SetTop(ellipse, 0);
                    canvas.Children.Add(ellipse);

                    EllipseInfo info = new EllipseInfo(ellipse, speedRatio * rand.Next(minStartSpeed, maxStartSpeed));
                    ellipses.Add(info);
                }
            }
            else
            {
                for (int i = ellipses.Count - 1; i >= 0; i--)
                {
                    EllipseInfo info = ellipses[i];
                    double top = Canvas.GetTop(info.Ellipse);
                    Canvas.SetTop(info.Ellipse, top + 1 * info.VelocityY);

                    if (top >= (canvas.ActualHeight - 2 * ellipseRadius - 10))
                    {
                        // This circle has reached the bottom.
                        // Stop animating it.
                        ellipses.Remove(info);
                    }
                    else
                    {
                        // Increase the velocity.
                        info.VelocityY += accelerationY;
                    }

                    if (ellipses.Count == 0)
                    {
                        // End this animation.
                        // There's no reason to keep calling this method if it has no work to do.
                        StopRendering();
                    }
                }
            }
        }

        private class EllipseInfo
        {
            public Ellipse Ellipse { get; set; }

            public double VelocityY { get; set; }

            public EllipseInfo(Ellipse ellipse, double velocityY)
            {
                VelocityY = velocityY;
                Ellipse = ellipse;
            }
        }
    }
}
