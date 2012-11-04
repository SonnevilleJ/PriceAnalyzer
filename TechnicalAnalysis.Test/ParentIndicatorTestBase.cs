using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Test.PriceData;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public abstract class ParentIndicatorTestBase : CommonIndicatorTests
    {
        /// <summary>
        /// Gets a list of expected values for a given lookback period.
        /// </summary>
        /// <param name="lookback"></param>
        /// <returns></returns>
        protected abstract decimal[] Get11ExpectedValues(int lookback);

        [TestMethod]
        public void CalculateFirstPeriodCorrectly()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            const int lookback = 14;
            var target = GetTestObjectInstance(priceSeries, lookback);

            var expected = Get11ExpectedValues(lookback)[0];
            var actual = target[target.Head];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNext10PeriodsCorrectly()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            const int lookback = 14;
            var target = GetTestObjectInstance(priceSeries, lookback);
            target.CalculateAll();

            for (var i = 1; i < Get11ExpectedValues(lookback).Length; i++)
            {
                var expected = Get11ExpectedValues(lookback)[i];
                var actual = target.TimePeriods.ToArray()[i].Value();
                Assert.AreEqual(expected, actual);
            }
        }
    }
}