using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of data.
    /// </summary>
    public interface ITimePeriod : IVariableValue
    {
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

        /// <summary>
        ///   Event which is invoked when new data is available for the ITimePeriod.
        /// </summary>
        event EventHandler<NewDataAvailableEventArgs> NewDataAvailable;
    }
}
