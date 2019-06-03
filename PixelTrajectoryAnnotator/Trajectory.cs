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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace PixelTrajectoryAnnotator
{
    public class Trajectory
    {
        private const int ColorCount = 7;
        private const string FileNamePrefix = "Trajectory";

        private readonly LinkedList<Point> points;

        public IEnumerable<Point> Points => points;

        private readonly LinkedList<TrajectorySegment> trajectory;

        public IEnumerable<TrajectorySegment> TrajectorySegments => trajectory;

        public int TrajectoryId { get; private set; }

        private static readonly Brush[] Colors;

        static Trajectory()
        {
            Colors = new Brush[ColorCount];
            Colors[0] = Brushes.Red;
            Colors[1] = Brushes.Blue;
            Colors[2] = Brushes.Orange;
            Colors[3] = Brushes.Green;
            Colors[4] = Brushes.IndianRed;
            Colors[5] = Brushes.GreenYellow;
            Colors[6] = Brushes.Purple;
        }

        public Trajectory(string trajectoryDataFileName)
        {
            int indexOfFileName = trajectoryDataFileName.LastIndexOf(FileNamePrefix, StringComparison.Ordinal);
            int indexOfLastDot = trajectoryDataFileName.LastIndexOf('.');
            int idStartIndex = indexOfFileName + FileNamePrefix.Length;

            string idString = trajectoryDataFileName.Substring(idStartIndex, indexOfLastDot - idStartIndex);
            this.TrajectoryId = int.Parse(idString);
            string[] trajectorySegmentsString = File.ReadAllLines(trajectoryDataFileName);

            trajectory = new LinkedList<TrajectorySegment>();
            foreach (string trajectorySegmentString in trajectorySegmentsString)
                trajectory.AddLast(new TrajectorySegment(trajectorySegmentString));

            points = new LinkedList<Point>();
            foreach (TrajectorySegment trajectorySegment in trajectory)
            {
                foreach (Point point in trajectorySegment.Points)
                    points.AddLast(point);
            }
        }

        public void Draw(Canvas canvas, double canvasWidth, double canvasHeight, double backgroundPixelWidth, double backgroundPixelHeight)
        {
            if (points.Count == 0)
                return;

            int colorIndex = 0;
            
            foreach (TrajectorySegment trajectorySegment in trajectory)
            {
                System.Windows.Shapes.Path path = new System.Windows.Shapes.Path
                {
                    Stroke = Colors[colorIndex % ColorCount],
                    StrokeThickness = 4
                };
                colorIndex++;

                PathGeometry pathGeometry = new PathGeometry();
                PathFigure pathFigure = new PathFigure();

                Point firstPoint = trajectorySegment.Points.First();
                double firstPointX = firstPoint.X / backgroundPixelWidth * canvasWidth;
                double firstPointY = firstPoint.Y / backgroundPixelHeight * canvasHeight;

                Rectangle startPoint = new Rectangle { Width = 20, Height = 20, Fill = Brushes.BlueViolet };
                Canvas.SetLeft(startPoint, firstPointX - 10);
                Canvas.SetTop(startPoint, firstPointY - 10);
                canvas.Children.Add(startPoint);

                Point lastPoint = trajectorySegment.Points.Last();
                Ellipse endPoint = new Ellipse { Width = 20, Height = 20, Fill = Brushes.Orange };
                Canvas.SetLeft(endPoint, lastPoint.X / backgroundPixelWidth * canvasWidth - 10);
                Canvas.SetTop(endPoint, lastPoint.Y / backgroundPixelHeight * canvasHeight - 10);
                canvas.Children.Add(endPoint);

                pathFigure.StartPoint = new Point(firstPointX, firstPointY);
                PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();

                foreach (Point point in trajectorySegment.Points)
                {
                    LineSegment newSegment = new LineSegment
                    {
                        Point = new Point(point.X / backgroundPixelWidth * canvasWidth, point.Y / backgroundPixelHeight * canvasHeight)
                    };
                    pathSegmentCollection.Add(newSegment);
                }

                pathFigure.Segments = pathSegmentCollection;
                pathGeometry.Figures.Add(pathFigure);

                path.Data = pathGeometry;
                path.ToolTip = trajectorySegment.AnnotationInfo;

                path.MouseDown += (sender, e) =>
                {
                    Window window = new AnnotationWindow(trajectorySegment);
                    window.ShowDialog();

                    path.ToolTip = trajectorySegment.AnnotationInfo;
                };

                Canvas.SetLeft(path, 0);
                Canvas.SetTop(path, 0);
                canvas.Children.Add(path);
            }


            TextBlock textTrajectoryId = new TextBlock
            {
                Text = $"Trajectory ID: {this.TrajectoryId}",
                Foreground = Brushes.Red,
                FontSize = 16
            };
            Canvas.SetLeft(textTrajectoryId, points.First.Value.X / backgroundPixelWidth * canvasWidth + 10);
            Canvas.SetTop(textTrajectoryId, points.First.Value.Y / backgroundPixelHeight * canvasHeight + 10);
            canvas.Children.Add(textTrajectoryId);
        }

        public void SaveAsText(string fileName)
        {
            StringBuilder trajectoryInfo = new StringBuilder();
            foreach (TrajectorySegment trajectorySegment in trajectory)
            {
                trajectoryInfo.Append(trajectorySegment.ToString());
                trajectoryInfo.Append(Environment.NewLine);
            }

            File.WriteAllText(fileName, trajectoryInfo.ToString());
        }

        public void SaveAsXml(string fileName)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement("TrajectoryAnnotation");
            XmlAttribute trajectoryIdAttribute = doc.CreateAttribute("TrajectoryId");
            trajectoryIdAttribute.Value = this.TrajectoryId.ToString();
            root.Attributes.Append(trajectoryIdAttribute);
            doc.AppendChild(root);

            foreach (TrajectorySegment trajectorySegment in trajectory)
                root.AppendChild(trajectorySegment.ToXmlElement(doc));

            doc.Save(fileName);
        }

        public void SaveAsJson(string fileName)
        {
            JArray trajectorySegments = new JArray();
            foreach (TrajectorySegment trajectorySegment in trajectory)
                trajectorySegments.Add(trajectorySegment.ToJsonObject());

            object trajectoryJson = new
            {
                Trajectory = trajectorySegments
            };

            File.WriteAllText(fileName, JObject.FromObject(trajectoryJson).ToString());
        }
    }
}
