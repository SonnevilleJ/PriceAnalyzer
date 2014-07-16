using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class VerticalAxis : Canvas
    {
        public void DrawVerticalAxis(double highestHigh, double lowestLow, double pixelsPerValue)
        {
            Children.Clear();

            const int minimumPixelsPerTick = 30;
            const double maximumTicks = 10;
            var pixelsPerTick = Math.Max(ActualHeight / maximumTicks, minimumPixelsPerTick);
            var valuesBetweenTicks = pixelsPerTick / pixelsPerValue;
            for (var i = 0.0; i < Math.Round(highestHigh - lowestLow, 0); i += valuesBetweenTicks)
            {
                var location = i * pixelsPerValue;
                DrawLine(40, location, 0, 5);
                DrawValue((highestHigh - i).ToString("C"), location - (6), 5);
            }
            var separator = CreateLine(ActualWidth, 0, ActualWidth, ActualHeight, Brushes.DarkBlue);
            Children.Add(separator);
        }

        private void DrawValue(string valueText, double top, double left)
        {
            var textBlock = new TextBlock { Text = valueText, Foreground = Brushes.Black };
            SetTop(textBlock, top);
            SetLeft(textBlock, left);
            Children.Add(textBlock);
        }

        private void DrawLine(double xLocation, double yLocation, int tickHeight, int tickWidth)
        {
            var line = CreateLine(xLocation, yLocation, xLocation + tickWidth, yLocation + tickHeight, Brushes.Black);
            Children.Add(line);
        }

        private static Line CreateLine(double x1, double y1, double x2, double y2, Brush stroke)
        {
            return new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = stroke };
        }
    }
}