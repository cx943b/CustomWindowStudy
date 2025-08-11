using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomWindowStudy
{
    internal class TextRegionTestControl : FrameworkElement
    {
        Point _mousePos = new Point(50, 0);
        public TextRegionTestControl()
        {
            // Initialize the control here if needed
        }

        protected override void OnRender(DrawingContext dc)
        {
            double divX = _mousePos.X;
            Brush brushLeft = Brushes.Gray;
            Brush brushRight = Brushes.Blue;

            Rect rectLeft = new Rect(0, 0, divX, ActualHeight);
            Rect rectRight = new Rect(divX, 0, ActualWidth - divX, ActualHeight);

            dc.DrawRectangle(brushLeft, null, rectLeft);
            dc.DrawRectangle(brushRight, null, rectRight);

            var text = "Hello, Custom Window!";
            var typeface = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
            var pxPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;

            FormattedText ftLeft = new FormattedText(
                text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 24, brushRight, pxPerDip);
            Point txtPos = new Point((ActualWidth - ftLeft.Width) / 2, (ActualHeight - ftLeft.Height) / 2);

            dc.PushClip(new RectangleGeometry(rectLeft));
            dc.DrawText(ftLeft, txtPos);
            dc.Pop();

            FormattedText ftRight = new FormattedText(
                text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 24, brushLeft, pxPerDip);

            dc.PushClip(new RectangleGeometry(rectRight));
            dc.DrawText(ftRight, txtPos);
            dc.Pop();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            _mousePos = e.GetPosition(this);
            InvalidateVisual(); // Redraw the control to reflect the mouse position
        }
    }
}
