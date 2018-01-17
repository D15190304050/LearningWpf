using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CustomElements.CustomControls
{
    public class WrapBreakPanel : Panel
    {
        public static DependencyProperty LinearBreakBeforeProperty;

        static WrapBreakPanel()
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata();
            metadata.AffectsArrange = true;
            metadata.AffectsMeasure = true;
            LinearBreakBeforeProperty = DependencyProperty.Register("LinearBreakBefore", typeof(bool), typeof(WrapBreakPanel), metadata);
        }

        public static void SetLineBreakBefore(UIElement element, bool value) => element.SetValue(LinearBreakBeforeProperty, value);

        public static bool GetLineBreakBefore(UIElement element) => (bool)element.GetValue(LinearBreakBeforeProperty);

        protected override Size MeasureOverride(Size constraint)
        {
            Size currentLineSize = new Size();
            Size panelSize = new Size();

            foreach (UIElement element in base.InternalChildren)
            {
                element.Measure(constraint);
                Size desiredSize = element.DesiredSize;

                if (GetLineBreakBefore(element) || (currentLineSize.Width + desiredSize.Width > constraint.Width))
                {
                    // Switch to a new line (either because the element has requested it or space has run out).
                    panelSize.Width = Math.Max(currentLineSize.Width, panelSize.Width);
                    panelSize.Height += currentLineSize.Height;
                    currentLineSize = desiredSize;

                    // If the element is too wide to fit using the maximum width of the line, just give it a separate line.
                    if (desiredSize.Width > constraint.Width)
                    {
                        panelSize.Width = Math.Max(desiredSize.Width, panelSize.Width);
                        panelSize.Height += desiredSize.Height;
                        currentLineSize = new Size();
                    }
                }
                else
                {
                    // Keep adding to the current line.
                    currentLineSize.Width += desiredSize.Width;

                    // Make sure the line is as tall as the tallest element.
                    currentLineSize.Height = Math.Max(desiredSize.Height, currentLineSize.Height);
                }
            }

            // Return the size required to fit all elements.
            // Ordinarily, this is the width of the constraint, and the height is based on the size of the elements.
            // However, if an element is wider than the width given to the panel, the desired width will be the width of that line.
            panelSize.Width = Math.Max(currentLineSize.Width, panelSize.Width);
            panelSize.Height += currentLineSize.Height;
            return panelSize;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            int firstLine = 0;
            Size currentLineSize = new Size();
            double accumulateHeight = 0;

            UIElementCollection elements = base.InternalChildren;
            for (int i = 0; i < elements.Count; i++)
            {
                Size desiredSize = elements[i].DesiredSize;

                if (GetLineBreakBefore(elements[i]) || (currentLineSize.Width + currentLineSize.Width > arrangeBounds.Width))
                {
                    // Need to switch to another line.
                    ArrangeLine(accumulateHeight, currentLineSize.Height, firstLine, i);

                    accumulateHeight += currentLineSize.Height;
                    currentLineSize = desiredSize;

                    if (desiredSize.Width > arrangeBounds.Width)
                    {
                        // The element is wider than the constraint - give it a separate line.
                        ArrangeLine(accumulateHeight, desiredSize.Height, i, ++i);
                        accumulateHeight += desiredSize.Height;
                        currentLineSize = new Size();
                    }
                    firstLine = i;
                }
                else
                {
                    // Continue to accumulate a line.
                    currentLineSize.Width += desiredSize.Width;
                    currentLineSize.Height = Math.Max(desiredSize.Height, currentLineSize.Height);
                }
            }

            if (firstLine < elements.Count)
                ArrangeLine(accumulateHeight, currentLineSize.Height, firstLine, elements.Count);

            return arrangeBounds;
        }

        private void ArrangeLine(double y, double lineHeight, int start, int end)
        {
            double x = 0;
            UIElementCollection children = base.InternalChildren;
            for (int i = start; i < end; i++)
            {
                UIElement child = children[i];
                child.Arrange(new Rect(x, y, child.DesiredSize.Width, lineHeight));
                x += child.DesiredSize.Width;
            }
        }
    }
}
