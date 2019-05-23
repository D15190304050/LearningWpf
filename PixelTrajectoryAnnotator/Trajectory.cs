using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.IO;

namespace PixelTrajectoryAnnotator
{
    public class Trajectory
    {
        private const int ColorCount = 7;
        private static readonly int PrefixLength = "Trajectory".Length;

        private LinkedList<Point> points;

        public IEnumerable<Point> Points
        {
            get { return points; }
        }

        private LinkedList<TrajectorySegment> trajectorySegments;

        public IEnumerable<TrajectorySegment> TrajectorySegments
        {
            get { return trajectorySegments; }
        }

        public int TrajectoryId { get; private set; }

        private static Brush[] brushes;

        static Trajectory()
        {
            brushes = new Brush[ColorCount];
            brushes[0] = Brushes.Red;
            brushes[1] = Brushes.Blue;
            brushes[2] = Brushes.Orange;
            brushes[3] = Brushes.Green;
            brushes[4] = Brushes.IndianRed;
            brushes[5] = Brushes.GreenYellow;
            brushes[6] = Brushes.Purple;
        }

        public Trajectory(string trajectoryDataFileName)
        {
            string idString = trajectoryDataFileName.Substring(PrefixLength);
            this.TrajectoryId = int.Parse(idString);
            string[] trajectorySegmentsString = File.ReadAllLines(trajectoryDataFileName);

            trajectorySegments = new LinkedList<TrajectorySegment>();
            foreach (string trajectorySegmentString in trajectorySegmentsString)
                trajectorySegments.AddLast(new TrajectorySegment(trajectorySegmentString));

            points = new LinkedList<Point>();
            foreach (TrajectorySegment trajectorySegment in trajectorySegments)
            {
                foreach (Point point in trajectorySegment.Points)
                    points.AddLast(point);
            }
        }

        public void Draw(Canvas canvas, double horizontalLength, double verticalLength)
        {
            int colorIndex = 0;
            
            foreach (TrajectorySegment trajectorySegment in trajectorySegments)
            {
                System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
                path.Stroke = brushes[colorIndex % ColorCount];
                path.StrokeThickness = 2;
                colorIndex++;

                PathGeometry pathGeometry = new PathGeometry();
                PathFigure pathFigure = new PathFigure();

                Point firstPoint = trajectorySegment.Points.First();
                double firstPointX = firstPoint.X / horizontalLength;
                double firstPointY = firstPoint.Y / verticalLength;
                pathFigure.StartPoint = new Point(firstPointX, firstPointY);
                PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();

                foreach (Point point in trajectorySegment.Points)
                {
                    LineSegment newSegment = new LineSegment();
                    newSegment.Point = new Point(point.X / horizontalLength, point.Y / verticalLength);
                    pathSegmentCollection.Add(newSegment);
                }

                pathFigure.Segments = pathSegmentCollection;
                pathGeometry.Figures.Add(pathFigure);

                path.Data = pathGeometry;

                

                Canvas.SetLeft(path, 0);
                Canvas.SetTop(path, 0);
                canvas.Children.Add(path);
            }

            
            TextBlock textTrajectoryId = new TextBlock();
            textTrajectoryId.Text = $"Trajectory ID: {this.TrajectoryId}";
            textTrajectoryId.Foreground = Brushes.Red;
            textTrajectoryId.FontSize = 16;
            Canvas.SetLeft(textTrajectoryId, points.First.Value.X - 5);
            Canvas.SetTop(textTrajectoryId, points.First.Value.Y - 5);
            canvas.Children.Add(textTrajectoryId);
        }
    }
}
