using System;
using System.Linq;
using Sonneville.Statistics;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// Indicates the level of correlation between two <see cref="ITimePeriod"/>.
    /// </summary>
    public class Correlation : TimeSeriesIndicator
    {
        public const int DefaultLookback = 20;

        //
        // The algorithms in the Correlation class are based on the following article:
        // http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:correlation_coeffici
        //

        private readonly ITimeSeries _target;

        public Correlation(ITimeSeries timeSeries, int lookback, ITimeSeries target)
            : base(timeSeries, lookback)
        {
            _target = target;
        }

        protected override decimal Calculate(DateTime index)
        {
            var myPeriods = new TimeSeriesUtility().GetPreviousTimePeriods(MeasuredTimeSeries, Lookback, index).Select(x => x[index]);
            var otherPeriods = new TimeSeriesUtility().GetPreviousTimePeriods(_target, Lookback, index).Select(x => x[index]);
            return myPeriods.Correlation(otherPeriods);
        }

        protected override bool CanCalculate(DateTime index)
        {
            if (new TimeSeriesUtility().HasValueInRange(MeasuredTimeSeries, index) && new TimeSeriesUtility().HasValueInRange(_target, index))
            {
                return base.CanCalculate(index);
            }
            return false;
        }
    }
}