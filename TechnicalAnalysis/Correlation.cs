using System;
using System.Linq;
using Sonneville.Statistics;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// Indicates the level of correlation between two <see cref="ITimePeriod"/>.
    /// </summary>
    public class Correlation : TimeSeriesIndicator<decimal>
    {
        public const int DefaultLookback = 20;

        //
        // The algorithms in the Correlation class are based on the following article:
        // http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:correlation_coeffici
        //

        private readonly ITimeSeries<ITimePeriod<decimal>, decimal> _target;

        public Correlation(ITimeSeries<ITimePeriod<decimal>, decimal> timeSeries, int lookback, ITimeSeries<ITimePeriod<decimal>, decimal> target)
            : base(timeSeries, lookback)
        {
            _target = target;
        }

        protected override decimal Calculate(DateTime index)
        {
            var myPeriods = TimeSeriesUtility.GetPreviousTimePeriods(MeasuredTimeSeries, Lookback, index).Select(x => x[index]);
            var otherPeriods = TimeSeriesUtility.GetPreviousTimePeriods(_target, Lookback, index).Select(x => x[index]);
            return myPeriods.Correlation(otherPeriods);
        }

        protected override bool CanCalculate(DateTime index)
        {
            if (TimeSeriesUtility.HasValueInRange(MeasuredTimeSeries, index) && TimeSeriesUtility.HasValueInRange(_target, index))
            {
                return base.CanCalculate(index);
            }
            return false;
        }
    }
}