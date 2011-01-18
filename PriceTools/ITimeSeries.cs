using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of data.
    /// </summary>
    public interface ITimeSeries : ISerializable, IEquatable<ITimeSeries>
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
        /// Determines if the ITimeSeries has a valid value for a given asOfDate.
        /// </summary>
        /// <param name="asOfDate">The asOfDate to check.</param>
        /// <returns>A value indicating if the ITimeSeries has a valid value for the given asOfDate.</returns>
        bool HasValue(DateTime date);
    }
}
