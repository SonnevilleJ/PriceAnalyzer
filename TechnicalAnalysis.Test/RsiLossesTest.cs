using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.TechnicalAnalysis;
using Sonneville.PriceTools.Test.PriceData;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
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

        #region Overrides of CommonIndicatorTests

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
        protected override RsiLosses GetTestObjectInstance(ITimeSeries timeSeries, int lookback)
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

        #endregion

        [TestMethod]
        public void LookbackDefaultsTo2()
        {
            var target = GetTestObjectInstance(CreateTestPriceSeries(10, new DateTime(2012, 10, 15), 1), 10);
            Assert.AreEqual(2, target.Lookback);
        }

        [TestMethod]
        public void CalculateDataCorrectly()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var periods = priceSeries.PricePeriods.ToArray();

            var target = GetTestObjectInstance(priceSeries);

            for (var i = 1; i < 10; i++)
            {
                var previousPeriod = periods.ElementAt(i - 1);
                var currentPeriod = periods.ElementAt(i);

                var expected = currentPeriod.Value() < previousPeriod.Value() ? currentPeriod.Value() - previousPeriod.Value() : 0;
                var actual = target[currentPeriod.Tail];
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
