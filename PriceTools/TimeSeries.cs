using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of data.
    /// </summary>
    public interface TimeSeries
    {
        /// <summary>
        /// Gets a value stored at a given DateTime index of the TimeSeries.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the TimeSeries as of the given DateTime.</returns>
        decimal this[DateTime dateTime] { get; }

        /// <summary>
        /// Gets the first DateTime in the TimeSeries.
        /// </summary>
        DateTime Head { get; }

        /// <summary>
        /// Gets the last DateTime in the TimeSeries.
        /// </summary>
        DateTime Tail { get; }

        /// <summary>
        /// Gets the <see cref="Resolution"/> of price data stored within the TimeSeries.
        /// </summary>
        Resolution Resolution { get; }

        /// <summary>
        /// Gets the values stored within the TimeSeries.
        /// </summary>
        IDictionary<DateTime, decimal> Values { get; }

        /// <summary>
        /// Determines if the TimeSeries has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the TimeSeries has a valid value for the given date.</returns>
        bool HasValueInRange(DateTime settlementDate);
    }
}
