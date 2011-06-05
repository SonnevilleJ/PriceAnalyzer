using System.Windows;
using System.Windows.Media;

namespace Sonneville.PriceChartTools
{
    public class CandlestickChart : Chart
    {
        /// <summary>
        /// Gets the points to chart for a period.
        /// </summary>
        /// <param name="center">The center of the period.</param>
        /// <param name="open">The opening price of the period.</param>
        /// <param name="high">The high price of the period.</param>
        /// <param name="low">The low price of the period.</param>
        /// <param name="close">The closing price of the period.</param>
        /// <returns>A <see cref="PointCollection"/> containing the points to be charted for the period.</returns>
        public override PointCollection GetPeriodPoints(double center, double open, double high, double low, double close)
        {
            double left = center - HalfCandleWidth;
            double right = center + HalfCandleWidth;

            var points = new PointCollection
                             {
                                 new Point(center, low),
                                 new Point(center, open),
                                 new Point(left, open),
                                 new Point(left, close),
                                 new Point(center, close),
                                 new Point(center, high),
                                 new Point(center, close),
                                 new Point(right, close),
                                 new Point(right, open),
                                 new Point(center, open),
                                 new Point(center, low)
                             };
            return points;
        }
    }
}