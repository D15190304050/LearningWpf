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

namespace EffectsAndVisuals
{
    /// <summary>
    /// VisualLayer.xaml 的交互逻辑
    /// </summary>
    public partial class VisualLayer : Window
    {
        public VisualLayer()
        {
            InitializeComponent();

            /* Initialize variables to carry out interaction logic. */
            isDragging = isMultiSelecting = false;
            drawingBrush = Brushes.AliceBlue;
            selectedBrush = Brushes.LightGoldenrodYellow;
            drawingPen = new Pen(Brushes.SteelBlue, 3);
            squareSize = new Size(30, 30);
            selectionSqureBrush = Brushes.Transparent;
            selectionRectanglePen = new Pen(Brushes.Black, 2);
        }

        /* Variables for dragging shapes. */

        /// <summary>
        /// True if the mouse is dragging some square, false otherwise.
        /// </summary>
        private bool isDragging;

        /// <summary>
        /// Offset from top-left corner of the square to the point where mouse clicked.
        /// </summary>
        private Vector clickOffset;

        /// <summary>
        /// The selected Visual object, it's a square here.
        /// </summary>
        private DrawingVisual selectedVisual;

        /* Drawing constants. */

        /// <summary>
        /// The brush to render squares.
        /// </summary>
        private Brush drawingBrush;

        /// <summary>
        /// The brush to render the selected square.
        /// </summary>
        private Brush selectedBrush;

        /// <summary>
        /// The pen to draw the border of squares.
        /// </summary>
        private Pen drawingPen;

        /// <summary>
        /// Size of each square.
        /// </summary>
        private Size squareSize;

        /* Variables for drawing the selection square. */

        /// <summary>
        /// True if user is selecing multiple squares, false otherwise.
        /// </summary>
        private bool isMultiSelecting;

        /// <summary>
        /// The top-left point of the square of multiple selection.
        /// </summary>
        private Point selectionRectangleTopLeft;

        /// <summary>
        /// The rectangle to select multiple squares.
        /// </summary>
        private DrawingVisual selectionRectangle;

        /// <summary>
        /// The brush to render the selectionRectangle.
        /// </summary>
        private Brush selectionSqureBrush;

        /// <summary>
        /// The pen to render the selectionRectangle.
        /// </summary>
        private Pen selectionRectanglePen;

        /// <summary>
        /// Response the MouseLeftButtonDown event happened on drawingSurface.
        /// </summary>
        /// <param name="sender">Sender of this event.</param>
        /// <param name="e">MouseButtonEventArgs of this event.</param>
        private void drawingSurface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pointClicked = e.GetPosition(drawingSurface);

            if (cmdAdd.IsChecked == true)
            {
                DrawingVisual visual = new DrawingVisual();
                DrawSquare(visual, pointClicked, false);
                drawingSurface.AddVisual(visual);
            }
            else if (cmdDelete.IsChecked == true)
            {
                DrawingVisual visual = drawingSurface.GetVisual(pointClicked);
                if (visual != null)
                    drawingSurface.DeleteVisual(visual);
            }
            else if (cmdSelectMove.IsChecked == true)
            {
                DrawingVisual visual = drawingSurface.GetVisual(pointClicked);
                if (visual != null)
                {
                    // Calculate the top-left corner of the square.
                    // This is done by looking at the current bounds and removing half the border (pen thickness).
                    // An alternate solution would be to store the top-left point of every visual in a collection in the DrawingCanvas,
                    // and provide this point when hit testing.
                    Point topLeftCorner = new Point(
                        visual.ContentBounds.TopLeft.X + drawingPen.Thickness / 2,
                        visual.ContentBounds.TopLeft.Y + drawingPen.Thickness / 2);
                    DrawSquare(visual, topLeftCorner, true);

                    clickOffset = topLeftCorner - pointClicked;
                    isDragging = true;

                    if ((selectedVisual != null) && (selectedVisual != visual))
                    {
                        // The selection is changed.
                        // Clear the previous selection.
                        ClearSelection();
                    }
                    selectedVisual = visual;
                }
            }
            else if (cmdSelectMultiple.IsChecked == true)
            {
                selectionRectangle = new DrawingVisual();

                drawingSurface.AddVisual(selectionRectangle);

                selectionRectangleTopLeft = pointClicked;
                isMultiSelecting = true;

                // Make sure we get the MouseLeftButtonUp event even if moves off the Canvas.
                // Otherwise, 2 selection squares could be drawn at once.
                drawingSurface.CaptureMouse();
            }
        }

        /// <summary>
        /// Response the MouseMove event happened on drawingSurface.
        /// </summary>
        /// <param name="sender">Sender of this event.</param>
        /// <param name="e">MouseEventArgs of this event.</param>
        private void drawingSurface_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point pointDragged = e.GetPosition(drawingSurface) + clickOffset;
                DrawSquare(selectedVisual, pointDragged, true);
            }
            else if (isMultiSelecting)
            {
                Point pointDragged = e.GetPosition(drawingSurface);
                DrawSelectionRectangle(selectionRectangleTopLeft, pointDragged);
            }
        }

        /// <summary>
        /// Response the MouseLeftButtonUp event happened on drawingSurface.
        /// </summary>
        /// <param name="sender">Sender of this event.</param>
        /// <param name="e">MouseButtonEventArgs of this event.</param>
        private void drawingSurface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;

            if (isMultiSelecting)
            {
                // Display all the squares in  this region.
                RectangleGeometry geometry = new RectangleGeometry(new Rect(selectionRectangleTopLeft, e.GetPosition(drawingSurface)));
                List<DrawingVisual> visualsInRegion = drawingSurface.GetVisuals(geometry);
                MessageBox.Show(string.Format("You selected {0} square(s).", visualsInRegion.Count));

                isMultiSelecting = false;
                drawingSurface.DeleteVisual(selectionRectangle);
                drawingSurface.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// Rendering the square.
        /// </summary>
        /// <param name="visual">The visual object (the square) to draw.</param>
        /// <param name="topLeftCorner">The top-left point of this square.</param>
        /// <param name="isSelected">True if this square is selected to move, false otherwise.</param>
        private void DrawSquare(DrawingVisual visual, Point topLeftCorner, bool isSelected)
        {
            using (DrawingContext dc = visual.RenderOpen())
            {
                Brush brush = drawingBrush;
                if (isSelected)
                    brush = selectedBrush;

                dc.DrawRectangle(brush, drawingPen, new Rect(topLeftCorner, squareSize));
            }
        }

        /// <summary>
        /// Clear previous selection if user select another square.
        /// </summary>
        private void ClearSelection()
        {
            Point topLeftCorner = new Point(
                selectedVisual.ContentBounds.TopLeft.X + drawingPen.Thickness / 2,
                selectedVisual.ContentBounds.TopLeft.Y + drawingPen.Thickness / 2);
            DrawSquare(selectedVisual, topLeftCorner, false);
            selectedVisual = null;
        }

        /// <summary>
        /// Draw the selection rectangle.
        /// </summary>
        /// <param name="startPoint">The start point of this rectangle.</param>
        /// <param name="endPoint">The end point of this rectanle.</param>
        private void DrawSelectionRectangle(Point startPoint, Point endPoint)
        {
            selectionRectanglePen.DashStyle = DashStyles.Dash;

            using (DrawingContext dc = selectionRectangle.RenderOpen())
            {
                dc.DrawRectangle(selectionSqureBrush, selectionRectanglePen, new Rect(startPoint, endPoint));
            }
        }
    }
}
