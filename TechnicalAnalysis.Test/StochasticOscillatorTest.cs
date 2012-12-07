using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.TechnicalAnalysis;
using Sonneville.PriceTools.Test.PriceData;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public class StochasticOscillatorTest : CommonIndicatorTests<StochasticOscillator>
    {

        //
        // The algorithms in the StochasticOscillator class are based on an Excel calculator from the following article:
        // http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:stochastic_oscillator
        // See the cs-soo.xls file in the Resources folder.
        //

        private decimal[] _expected14
        {
            get
            {
                return new[]
                           {
                               75.686673448626653102746693790m,
                               84.63886063072227873855544252m,
                               76.042780748663101604278074870m,
                               92.31692677070828331332533013m,
                               92.19687875150060024009603842m,
                               63.834951456310679611650485440m,
                               85.22388059701492537313432836m,
                               92.24137931034482758620689655m,
                               97.95918367346938775510204082m,
                               89.65986394557823129251700680m
                           };
            }
        }

        protected override int GetDefaultLookback()
        {
            return 14;
        }

        protected override StochasticOscillator GetTestObjectInstance(ITimeSeries timeSeries, int lookback)
        {
            var priceSeries = timeSeries as IPriceSeries;
            if(priceSeries == null) Assert.Fail("Indicator construction requires PriceSeries object.");
            
            return new StochasticOscillator(priceSeries, lookback);
        }

        [TestMethod]
        public void StochasticDTestDefaultOf3()
        {
            TestD(3);
        }

        [TestMethod]
        public void StochasticDTest10()
        {
            TestD(10);
        }

        private void TestD(int lookback)
        {
            var target = GetTestObjectInstance(TestPriceSeries.DE_1_1_2011_to_6_30_2011);
            target.SignalLineLookback = lookback;
            var smaD = new SimpleMovingAverage(target, lookback);

            for (var i = 0; i < 10; i++)
            {
                var testDateTime = smaD.Head.SeekTradingPeriods(i, Resolution.Days);
                var expected = smaD[testDateTime];
                var actual = target.D[testDateTime];
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Gets a list of expected values for a given lookback period.
        /// </summary>
        /// <param name="lookback"></param>
        /// <returns></returns>
        protected override decimal[] GetExpectedValues(int lookback)
        {
            switch (lookback)
            {
                case 14:
                    return _expected14;
                default:
                    Assert.Inconclusive("Expected values for lookback period of {0} are unknown.", lookback);
                    return null;
            }
        }
    }
}
