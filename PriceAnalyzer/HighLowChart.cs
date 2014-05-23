using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class HighLowChart : Canvas
    {
        private IList<IPricePeriod> _pricePeriods;

        public HighLowChart()
        {
            SizeChanged += delegate { if (_pricePeriods != null) Draw(); };
        }

        public void DrawPricePeriods(IList<IPricePeriod> pricePeriods)
        {
            _pricePeriods = pricePeriods;
            Draw();
        }

        private void Draw()
        {
            Children.Clear();

            var highestHigh = ((double) _pricePeriods.Max(pricePeriod => pricePeriod.High));
            var lowestLow = ((double) _pricePeriods.Min(pricePeriod => pricePeriod.Low));

            var maxYdollar = highestHigh;
            var minYdollar = lowestLow;
            var dollarRange = (maxYdollar - minYdollar);
            var minX = _pricePeriods.First().Head;
            var maxX = _pricePeriods.Last().Tail;

            var pixelsPerDollar = (ActualHeight/dollarRange);

            foreach (var pricePeriod in _pricePeriods)
            {
                var line = new Line();
                line.X1 = ((pricePeriod.Head - minX).Days*5) + 5;
                line.Y1 = ActualHeight - (((double) pricePeriod.Low - minYdollar)*pixelsPerDollar);
                line.X2 = line.X1;
                line.Y2 = (maxYdollar - (double) pricePeriod.High)*pixelsPerDollar;
                line.Stroke = Brushes.Blue;

                Children.Add(line);
            }
        }
    }
}