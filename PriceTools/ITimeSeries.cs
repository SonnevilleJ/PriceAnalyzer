using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of data.
    /// </summary>
    public interface ITimeSeries : IEquatable<object>
    {
        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        decimal this[DateTime index] { get; }

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        DateTime Head { get; }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        DateTime Tail { get; }

        /// <summary>
        /// Determines if the ITimeSeries has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimeSeries has a valid value for the given date.</returns>
        bool HasValue(DateTime settlementDate);
    }
}
