using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class CandleStickRenderer : IRenderer
    {
        public void DrawCanvas(double maxYdollar, double minYdollar, double pixelsPerDollar, double pixelsPerDay, Canvas canvas, IList<IPricePeriod> pricePeriods)
        {
            decimal priorPeriodClose = 0;

            for (var i = 0; i < pricePeriods.Count; i++)
            {
                var pricePeriod = pricePeriods[i];
                var brush = CloseColorBrush(priorPeriodClose, pricePeriod.Close);
                var leftX = (i*pixelsPerDay) + (.25*pixelsPerDay);
                var centerX = leftX + (.25*pixelsPerDay);
                var rightX = centerX + (.25*pixelsPerDay);
                var lowY = canvas.ActualHeight - ((double) pricePeriod.Low - minYdollar)*pixelsPerDollar;
                var highY = (maxYdollar - (double) pricePeriod.High)*pixelsPerDollar;
                var openY = canvas.ActualHeight - ((double) pricePeriod.Open - minYdollar)*pixelsPerDollar;
                var closeY = canvas.ActualHeight - ((double) pricePeriod.Close - minYdollar)*pixelsPerDollar;
                priorPeriodClose = pricePeriod.Close;


                var body = new Rectangle
                {
                    Width = rightX - leftX,
                    Height = Math.Abs(openY - closeY),
                    Stroke = brush,
                    Fill = pricePeriod.Close <= pricePeriod.Open ? brush : Brushes.Transparent,
                };
                Canvas.SetLeft(body, leftX);
                Canvas.SetTop(body, Math.Min(openY, closeY));

                var lowLine = CreateLine(centerX, Math.Max(openY, closeY), centerX, lowY, brush);
                var highLine = CreateLine(centerX, Math.Min(openY, closeY), centerX, highY, brush);
                canvas.Children.Add(body);
                canvas.Children.Add(lowLine);
                canvas.Children.Add(highLine);
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

        protected static Line CreateLine(double x1, double y1, double x2, double y2, Brush stroke)
        {
            return new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = stroke };
        }
    }
}