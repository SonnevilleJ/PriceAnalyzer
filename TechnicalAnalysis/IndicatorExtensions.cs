namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public static class IndicatorExtensions
    {
        public static SimpleMovingAverage SimpleMovingAverage(this ITimeSeries timeSeries, int lookback)
        {
            return new SimpleMovingAverage(timeSeries, lookback);
        }

        public static RelativeStrengthIndex RelativeStrengthIndex(this ITimeSeries timeSeries, int lookback = TechnicalAnalysis.RelativeStrengthIndex.DefaultLookback)
        {
            return new RelativeStrengthIndex(timeSeries, lookback);
        }

        public static StochasticOscillator StochasticOscillator(this ITimeSeries timeSeries, int lookback)
        {
            return new StochasticOscillator(timeSeries, lookback);
        }

        public static StochRSI StochRsi(this ITimeSeries timeSeries, int lookback)
        {
            return new StochRSI(timeSeries, lookback);
        }

        public static Correlation Correlation(this ITimeSeries timeSeries, ITimeSeries target, int lookback = TechnicalAnalysis.Correlation.DefaultLookback)
        {
            return new Correlation(timeSeries, lookback, target);
        }
    }
}
