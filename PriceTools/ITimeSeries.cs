using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of data.
    /// </summary>
    public interface ITimeSeries : ISerializable
    {
        /// <summary>
        /// Gets a value stored at a given index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The index of the desired value.</param>
        /// <returns>The value stored at the given index.</returns>
        decimal this[int index] { get; }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        decimal this[DateTime index] { get; }

        /// <summary>
        /// Gets the span of the ITimeSeries.
        /// </summary>
        int Span { get; }

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        DateTime Head { get; }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        DateTime Tail { get; }
    }
}
