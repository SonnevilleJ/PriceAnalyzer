using System;
using System.Linq;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// A momentum oscillator which measures the speed of changes in price movements.
    /// </summary>
    public class RelativeStrengthIndex : TimeSeriesIndicator
    {
        public const int DefaultLookback = 14;
        //
        // The algorithms in the RelativeStrengthIndex class are based on an Excel calculator from the following article:
        // http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:relative_strength_index_rsi
        //
        private ITimeSeriesIndicator _avgGains;
        private ITimeSeriesIndicator _avgLosses;

        public RelativeStrengthIndex(ITimeSeries timeSeries, int lookback = DefaultLookback)
            : base(timeSeries, lookback)
        {
            _avgGains = new RsiAverageGains(MeasuredTimeSeries, Lookback);
            _avgLosses = new RsiAverageLosses(MeasuredTimeSeries, Lookback);
        }

        protected override void ClearCachedValues()
        {
            _avgGains = new RsiAverageGains(MeasuredTimeSeries, Lookback);
            _avgLosses = new RsiAverageLosses(MeasuredTimeSeries, Lookback);
            base.ClearCachedValues();
        }
        public override DateTime Head
        {
            get { return MeasuredTimeSeries.TimePeriods.ElementAt(Lookback).Head; }
        }

        /// <summary>
        /// Calculates a single value of this TimeSeriesIndicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate.</param>
        protected override decimal Calculate(DateTime index)
        {
            var avgGain = _avgGains[index];
            var avgLoss = _avgLosses[index];
            
            if (avgLoss == 0) return 100m;

            var relativeStrength = Math.Abs(avgGain/avgLoss);
            var relativeStrengthIndex = 100 - (100/(1 + relativeStrength));
            return relativeStrengthIndex;
        }
    }
}