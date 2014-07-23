using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class OpenHighLowCloseRenderer : IRenderer
    {
        public void DrawCanvas(double maxYdollar, double minYdollar, double pixelsPerDollar, double pixelsPerDay, Canvas canvas, IList<IPricePeriod> pricePeriods)
        {
            decimal priorPeriodClose = 0;

            for (var i = 0; i < pricePeriods.Count; i++)
            {
                var pricePeriod = pricePeriods[i];
                var closeColorBrush = CloseColorBrush(priorPeriodClose, pricePeriod.Close);
                var x = (i*pixelsPerDay) + (.5*pixelsPerDay);
                var highLowBar = CreateLine(x,
                    canvas.ActualHeight - ((double) pricePeriod.Low - minYdollar)*pixelsPerDollar, x,
                    (maxYdollar - (double) pricePeriod.High)*pixelsPerDollar, closeColorBrush);
                var openY = canvas.ActualHeight - ((double) pricePeriod.Open - minYdollar)*pixelsPerDollar;
                var openBar = CreateLine(x - (pixelsPerDay*.25), openY, x, openY, closeColorBrush);
                var closeY = canvas.ActualHeight - ((double) pricePeriod.Close - minYdollar)*pixelsPerDollar;
                var closeBar = CreateLine(x + (pixelsPerDay*.25), closeY, x, closeY, closeColorBrush);
                priorPeriodClose = pricePeriod.Close;

                canvas.Children.Add(highLowBar);
                canvas.Children.Add(openBar);
                canvas.Children.Add(closeBar);
            }
        }

        private static Brush CloseColorBrush(decimal priorPeriodClose, decimal currentPeriodClose)
        {
            if (priorPeriodClose < currentPeriodClose)
            {
                return Brushes.Black;
            }
            else
            {
                return Brushes.Firebrick;
            }
        }

        private static Line CreateLine(double x1, double y1, double x2, double y2, Brush stroke)
        {
            return new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = stroke };
        }
    }
}