using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class SimpleMovingAverageTest
    {
        [TestMethod]
        public void FlatPeriodReturnsSameAverage()
        {
            var date = new DateTime(2011, 3, 1);
            const int price = 2;

            var series = CreateTestPriceSeries(10, date, price);

            const int lookback = 2;
            var ma = new SimpleMovingAverage(series, lookback);

            const decimal expected = price;
            for (var i = lookback; i < series.PricePeriods.Count; i++)
            {
                decimal? actual = ma[date.AddDays(i)];
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void RisingAndFallingMovingAverageReturnsCorrectValues()
        {
            var p1 = new QuotedPricePeriod();
            var p2 = new QuotedPricePeriod();
            var p3 = new QuotedPricePeriod();
            var p4 = new QuotedPricePeriod();
            var p5 = new QuotedPricePeriod();
            var p6 = new QuotedPricePeriod();
            var p7 = new QuotedPricePeriod();
            var p8 = new QuotedPricePeriod();
            var p9 = new QuotedPricePeriod();

            var date = new DateTime(2000, 1, 1);
            p1.AddPriceQuotes(new PriceQuote {SettlementDate = date, Price = 1});
            p2.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(1), Price = 2});
            p3.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(2), Price = 3});
            p4.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(3), Price = 4});
            p5.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(4), Price = 5});
            p6.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(5), Price = 4});
            p7.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(6), Price = 3});
            p8.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(7), Price = 2});
            p9.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(8), Price = 1});

            var series = PriceSeriesFactory.CreatePriceSeries("test");
            series.AddPriceData(p1);
            series.AddPriceData(p2);
            series.AddPriceData(p3);
            series.AddPriceData(p4);
            series.AddPriceData(p5);
            series.AddPriceData(p6);
            series.AddPriceData(p7);
            series.AddPriceData(p8);
            series.AddPriceData(p9);

            // create 4 day moving average
            const int lookback = 4;
            var target = new SimpleMovingAverage(series, lookback);

            target.CalculateAll();
            Assert.AreEqual(2.5m, target[date.AddDays(3)]);
            Assert.AreEqual(3.5m, target[date.AddDays(4)]);
            Assert.AreEqual(4.0m, target[date.AddDays(5)]);
            Assert.AreEqual(4.0m, target[date.AddDays(6)]);
            Assert.AreEqual(3.5m, target[date.AddDays(7)]);
            Assert.AreEqual(2.5m, target[date.AddDays(8)]);
        }

        private static IPriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price)
        {
            var series = PriceSeriesFactory.CreatePriceSeries("test");
            for (var i = 0; i < count; i++)
            {
                var period = new QuotedPricePeriod();
                period.AddPriceQuotes(new PriceQuote {SettlementDate = startDate.AddDays(i), Price = price});
                series.AddPriceData(period);
            }
            return series;
        }
    }
}
