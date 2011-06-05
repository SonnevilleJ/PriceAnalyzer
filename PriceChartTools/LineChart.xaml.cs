using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Sonneville.PriceChartTools
{
    public class LineChart : Chart
    {
        public LineChart()
            : base(true)
        {
        }

        /// <summary>
        /// Gets the polyline points to chart for a period.
        /// </summary>
        /// <param name="center">The center of the period.</param>
        /// <param name="open">The opening price of the period.</param>
        /// <param name="high">The high price of the period.</param>
        /// <param name="low">The low price of the period.</param>
        /// <param name="close">The closing price of the period.</param>
        /// <returns>A <see cref="PointCollection"/> containing the points to be charted for the period.</returns>
        public override PointCollection GetPolylinePoints(double center, double? open, double? high, double? low, double close)
        {
            return new PointCollection {new Point(center, close)};
        }
    }
}