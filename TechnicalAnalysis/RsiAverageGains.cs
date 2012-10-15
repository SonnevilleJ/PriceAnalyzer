namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public class RsiAverageGains : RsiAverageGainsLossesIndicator
    {
        public RsiAverageGains(ITimeSeries timeSeries, int lookback)
            : base(new RsiGainsIndicator(timeSeries), lookback)
        {
        }
    }
}
