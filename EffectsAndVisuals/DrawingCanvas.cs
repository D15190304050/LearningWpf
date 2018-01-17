using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace EffectsAndVisuals
{
    public class DrawingCanvas : Canvas
    {
        /// <summary>
        /// Collection of Visual objects.
        /// </summary>
        private List<Visual> visuals;

        /// <summary>
        /// Collection of visuals which meet the condition of hit test.
        /// </summary>
        private List<DrawingVisual> hits;

        /// <summary>
        /// Initialize an empty DrawingCanvas.
        /// </summary>
        public DrawingCanvas() : base()
        {
            visuals = new List<Visual>();
            hits = new List<DrawingVisual>();
        }

        /// <summary>
        /// Gets the number of Visual objects.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get
            {
                return visuals.Count;
            }
        }

        /// <summary>
        /// Access Visual object contained in this DrawingCanvas by index.
        /// </summary>
        /// <param name="index">Index of the Visual object.</param>
        /// <returns>The Visual object identified by index.</returns>
        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        /// <summary>
        /// Add a Visual object to this DrawingCanvas.
        /// </summary>
        /// <param name="visual">The Visual object to add.</param>
        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        /// <summary>
        /// Remove a Visual object from this DrawingCanvas.
        /// </summary>
        /// <param name="visual">The Visual object to remove.</param>
        public void DeleteVisual(Visual visual)
        {
            visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        /// <summary>
        /// Returns the top-most Visual object of a hit test by specifying a Point.
        /// </summary>
        /// <param name="point">The point value to hit test against.</param>
        /// <returns>The top-most Visual object of a hit test by specifying a Point.</returns>
        public DrawingVisual GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as DrawingVisual;
        }

        /// <summary>
        /// Initiates a hit test on the specified Visual object, with called-defined HitTestFilterCallback and HitTestResultCallback methods.
        /// </summary>
        /// <param name="region">The region to hit test against.</param>
        /// <returns></returns>
        public List<DrawingVisual> GetVisuals(Geometry region)
        {
            // Remove matches from the previous search.
            hits.Clear();

            // Prepare the parameters for the hit test operation.
            // The geometry and callback.
            GeometryHitTestParameters parameters = new GeometryHitTestParameters(region);
            HitTestResultCallback callback = new HitTestResultCallback(this.HitTestCallback);

            // Search for hits.
            VisualTreeHelper.HitTest(this, null, callback, parameters);
            return hits;
        }

        /// <summary>
        /// Represents a callback that is used to customize hit testing.
        /// WPF invokes HitRestResult to report hit test intersections to the user.
        /// </summary>
        /// <param name="result">The HitTestResult value that represents a visual object that is returned from a hit test.</param>
        /// <returns>A HitTestResultBehavior that represents the action resulting from the hit test.</returns>
        private HitTestResultBehavior HitTestCallback(HitTestResult result)
        {
            GeometryHitTestResult geometryResult = (GeometryHitTestResult)result;
            DrawingVisual visual = result.VisualHit as DrawingVisual;

            // Only include matches that are DrawingVisual objects and that are completely inside the geometry.
            if ((visual != null) && (geometryResult.IntersectionDetail == IntersectionDetail.FullyInside))
                hits.Add(visual);
            return HitTestResultBehavior.Continue;
        }
    }
}
