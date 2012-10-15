namespace Sonneville.PriceTools.TechnicalAnalysis
{
    class RsiAverageLosses : RsiAverageGainsLossesIndicator
    {
        public RsiAverageLosses(ITimeSeries timeSeries, int lookback)
            : base(new RsiLossesIndicator(timeSeries), lookback)
        {
        }
    }
}