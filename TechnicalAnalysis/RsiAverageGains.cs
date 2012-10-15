namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public class RsiAverageGains : RsiAverageGainsLosses
    {
        public RsiAverageGains(ITimeSeries timeSeries, int lookback)
            : base(new RsiGains(timeSeries), lookback)
        {
        }
    }
}
