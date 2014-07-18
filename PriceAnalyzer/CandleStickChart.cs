using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class CandleStickChart : ChartBase
    {
        protected override void DrawCanvas(double maxYdollar, double minYdollar, double pixelsPerDollar, double pixelsPerDay)
        {
            _canvas.Children.Clear();

            var minX = PricePeriods.First().Head.CurrentPeriodOpen(Resolution.Days);
            decimal priorPeriodClose = 0;

            foreach (var pricePeriod in PricePeriods)
            {
                var brush = CloseColorBrush(priorPeriodClose, pricePeriod.Close);
                var leftX = ((pricePeriod.Head - minX).Days * pixelsPerDay) + (.25 * pixelsPerDay);
                var centerX = leftX + (.25 * pixelsPerDay);
                var rightX = centerX + (.25 * pixelsPerDay);
                var lowY = _canvas.ActualHeight - ((double)pricePeriod.Low - minYdollar) * pixelsPerDollar;
                var highY = (maxYdollar - (double) pricePeriod.High)*pixelsPerDollar;
                var openY = _canvas.ActualHeight - ((double)pricePeriod.Open - minYdollar) * pixelsPerDollar;
                var closeY = _canvas.ActualHeight - ((double)pricePeriod.Close - minYdollar) * pixelsPerDollar;
                priorPeriodClose = pricePeriod.Close;


                var body = new Rectangle
                {
                    Width = rightX - leftX,
                    Height = Math.Abs(openY - closeY),
                    Stroke = brush
                };
                Canvas.SetLeft(body, leftX);
                Canvas.SetTop(body, Math.Min(openY, closeY));

                var lowLine = CreateLine(centerX, Math.Max(openY, closeY), centerX, lowY, brush);
                var highLine = CreateLine(centerX, Math.Min(openY, closeY), centerX, highY, brush);
                _canvas.Children.Add(body);
                _canvas.Children.Add(lowLine);
                _canvas.Children.Add(highLine);
            }
        }
    }
}