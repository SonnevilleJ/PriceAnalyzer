namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// A statistical indicator used to transform <see cref="PriceTools.ITimeSeries"/> data in order to identify a trend, correlation, reversal, or other meaningful information about the underlying data series.
    /// </summary>
    public interface ITimeSeriesIndicator : ITimeSeries
    {
        /// <summary>
        /// Gets the lookback of this TimeSeriesIndicator which specifies how many periods are required for the first indicator value.
        /// </summary>
        /// <example>A 50-period MovingAverage has a Lookback of 50.</example>
        int Lookback { get; set; }

        /// <summary>
        /// The underlying data which is to be analyzed by this ITimeSeriesIndicator.
        /// </summary>
        ITimeSeries MeasuredTimeSeries { get; }

        /// <summary>
        /// Pre-caches all values for this TimeSeriesIndicator.
        /// </summary>
        void CalculateAll();
    }
}