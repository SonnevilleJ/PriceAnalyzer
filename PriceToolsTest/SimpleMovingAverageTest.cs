using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Sonneville.PriceToolsTest
{
    [TestClass()]
    public class SimpleMovingAverageTest
    {
        [TestMethod]
        public void ResolutionDaysByDefault()
        {
            IPriceSeries priceSeries = new PriceSeries();
            DateTime date = new DateTime(2011, 3, 1);
            for (int i = 0; i < 20; i++)
            {
                IPriceQuote quote = new PriceQuote {SettlementDate = date.AddDays(i), Price = 1};
                priceSeries.AddPriceQuote(quote);
            }

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
            for (int i = range; i < series.PriceQuotes.Count; i++)
            {
                decimal actual = ma[date.AddDays(i)];
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void RisingAndFallingMovingAverageReturnsCorrectValues()
        {
            DateTime date = new DateTime(2000, 1, 1);
            PriceQuote p1 = new PriceQuote {SettlementDate = date, Price = 1};
            PriceQuote p2 = new PriceQuote {SettlementDate = date.AddDays(1), Price = 2 };
            PriceQuote p3 = new PriceQuote { SettlementDate = date.AddDays(2), Price = 3 };
            PriceQuote p4 = new PriceQuote { SettlementDate = date.AddDays(3), Price = 4 };
            PriceQuote p5 = new PriceQuote { SettlementDate = date.AddDays(4), Price = 5 };
            PriceQuote p6 = new PriceQuote { SettlementDate = date.AddDays(5), Price = 4 };
            PriceQuote p7 = new PriceQuote { SettlementDate = date.AddDays(6), Price = 3 };
            PriceQuote p8 = new PriceQuote { SettlementDate = date.AddDays(7), Price = 2 };
            PriceQuote p9 = new PriceQuote { SettlementDate = date.AddDays(8), Price = 1 };

            IPriceSeries series = new PriceSeries();
            series.AddPriceQuote(p1, p2, p3, p4, p5, p6, p7, p8, p9);

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

            int expected = series.PriceQuotes.Count - (range - 1);
            int actual = target.Span;
            Assert.AreEqual(expected, actual);
        }

        private static IPriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price)
        {
            IPriceSeries series = new PriceSeries();
            for (int i = 0; i < count; i++)
            {
                series.AddPriceQuote(new PriceQuote {SettlementDate = startDate.AddDays(i), Price = price});
            }
            return series;
        }
    }
}
