using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class HorizontalAxis : Canvas
    {
        public IList<IPricePeriod> PricePeriods { get; set; }

        public void DrawHorizontalAxis(double pixelsPerPeriod)
        {
            Children.Clear();

            const int minimumPixelsPerTick = 80;
            var pixelsPerTick = Math.Max(ActualWidth / PricePeriods.Count, minimumPixelsPerTick);
            var valuesBetweenTicks = pixelsPerTick / pixelsPerPeriod;
            for (var i = 0; i < PricePeriods.Count; i += (int)Math.Round(valuesBetweenTicks, 0))
            {
                var location = (i * pixelsPerPeriod) + (.5 * pixelsPerPeriod);
                DrawLine(location, 0, 5, 0);
                DrawValue(PricePeriods[i].Head.ToShortDateString(), 7, location - 25);
            }
            var separator = CreateLine(0, 0, ActualWidth, 0, Brushes.DarkBlue);
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