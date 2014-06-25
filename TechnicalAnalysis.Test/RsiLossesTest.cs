using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.SampleData;

namespace Sonneville.PriceTools.TechnicalAnalysis.Test
{
    [TestClass]
    public class RsiLossesTest : CommonIndicatorTests<RsiLosses>
    {
        private decimal[] _expected2
        {
            get
            {
                return new[]
                           {
                               -0.57m,
                               0.0m,
                               0.0m,
                               0.0m,
                               0.0m,
                               0.0m,
                               0.0m,
                               0.0m,
                               0.0m,
                               0.0m
                           };
            }
        }

        /// <summary>
        /// The default lookback period to use when creating test instances.
        /// </summary>
        protected override int GetDefaultLookback()
        {
            return 2;
        }

        /// <summary>
        /// Gets an instance of the <see cref="TimeSeriesIndicator"/> to test, using a specific lookback period.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to transform.</param>
        /// <param name="lookback">The lookback period the <see cref="TimeSeriesIndicator"/> should use.</param>
        /// <returns></returns>
        protected override RsiLosses GetTestObjectInstance(ITimeSeries<ITimePeriod> timeSeries, int lookback)
        {
            return new RsiLosses(timeSeries);
        }

        protected override decimal[] GetExpectedValues(int lookback)
        {
            switch (lookback)
            {
                case 2:
                    return _expected2;
                default:
                    Assert.Inconclusive("Expected values for lookback period of {0} are unknown.", lookback);
                    return null;
            }
        }

        [TestMethod]
        public void LookbackDefaultsTo2()
        {
            var target = GetTestObjectInstance(CreateTestPriceSeries(10, new DateTime(2012, 10, 15), 1), 10);
            Assert.AreEqual(2, target.Lookback);
        }

        [TestMethod]
        public void CalculateDataCorrectly()
        {
            var priceSeries = SamplePriceDatas.Deere.PriceSeries;
            var periods = priceSeries.PricePeriods.ToArray();

            var target = GetTestObjectInstance(priceSeries);

            for (var i = 1; i < 10; i++)
            {
                var previousPeriod = periods.ElementAt(i - 1);
                var currentPeriod = periods.ElementAt(i);

                var expected = currentPeriod.Value<decimal>() < previousPeriod.Value<decimal>() ? currentPeriod.Value<decimal>() - previousPeriod.Value<decimal>() : 0;
                var actual = target[currentPeriod.Tail];
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
