using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class SimpleMovingAverageTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Settings.SetDefaultSettings();
            Settings.CanConnectToInternet = false;
        }

        [TestMethod]
        public void ResolutionDaysByDefault()
        {
            DateTime date = new DateTime(2011, 3, 1);
            IPriceSeries priceSeries = CreateTestPriceSeries(20, date, 1);

            const int range = 5;
            SimpleMovingAverage target = new SimpleMovingAverage(priceSeries, range);

            const PriceSeriesResolution expected = PriceSeriesResolution.Days;
            PriceSeriesResolution actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadTest()
        {
            DateTime date = new DateTime(2011, 3, 1);
            IPriceSeries priceSeries = CreateTestPriceSeries(10, date, 1);
            const int range = 4;

            SimpleMovingAverage target = new SimpleMovingAverage(priceSeries, range);

            DateTime expected = date.AddDays(3);
            DateTime actual = target.Head;
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
        public void FlatPeriodReturnsSameAverage()
        {
            DateTime date = new DateTime(2011, 3, 1);
            const int price = 2;

            IPriceSeries series = CreateTestPriceSeries(10, date, price);

            const int range = 2;
            SimpleMovingAverage ma = new SimpleMovingAverage(series, range);

            const decimal expected = price;
            for (int i = range; i < series.PricePeriods.Count; i++)
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
            p1.AddPriceQuote(new PriceQuote {SettlementDate = date, Price = 1});
            p2.AddPriceQuote(new PriceQuote {SettlementDate = date.AddDays(1), Price = 2});
            p3.AddPriceQuote(new PriceQuote {SettlementDate = date.AddDays(2), Price = 3});
            p4.AddPriceQuote(new PriceQuote {SettlementDate = date.AddDays(3), Price = 4});
            p5.AddPriceQuote(new PriceQuote {SettlementDate = date.AddDays(4), Price = 5});
            p6.AddPriceQuote(new PriceQuote {SettlementDate = date.AddDays(5), Price = 4});
            p7.AddPriceQuote(new PriceQuote {SettlementDate = date.AddDays(6), Price = 3});
            p8.AddPriceQuote(new PriceQuote {SettlementDate = date.AddDays(7), Price = 2});
            p9.AddPriceQuote(new PriceQuote {SettlementDate = date.AddDays(8), Price = 1});

            IPriceSeries series = PriceSeriesFactory.CreatePriceSeries("test");
            series.PricePeriods.Add(p1);
            series.PricePeriods.Add(p2);
            series.PricePeriods.Add(p3);
            series.PricePeriods.Add(p4);
            series.PricePeriods.Add(p5);
            series.PricePeriods.Add(p6);
            series.PricePeriods.Add(p7);
            series.PricePeriods.Add(p8);
            series.PricePeriods.Add(p9);

            // create 4 day moving average
            const int range = 4;
            SimpleMovingAverage target = new SimpleMovingAverage(series, range);

            target.CalculateAll();
            Assert.AreEqual(2.5m, target[date.AddDays(3)]);
            Assert.AreEqual(3.5m, target[date.AddDays(4)]);
            Assert.AreEqual(4.0m, target[date.AddDays(5)]);
            Assert.AreEqual(4.0m, target[date.AddDays(6)]);
            Assert.AreEqual(3.5m, target[date.AddDays(7)]);
            Assert.AreEqual(2.5m, target[date.AddDays(8)]);
        }

        [TestMethod]
        public void SpanTest()
        {
            IPriceSeries series = CreateTestPriceSeries(90, new DateTime(2011, 3, 1), 100);
            const int range = 30;

            SimpleMovingAverage target = new SimpleMovingAverage(series, range);

            int expected = series.PricePeriods.Count - (range - 1);
            int actual = target.Span;
            Assert.AreEqual(expected, actual);
        }

        private static IPriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price)
        {
            IPriceSeries series = PriceSeriesFactory.CreatePriceSeries("test");
            for (int i = 0; i < count; i++)
            {
                QuotedPricePeriod period = new QuotedPricePeriod();
                period.AddPriceQuote(new PriceQuote {SettlementDate = startDate.AddDays(i), Price = price});
                series.PricePeriods.Add(period);
            }
            return series;
        }
    }
}
