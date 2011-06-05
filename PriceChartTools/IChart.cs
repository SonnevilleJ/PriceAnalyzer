using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Sonneville.PriceTools;

namespace Sonneville.PriceChartTools
{
    public interface IChart
    {
        /// <summary>
        /// Gets or sets the thickness of the stroke used to draw the chart.
        /// </summary>
        int StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IPriceSeries"/> containing the price data to be charted.
        /// </summary>
        IPriceSeries PriceSeries { get; set; }

        /// <summary>
        /// Gets or sets the space in candle widths that should be placed between candles.
        /// </summary>
        double CandleSpacing { get; set; }

        /// <summary>
        /// Gets or sets the space in candle widths between the last candle and the right edge of the chart.
        /// </summary>
        double BufferRight { get; set; }

        /// <summary>
        /// Gets or sets the space in candle widths between the first candle and the left edge of the chart.
        /// </summary>
        double BufferLeft { get; set; }

        /// <summary>
        /// Gets or sets the space in price units between the <see cref="MaxDisplayedPrice"/> and the top edge of the chart.
        /// </summary>
        double BufferTop { get; set; }

        /// <summary>
        /// Gets or sets the space in price units between the <see cref="MinDisplayedPrice"/> and the bottom edge of the chart.
        /// </summary>
        double BufferBottom { get; set; }

        /// <summary>
        /// Gets or sets the horizontal space taken up by half a candle.
        /// </summary>
        int HalfCandleWidth { get; set; }

        /// <summary>
        /// Gets or sets the horizontal space taken up by a full candle.
        /// </summary>
        int CandleWidth { get; set; }

        /// <summary>
        /// Gets or sets the first period displayed on the chart.
        /// </summary>
        DateTime FirstDisplayedPeriod { get; set; }

        /// <summary>
        /// Gets or sets the last period displayed on the chart.
        /// </summary>
        DateTime LastDisplayedPeriod { get; set; }

        /// <summary>
        /// Gets the lower boundary of the price axis (vertical).
        /// </summary>
        double MinDisplayedPrice { get; }

        /// <summary>
        /// Gets the upper boundary of the price axis (vertical).
        /// </summary>
        double MaxDisplayedPrice { get; }

        /// <summary>
        /// Gets or sets a value indicating if the periods should be connected.
        /// </summary>
        bool ConnectPeriods { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> used to outline a gain day. Default: <see cref="Brushes.Black"/>.
        /// </summary>
        Brush GainStroke { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> used to outline a loss day. Default: <see cref="Brushes.Red"/>.
        /// </summary>
        /// <remarks>This brush will also be used to fill a loss day.</remarks>
        Brush LossStroke { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> used to fill a gain day. Default: <see cref="Brushes.White"/>.
        /// </summary>
        /// <remarks>Loss days will be filled with the <see cref="LossStroke"/> brush.</remarks>
        Brush GainFill { get; set; }

        /// <summary>
        /// Gets the polyline points to chart for a period.
        /// </summary>
        /// <param name="center">The center of the period.</param>
        /// <param name="open">The opening price of the period.</param>
        /// <param name="high">The high price of the period.</param>
        /// <param name="low">The low price of the period.</param>
        /// <param name="close">The closing price of the period.</param>
        /// <returns>A <see cref="PointCollection"/> containing the points to be charted for the period.</returns>
        PointCollection GetPolylinePoints(double center, double? open, double? high, double? low, double close);
    }
}