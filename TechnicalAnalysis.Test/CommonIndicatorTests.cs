using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        private static PriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price)
        {
            var series = PriceSeriesFactory.CreatePriceSeries("test");
            for (var i = 0; i < count; i++)
            {
                var period = new QuotedPricePeriod();
                period.AddPriceQuotes(PriceQuoteFactory.ConstructPriceQuote(startDate.AddDays(i), price));
                series.AddPriceData(period);
            }
            return series;
        }

    }
}