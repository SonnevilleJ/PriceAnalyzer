using System.Windows;
using System.Windows.Media;

namespace Sonneville.PriceTools.Charting
{
    public class CandlestickChart : ChartCanvas
    {
        /// <summary>
        /// Gets the polyline points to chart for a period.
        /// </summary>
        /// <param name="left">The left edge of the period.</param>
        /// <param name="center">The center of the period.</param>
        /// <param name="right">The right edge of the period.</param>
        /// <param name="open">The opening price of the period.</param>
        /// <param name="high">The high price of the period.</param>
        /// <param name="low">The low price of the period.</param>
        /// <param name="close">The closing price of the period.</param>
        /// <returns>A <see cref="PointCollection"/> containing the points to be charted for the period.</returns>
        protected override PointCollection GetPolylinePoints(double left, double center, double right, double open, double high, double low, double close)
        {
            return new PointCollection
                       {
                           new Point(center, close),
                           new Point(center, high),
                           new Point(center, close),
                           new Point(right, close),
                           new Point(right, open),
                           new Point(center, open),
                           new Point(center, low),
                           new Point(center, open),
                           new Point(left, open),
                           new Point(left, close),
                           new Point(center, close)
                       };
        }
    }
}
