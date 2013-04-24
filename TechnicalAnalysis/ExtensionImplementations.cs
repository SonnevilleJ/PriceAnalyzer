using System;
using System.Collections.Generic;
using System.Linq;
using Statistics;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public static class ExtensionImplementations
    {
        private const int DefaultCorrelationLookback = 20;

        public static decimal Correlation(this ITimeSeries timeSeriesA, ITimeSeries timeSeries, DateTime dateTime, int lookback = DefaultCorrelationLookback)
        {
            var tsa = timeSeriesA.GetPreviousTimePeriods(lookback, dateTime).Select(x => x.Value());
            var tsb = timeSeries.GetPreviousTimePeriods(lookback, dateTime).Select(x => x.Value());
            return tsa.Correlation(tsb);
        }

        public static ITimeSeries Correlation(this IPriceSeries priceSeries, ITimeSeries timeSeries, int lookback = DefaultCorrelationLookback)
        {
            return new Correlation(priceSeries, lookback, timeSeries);
        }

        private static DateTime GetCommonHead(ITimeSeries timeSeriesA, ITimeSeries timeSeriesB, int lookback = DefaultCorrelationLookback)
        {
            var earliestA = timeSeriesA.TimePeriods.Skip(lookback).First().Head;
            var earliestB = timeSeriesB.TimePeriods.Skip(lookback).First().Head;
            var commonHead = earliestA <= earliestB ? earliestA : earliestB;

            return commonHead;
        }

        private static DateTime GetCommonTail(ITimeSeries timeSeriesA, ITimeSeries timeSeriesB, int lookback = DefaultCorrelationLookback)
        {
            var lastA = timeSeriesA.TimePeriods.Skip(lookback).Last().Tail;
            var lastB = timeSeriesB.TimePeriods.Skip(lookback).Last().Tail;
            var commonTail = lastA <= lastB ? lastA : lastB;

            return commonTail;
        }
    }
}
