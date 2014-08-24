using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class LineRenderer : IRenderer
    {
        public void DrawCanvas(double maxYdollar, double minYdollar, double pixelsPerDollar, double pixelsPerDay, Canvas canvas, IList<IPricePeriod> pricePeriods)
        {
            for (var i = 1; i < pricePeriods.Count; i++)
            {
                var previousCloseY = canvas.ActualHeight - ((double) pricePeriods[i - 1].Close - minYdollar)*pixelsPerDollar;
                var currentCloseY = canvas.ActualHeight - ((double)pricePeriods[i].Close - minYdollar) * pixelsPerDollar;
                var leftX = (i*pixelsPerDay) - (.5*pixelsPerDay);
                var rightX = leftX + pixelsPerDay;

                var line = CreateLine(leftX, previousCloseY, rightX, currentCloseY, Brushes.Black);
                canvas.Children.Add(line);
            }
     
        }
        protected static Line CreateLine(double x1, double y1, double x2, double y2, Brush stroke)
        {
            return new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = stroke };
        }
    }
}