using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of data.
    /// </summary>
    public interface ITimePeriod
    {
        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimePeriod.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimePeriod as of the given DateTime.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1043:UseIntegralOrStringArgumentForIndexers")]
        decimal this[DateTime dateTime] { get; }

        /// <summary>
        /// Gets the first DateTime in the ITimePeriod.
        /// </summary>
        DateTime Head { get; }

        /// <summary>
        /// Gets the last DateTime in the ITimePeriod.
        /// </summary>
        DateTime Tail { get; }

        /// <summary>
        /// Gets the <see cref="Resolution"/> of price data stored within the ITimePeriod.
        /// </summary>
        Resolution Resolution { get; }

        /// <summary>
        /// Determines if the ITimePeriod has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimePeriod has a valid value for the given date.</returns>
        bool HasValueInRange(DateTime settlementDate);
    }
}
