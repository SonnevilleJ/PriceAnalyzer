using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.TechnicalAnalysis;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public abstract class CommonIndicatorTests
    {
        protected abstract Indicator GetTestInstance(ITimeSeries timeSeries, int lookback);

        [TestMethod]
        public void ResolutionDaysMatchesPriceSeries()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(20, date, 1);

            const int lookback = 5;
            var target = GetTestInstance(priceSeries, lookback);

            var expected = priceSeries.Resolution;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void QueryBeforeHeadThrowsException()
        {
            var series = CreateTestPriceSeries(4, new DateTime(2011, 1, 6), 2);
            var target = GetTestInstance(series, 2);

            var result = target[target.Head.AddTicks(-1)];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void QueryBeforeHeadCloseThrowsException()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);
            const int lookback = 4;

            var target = GetTestInstance(priceSeries, lookback);

            // CreateTestPriceSeries above does NOT create a full period for the resolution (TickedPricePeriodImpl)
            var tail = priceSeries.PricePeriods[lookback - 1].Tail;
            var result = target[tail.AddTicks(-1)];
        }

        [TestMethod]
        public void CalculateAllDoesNotThrow()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);
            const int lookback = 4;

            var target = GetTestInstance(priceSeries, lookback);

            target.CalculateAll();
        }

        [TestMethod]
        public void IndexerTest()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);
            const int lookback = 4;

            var target = GetTestInstance(priceSeries, lookback);

            var testDate = date.AddDays(lookback);
            var expected = priceSeries[testDate];
            var actual = target[testDate];
            Assert.AreEqual(expected, actual);
        }

        private static IPriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price)
        {
            var series = PriceSeriesFactory.CreatePriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            for (var i = 0; i < count; i++)
            {
                var period = PricePeriodFactory.ConstructTickedPricePeriod();
                period.AddPriceTicks(PriceTickFactory.ConstructPriceTick(startDate.AddDays(i), price));
                series.AddPriceData(period);
            }
            return series;
        }
    }
}