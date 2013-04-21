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
        //
        // The algorithms in the Correlation class are based on the following article:
        // http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:correlation_coeffici
        //

        #region Private Members

        private readonly ITimeSeries _target;

        #endregion

        #region Constructors

        public Correlation(IPriceSeries priceSeries, int lookback, ITimeSeries target)
            : base(priceSeries, lookback)
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
    }
}