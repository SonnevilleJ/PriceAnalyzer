using System;
using System.Linq;
using Statistics;

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

        #region Private Members

        private readonly ITimeSeries _target;

        #endregion

        #region Constructors

        public Correlation(ITimeSeries timeSeries, int lookback, ITimeSeries target)
            : base(timeSeries, lookback)
        {
            _target = target;
        }

        #endregion

        protected override decimal Calculate(DateTime index)
        {
            var myPeriods = MeasuredTimeSeries.GetPreviousTimePeriods(Lookback, index).Select(x => x[index]);
            var otherPeriods = _target.GetPreviousTimePeriods(Lookback, index).Select(x => x[index]);
            return myPeriods.Correlation(otherPeriods);
        }

        protected override bool CanCalculate(DateTime index)
        {
            if (MeasuredTimeSeries.HasValueInRange(index) && _target.HasValueInRange(index))
            {
                return base.CanCalculate(index);
            }
            return false;
        }
    }
}