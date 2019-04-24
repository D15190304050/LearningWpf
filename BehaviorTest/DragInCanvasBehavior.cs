using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace BehaviorTest
{
    class DragInCanvasBehavior : Behavior<UIElement>
    {
        private Canvas canvas;

        /// <summary>
        /// Keep track of when the elements is being dragged.
        /// </summary>
        private bool isDragging;

        /// <summary>
        /// When the element is clicked, record the exact position where the click made.
        /// </summary>
        private Point mouseOffset;

        protected override void OnAttached()
        {
            base.OnAttached();

            // Hook up event handlers.
            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove += AssociateObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonUp += AssociateObject_MouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            // Detach event handlers.
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove -= AssociateObject_MouseMove;
            this.AssociatedObject.PreviewMouseLeftButtonUp -= AssociateObject_MouseLeftButtonUp;
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Find the canvas.
            if (canvas == null)
                canvas = VisualTreeHelper.GetParent(this.AssociatedObject) as Canvas;

            // Drag mode begins.
            isDragging = true;

            // Get the position of the click relative to the element
            // (so the top-left corner of the element if (0,0)).
            mouseOffset = e.GetPosition(AssociatedObject);

            // Capture the mouse.
            // This way you'll keep receiveing the MouseMove event even if the user jerks the mouse off the element.
            AssociatedObject.CaptureMouse();
        }

        private void AssociateObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // Get the position of the element relative to the canvas.
                Point point = e.GetPosition(canvas);

                // Move the element.
                AssociatedObject.SetValue(Canvas.TopProperty, point.Y - mouseOffset.Y);
                AssociatedObject.SetValue(Canvas.LeftProperty, point.X - mouseOffset.X);
            }
        }

        private void AssociateObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                AssociatedObject.ReleaseMouseCapture();
                isDragging = false;
            }
        }
    }
}