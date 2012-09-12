using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
    public interface IPricePeriod : TimePeriod
    {
        /// <summary>
        /// Gets the closing price for the PricePeriod.
        /// </summary>
        decimal Close { get; }

        /// <summary>
        /// Gets the highest price that occurred during the PricePeriod.
        /// </summary>
        decimal High { get; }

        /// <summary>
        /// Gets the lowest price that occurred during the PricePeriod.
        /// </summary>
        decimal Low { get; }

        /// <summary>
        /// Gets the opening price for the PricePeriod.
        /// </summary>
        decimal Open { get; }

        /// <summary>
        /// Gets the total volume of trades during the PricePeriod.
        /// </summary>
        long? Volume { get; }

        /// <summary>
        ///   Event which is invoked when new price data is available for the PricePeriod.
        /// </summary>
        event EventHandler<NewDataAvailableEventArgs> NewDataAvailable;
    }
}