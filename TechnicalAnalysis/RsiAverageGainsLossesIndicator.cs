using System;
using System.Linq;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    public class RsiAverageGainsLossesIndicator : Indicator
    {
        public RsiAverageGainsLossesIndicator(ITimeSeries timeSeries, int lookback)
            : base(timeSeries, lookback)
        {
        }

        #region Overrides of Indicator
        
        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate.</param>
        protected override decimal Calculate(DateTime index)
        {
            // if first period
            if (MeasuredTimeSeries.TimePeriods.Count(p => p.Tail < index) == Lookback - 1)
                return MeasuredTimeSeries.TimePeriods.Take(Lookback).Average(p => p.Value());
            
            // if not first period
            var previousTail = MeasuredTimeSeries.TimePeriods.Last(p => p.Tail < index).Tail;
            var previousAverageLoss = this[previousTail];
            var currentLoss = MeasuredTimeSeries[index];
            return ((previousAverageLoss*(Lookback - 1)) + currentLoss)/Lookback;
        }

        #endregion
    }
}
