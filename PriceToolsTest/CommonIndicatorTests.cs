using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class CommonIndicatorTests
    {
        [TestMethod]
        public void ResolutionDaysByDefault()
        {
            DateTime date = new DateTime(2011, 3, 1);
            IPriceSeries priceSeries = CreateTestPriceSeries(20, date, 1);

            const int lookback = 5;
            SimpleMovingAverage target = new SimpleMovingAverage(priceSeries, lookback);

            const Resolution expected = Resolution.Days;
            Resolution actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void QueryBeforeHeadThrowsException()
        {
            IPriceSeries series = CreateTestPriceSeries(4, new DateTime(2011, 1, 6), 2);
            SimpleMovingAverage ma = new SimpleMovingAverage(series, 2);

            var result = ma[ma.Head.Subtract(new TimeSpan(1))];
        }

        [TestMethod]
        public void HeadTest()
        {
            DateTime date = new DateTime(2011, 3, 1);
            IPriceSeries priceSeries = CreateTestPriceSeries(10, date, 1);
            const int lookback = 4;

            SimpleMovingAverage target = new SimpleMovingAverage(priceSeries, lookback);

            DateTime expected = priceSeries.GetPricePeriods(target.Resolution)[target.Lookback - 1].Head;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        private static IPriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price)
        {
            PriceSeries series = PriceSeriesFactory.CreatePriceSeries("test");
            for (int i = 0; i < count; i++)
            {
                QuotedPricePeriod period = new QuotedPricePeriod();
                period.AddPriceQuotes(new PriceQuote { SettlementDate = startDate.AddDays(i), Price = price });
                series.DataPeriods.Add(period);
            }
            return series;
        }

    }
}