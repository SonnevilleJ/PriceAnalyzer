namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public class RsiAverageLosses : RsiAverageGainsLosses
    {
        public RsiAverageLosses(ITimeSeries<ITimePeriod<decimal>, decimal> timeSeries, int lookback)
            : base(new RsiLosses(timeSeries), lookback)
        {
        }
    }
}