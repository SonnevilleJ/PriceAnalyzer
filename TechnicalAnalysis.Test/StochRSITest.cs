using System.Linq;
using Sonneville.PriceTools;
using Sonneville.PriceTools.TechnicalAnalysis;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    class StochRSITest : CommonIndicatorTests<StochRSI>
    {
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