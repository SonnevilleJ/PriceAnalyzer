using System;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public static class ExtensionImplementations
    {
        private const int DefaultCorrelationLookback = 20;

        public static decimal Correlation(this ITimeSeries timeSeries, ITimeSeries target, DateTime dateTime, int lookback = DefaultCorrelationLookback)
        {
            return Correlation(timeSeries, target, lookback)[dateTime];
        }

        public static ITimeSeries Correlation(this ITimeSeries timeSeries, ITimeSeries target, int lookback = DefaultCorrelationLookback)
        {
            return new Correlation(timeSeries, lookback, target);
        }
    }
}
