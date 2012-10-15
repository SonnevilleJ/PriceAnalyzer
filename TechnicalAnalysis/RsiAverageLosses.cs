namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public class RsiAverageLosses : RsiAverageGainsLosses
    {
        public RsiAverageLosses(ITimeSeries timeSeries, int lookback)
            : base(new RsiLosses(timeSeries), lookback)
        {
        }
    }
}