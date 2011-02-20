﻿using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents price history for a desired length of time.
    /// </summary>
    public interface IPricePeriod : ITimeSeries
    {
        /// <summary>
        /// Gets the closing price for the IPricePeriod.
        /// </summary>
        decimal Close { get; }

        /// <summary>
        /// Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        decimal? High { get; }

        /// <summary>
        /// Gets the lowest price that occurred during  the IPricePeriod.
        /// </summary>
        decimal? Low { get; }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        decimal? Open { get; }

        /// <summary>
        /// Gets a TimeSpan representing the total duration of the IPricePeriod.
        /// </summary>
        TimeSpan TimeSpan { get; }

        /// <summary>
        /// Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        Int64? Volume { get; }
    }
}
