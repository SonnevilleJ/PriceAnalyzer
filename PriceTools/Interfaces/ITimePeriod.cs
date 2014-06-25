namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of data.
    /// </summary>
    public interface ITimePeriod<out TPeriodValue> : IVariableValue<TPeriodValue>
    {
        /// <summary>
        /// Gets the <see cref="Resolution"/> of price data stored within the ITimePeriod.
        /// </summary>
        Resolution Resolution { get; }
    }
}
