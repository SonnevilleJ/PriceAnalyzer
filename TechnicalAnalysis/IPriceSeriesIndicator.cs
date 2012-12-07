namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public interface IPriceSeriesIndicator : ITimeSeriesIndicator
    {
        /// <summary>
        /// The underlying data which is to be analyzed by this IPriceSeriesIndicator.
        /// </summary>
        IPriceSeries MeasuredPriceSeries { get; }
    }
}