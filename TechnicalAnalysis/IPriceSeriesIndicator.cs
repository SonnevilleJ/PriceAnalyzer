namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public interface IPriceSeriesIndicator : ITimeSeriesIndicator<decimal>
    {
        /// <summary>
        /// The underlying data which is to be analyzed by this IPriceSeriesIndicator.
        /// </summary>
        IPriceSeries MeasuredPriceSeries { get; }
    }
}