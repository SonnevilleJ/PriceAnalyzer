namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public class RsiAverageGains : RsiAverageGainsLosses
    {
        public RsiAverageGains(ITimeSeries<ITimePeriod<decimal>, decimal> timeSeries, int lookback)
            : base(new RsiGains(timeSeries), lookback)
        {
        }
    }
}
