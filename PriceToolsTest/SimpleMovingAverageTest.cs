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
            DateTime date = new DateTime(2011, 3, 1);
            const int price = 2;

            IPriceSeries series = CreateTestPriceSeries(10, date, price);

            const int lookback = 2;
            SimpleMovingAverage ma = new SimpleMovingAverage(series, lookback);

            const decimal expected = price;
            for (int i = lookback; i < series.PricePeriods.Count; i++)
            {
                decimal? actual = ma[date.AddDays(i)];
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void RisingAndFallingMovingAverageReturnsCorrectValues()
        {
            QuotedPricePeriod p1 = new QuotedPricePeriod();
            QuotedPricePeriod p2 = new QuotedPricePeriod();
            QuotedPricePeriod p3 = new QuotedPricePeriod();
            QuotedPricePeriod p4 = new QuotedPricePeriod();
            QuotedPricePeriod p5 = new QuotedPricePeriod();
            QuotedPricePeriod p6 = new QuotedPricePeriod();
            QuotedPricePeriod p7 = new QuotedPricePeriod();
            QuotedPricePeriod p8 = new QuotedPricePeriod();
            QuotedPricePeriod p9 = new QuotedPricePeriod();

            DateTime date = new DateTime(2000, 1, 1);
            p1.AddPriceQuotes(new PriceQuote {SettlementDate = date, Price = 1});
            p2.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(1), Price = 2});
            p3.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(2), Price = 3});
            p4.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(3), Price = 4});
            p5.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(4), Price = 5});
            p6.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(5), Price = 4});
            p7.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(6), Price = 3});
            p8.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(7), Price = 2});
            p9.AddPriceQuotes(new PriceQuote {SettlementDate = date.AddDays(8), Price = 1});

            PriceSeries series = PriceSeriesFactory.CreatePriceSeries("test");
            series.DataPeriods.Add(p1);
            series.DataPeriods.Add(p2);
            series.DataPeriods.Add(p3);
            series.DataPeriods.Add(p4);
            series.DataPeriods.Add(p5);
            series.DataPeriods.Add(p6);
            series.DataPeriods.Add(p7);
            series.DataPeriods.Add(p8);
            series.DataPeriods.Add(p9);

            // create 4 day moving average
            const int lookback = 4;
            SimpleMovingAverage target = new SimpleMovingAverage(series, lookback);

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
            PriceSeries series = PriceSeriesFactory.CreatePriceSeries("test");
            for (int i = 0; i < count; i++)
            {
                QuotedPricePeriod period = new QuotedPricePeriod();
                period.AddPriceQuotes(new PriceQuote {SettlementDate = startDate.AddDays(i), Price = price});
                series.DataPeriods.Add(period);
            }
            return series;
        }
    }
}
