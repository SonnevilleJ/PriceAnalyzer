using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.TechnicalAnalysis;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public class CommonIndicatorTests
    {
        [TestMethod]
        public void ResolutionDaysMatchesPriceSeries()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(20, date, 1);

            const int lookback = 5;
            var target = new SimpleMovingAverage(priceSeries, lookback);

            var expected = priceSeries.Resolution;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void QueryBeforeHeadThrowsException()
        {
            var series = CreateTestPriceSeries(4, new DateTime(2011, 1, 6), 2);
            var ma = new SimpleMovingAverage(series, 2);

            var result = ma[ma.Head.Subtract(new TimeSpan(1))];
        }

        [TestMethod]
        public void HeadTest()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);
            const int lookback = 4;

            var target = new SimpleMovingAverage(priceSeries, lookback);

            Assert.IsNotNull(priceSeries.PricePeriods[target.Lookback - 1].Head);
        }

        [TestMethod]
        public void CalculateAllDoesNotThrow()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);
            const int lookback = 4;

            var target = new SimpleMovingAverage(priceSeries, lookback);

            target.CalculateAll();
        }

        [TestMethod]
        public void IndexerTest()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);
            const int lookback = 4;

            var target = new SimpleMovingAverage(priceSeries, lookback);

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