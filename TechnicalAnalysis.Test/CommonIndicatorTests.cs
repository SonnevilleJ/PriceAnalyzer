using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Test.Utilities;

namespace Sonneville.PriceTools.TechnicalAnalysis.Test
{
    [TestClass]
    public class CommonIndicatorTests
    {
        [TestMethod]
        public void ResolutionDaysByDefault()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(20, date, 1);

            const int lookback = 5;
            var target = new SimpleMovingAverage(priceSeries, lookback);

            const Resolution expected = Resolution.Days;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
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

            var expected = priceSeries.PricePeriods[target.Lookback - 1].Head;
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IndexerTest1()
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

        [TestMethod]
        public void IndexerTest2()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);
            const int lookback = 4;

            var target = new SimpleMovingAverage(priceSeries, lookback);

            var expected = priceSeries[lookback];
            var actual = target[0];
            Assert.AreEqual(expected, actual);
        }

        private static PriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price)
        {
            var series = PriceSeriesFactory.CreatePriceSeries(TestUtilities.GetUniqueTicker());
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