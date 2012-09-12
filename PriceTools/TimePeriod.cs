using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of data.
    /// </summary>
    public interface TimePeriod
    {
        /// <summary>
        /// Gets a value stored at a given DateTime index of the TimePeriod.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the TimePeriod as of the given DateTime.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1043:UseIntegralOrStringArgumentForIndexers")]
        decimal this[DateTime dateTime] { get; }

        /// <summary>
        /// Gets the first DateTime in the TimePeriod.
        /// </summary>
        DateTime Head { get; }

        /// <summary>
        /// Gets the last DateTime in the TimePeriod.
        /// </summary>
        DateTime Tail { get; }

        /// <summary>
        /// Gets the <see cref="Resolution"/> of price data stored within the TimePeriod.
        /// </summary>
        Resolution Resolution { get; }

        /// <summary>
        /// Determines if the TimePeriod has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the TimePeriod has a valid value for the given date.</returns>
        bool HasValueInRange(DateTime settlementDate);
    }
}
