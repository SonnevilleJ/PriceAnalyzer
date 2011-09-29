using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A statistical indicator used to transform <see cref="ITimeSeries"/> data in order to identify a trend, correlation, reversal, or other meaningful information about the underlying ITimeSeries data.
    /// </summary>
    public interface IIndicator : ITimeSeries
    {
        /// <summary>
        /// Gets the range of this Indicator which specifies how many periods are required for the first indicator value.
        /// </summary>
        /// <example>A 50-period MovingAverage has a Lookback of 50.</example>
        int Lookback { get; }

        /// <summary>
        /// The underlying data which is to be analyzed by this Indicator.
        /// </summary>
        ITimeSeries PriceSeries { get; }

        /// <summary>
        /// The Resolution of this Indicator. Used when splitting the PriceSeries into periods.
        /// </summary>
        Resolution Resolution { get; }

        /// <summary>
        /// Pre-caches all values for this Indicator.
        /// </summary>
        void CalculateAll();
    }
}