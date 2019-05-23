using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PixelTrajectoryAnnotator
{
    public class TrajectorySegment
    {
        private LinkedList<Point> points;

        public IEnumerable<Point> Points
        {
            get { return points; }
        }

        public TrajectorySegment(string pointData)
        {
            if (pointData.EndsWith(";"))
                pointData = pointData.Substring(0, pointData.Length - 1);

            points = new LinkedList<Point>();
            foreach (string pointString in pointData.Split(';'))
            {
                string[] coordinateStrings = pointString.Substring(1, pointString.Length - 2).Split(',');
                int x = int.Parse(coordinateStrings[0]);
                int y = int.Parse(coordinateStrings[1]);
                points.AddLast(new Point(x, y));
            }
        }
    }
}
