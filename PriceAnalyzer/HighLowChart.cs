using System.Linq;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class HighLowChart : ChartBase
    {
        protected override void DrawCanvas(double maxYdollar, double minYdollar, double pixelsPerDollar, double pixelsPerDay)
        {
            _canvas.Children.Clear();

            var minX = _pricePeriods.First().Head;
            decimal priorPeriodClose = 0;
            
            foreach (var pricePeriod in _pricePeriods)
            {
                var closeColorBrush = CloseColorBrush(priorPeriodClose, pricePeriod.Close);
                var x = ((pricePeriod.Head - minX).Days*pixelsPerDay) + (.5*pixelsPerDay);
                var highLowBar = CreateLine(x, _canvas.ActualHeight - ((double) pricePeriod.Low - minYdollar)*pixelsPerDollar, x, (maxYdollar - (double) pricePeriod.High)*pixelsPerDollar, closeColorBrush);
                var openY = _canvas.ActualHeight - ((double)pricePeriod.Open - minYdollar)*pixelsPerDollar;
                var openBar = CreateLine(x - (pixelsPerDay*.25), openY, x, openY, closeColorBrush);
                var closeY = _canvas.ActualHeight - ((double)pricePeriod.Close - minYdollar) * pixelsPerDollar;
                var closeBar = CreateLine(x + (pixelsPerDay*.25), closeY, x, closeY, closeColorBrush);
                priorPeriodClose = pricePeriod.Close;

                _canvas.Children.Add(highLowBar);
                _canvas.Children.Add(openBar);
                _canvas.Children.Add(closeBar);
            }
        }
    }
}