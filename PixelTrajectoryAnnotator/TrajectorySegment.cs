using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace PixelTrajectoryAnnotator
{
    public class TrajectorySegment
    {
        private readonly LinkedList<Point> points;

        public IEnumerable<Point> Points => points;

        public string AnnotationInfo { get; set; }

        private int startFrameNumber;


        public TrajectorySegment(string pointData)
        {
            this.AnnotationInfo = "无标注";

            if (pointData.EndsWith(";"))
                pointData = pointData.Substring(0, pointData.Length - 1);

            points = new LinkedList<Point>();
            string[] pointDataArray = pointData.Split(';');
            foreach (string pointString in pointDataArray)
            {
                string[] coordinateStrings = pointString.Substring(1, pointString.Length - 2).Split(',');
                int x = int.Parse(coordinateStrings[0]);
                int y = int.Parse(coordinateStrings[1]);
                points.AddLast(new Point(x, y));
            }

            string startFrameNumberString = pointDataArray[0].Substring(1, pointDataArray[0].Length - 2).Split(',')[2];
            startFrameNumber = int.Parse(startFrameNumberString);
        }

        public override string ToString()
        {
            StringBuilder trajectorySegmentInfo = new StringBuilder();

            if (string.IsNullOrEmpty(this.AnnotationInfo))
                trajectorySegmentInfo.Append("无标注");
            else
                trajectorySegmentInfo.Append(this.AnnotationInfo);

            trajectorySegmentInfo.Append(": ");

            int frameNumber = startFrameNumber;

            foreach (Point point in points)
            {
                trajectorySegmentInfo.Append($"({point.X}, {point.Y}, {frameNumber}); ");
                frameNumber++;
            }

            // Remove the last "; " at the end.
            string result = trajectorySegmentInfo.ToString();
            return result.Substring(0, result.Length - 2);
        }

        public XmlElement ToXmlElement(XmlDocument doc)
        {
            // Create the node that represents this trajectory segment.
            XmlElement trajectorySegmentElement = doc.CreateElement("TrajectorySegment");

            // Add the annotation information as an attribute of this XML element.
            XmlAttribute annotationAttribute = doc.CreateAttribute("AnnotationInfo");
            annotationAttribute.Value = this.AnnotationInfo;
            trajectorySegmentElement.Attributes.Append(annotationAttribute);

            int frameNumber = startFrameNumber;
            foreach (Point point in points)
            {
                // Create the inner element that represent a point.
                XmlElement pointElement = doc.CreateElement("Point");

                // Set the attributes of the element, namely X, Y, FrameNumber.
                XmlAttribute xAttribute = doc.CreateAttribute("X");
                xAttribute.Value = point.X.ToString();
                pointElement.Attributes.Append(xAttribute);

                XmlAttribute yAttribute = doc.CreateAttribute("Y");
                yAttribute.Value = point.Y.ToString();
                pointElement.Attributes.Append(yAttribute);

                XmlAttribute frameNumberAttribute = doc.CreateAttribute("FrameNumber");
                frameNumberAttribute.Value = frameNumber.ToString();
                pointElement.Attributes.Append(frameNumberAttribute);

                trajectorySegmentElement.AppendChild(pointElement);

                frameNumber++;
            }

            return trajectorySegmentElement;
        }

        public JObject ToJsonObject()
        {
            JArray pointArray = new JArray();
            int frameNumber = startFrameNumber;
            foreach (Point point in points)
            {
                object pointJson = new
                {
                    X = point.X,
                    Y = point.Y,
                    FrameNumber = frameNumber
                };
                pointArray.Add(JObject.FromObject(pointJson));

                frameNumber++;
            }

            object annotation = new
            {
                AnnotationInfo = this.AnnotationInfo,
                Points = pointArray
            };

            return JObject.FromObject(annotation);
        }
    }
}
