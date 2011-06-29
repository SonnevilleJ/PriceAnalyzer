using System;
using System.Windows;
using System.Windows.Media;

namespace Sonneville.PriceChartTools
{
    //public class CandlestickChart : Chart
    //{
    //    /// <summary>
    //    /// Gets the polyline points to chart for a period.
    //    /// </summary>
    //    /// <param name="left">The left edge of the period.</param>
    //    /// <param name="center">The center of the period.</param>
    //    /// <param name="right">The right edge of the period.</param>
    //    /// <param name="open">The opening price of the period.</param>
    //    /// <param name="high">The high price of the period.</param>
    //    /// <param name="low">The low price of the period.</param>
    //    /// <param name="close">The closing price of the period.</param>
    //    /// <returns>A <see cref="PointCollection"/> containing the points to be charted for the period.</returns>
    //    protected override PointCollection GetPolylinePoints(double left, double center, double right, double? open, double? high, double? low, double close)
    //    {
    //        if (open == null) { throw new ArgumentNullException("open"); }
    //        if (high == null) { throw new ArgumentNullException("high"); }
    //        if (low == null) { throw new ArgumentNullException("low"); }

    //        return new PointCollection
    //                   {
    //                       new Point(center, close),
    //                       new Point(center, high.Value),
    //                       new Point(center, close),
    //                       new Point(right, close),
    //                       new Point(right, open.Value),
    //                       new Point(center, open.Value),
    //                       new Point(center, low.Value),
    //                       new Point(center, open.Value),
    //                       new Point(left, open.Value),
    //                       new Point(left, close),
    //                       new Point(center, close)
    //                   };
    //    }
    //}
}
