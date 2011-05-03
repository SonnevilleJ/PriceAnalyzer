using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
    public interface IPricePeriod : ITimeSeries, IEquatable<IPricePeriod>
    {
        /// <summary>
        /// Gets the last price for the IPricePeriod.
        /// </summary>
        decimal Last { get; }

        /// <summary>
        /// Gets the closing price for the IPricePeriod.
        /// </summary>
        decimal Close { get; }

        /// <summary>
        /// Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        decimal? High { get; }

        /// <summary>
        /// Gets the lowest price that occurred during the IPricePeriod.
        /// </summary>
        decimal? Low { get; }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        decimal? Open { get; }

        /// <summary>
        /// Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        Int64? Volume { get; }

        /// <summary>
        ///   Gets a <see cref = "TimeSpan" /> value indicating the length of time covered by this IPricePeriod.
        /// </summary>
        TimeSpan TimeSpan { get; }

        /// <summary>
        ///   Event which is invoked when new price data is available for the IPricePeriod.
        /// </summary>
        event EventHandler<NewPriceDataAvailableEventArgs> NewPriceDataAvailable;
    }
}
