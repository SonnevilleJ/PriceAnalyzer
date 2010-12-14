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
        /// Gets the span of the ITimeSeries.
        /// </summary>
        int Span { get; }
    }
}
