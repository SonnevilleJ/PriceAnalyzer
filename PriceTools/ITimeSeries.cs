namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of data.
    /// </summary>
    public interface ITimeSeries
    {
        /// <summary>
        /// Gets a value stored at a given index of the time series.
        /// </summary>
        /// <param name="i">The index of the desired value.</param>
        /// <returns>The value stored at the given index.</returns>
        decimal this[int i] { get; }
    }
}