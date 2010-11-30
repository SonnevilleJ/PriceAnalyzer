namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of data.
    /// </summary>
    public interface ITimeSeries
    {
        /// <summary>
        /// Gets a value stored at a given index of the ITimeSeries.
        /// </summary>
        /// <param name="i">The index of the desired value.</param>
        /// <returns>The value stored at the given index.</returns>
        decimal this[int i] { get; }

        /// <summary>
        /// Gets the span of the ITimeSeries.
        /// </summary>
        int Span { get; }
    }
}
