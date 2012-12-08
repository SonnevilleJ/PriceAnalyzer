using System.Linq;
using Sonneville.PriceTools;
using Sonneville.PriceTools.TechnicalAnalysis;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    class StochRSITest : CommonIndicatorTests<StochRSI>
    {

        //
        // The algorithms in the StochRSI class are based on an Excel calculator from the following article:
        // http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:stochrsi
        // See the cs-stochrsi.xls file in the Resources folder.
        //

        private ITimeSeries _timeSeries;

        #region Overrides of CommonIndicatorTests<StochRSI>

        protected override int GetDefaultLookback()
        {
            return 14;
        }

        protected override StochRSI GetTestObjectInstance(ITimeSeries timeSeries, int lookback)
        {
            _timeSeries = timeSeries;

            return new StochRSI(timeSeries, lookback);
        }

        protected override decimal[] GetExpectedValues(int lookback)
        {
            var rsi = new RelativeStrengthIndex(_timeSeries, lookback);
            var stochastic = new StochasticOscillator(rsi, lookback);

            stochastic.CalculateAll();
            return stochastic.TimePeriods.Take(10).Select(p=>p.Value()).ToArray();
        }

        #endregion
    }
}