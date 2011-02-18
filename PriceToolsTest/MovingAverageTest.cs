using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Sonneville.PriceToolsTest
{
    [TestClass()]
    public class MovingAverageTest
    {
        [TestMethod]
        public void DefaultConstructorAssignsSimpleMethod()
        {
            DateTime date = new DateTime(2011, 1, 6);
            const int value = 2;
            PricePeriod p1 = new PricePeriod(date.AddDays(1), date.AddDays(1), value, value, value, value);
            IPriceSeries series = new PriceSeries(p1);

            MovingAverage ma = new MovingAverage(series, 1);
            Assert.IsTrue(ma.Method == MovingAverageMethod.Simple);
        }

        [TestMethod]
        public void FlatPeriodReturnsSameAverage()
        {
            DateTime date = new DateTime(2011, 1, 6);
            const int value = 2;
            PricePeriod p1 = new PricePeriod(date.AddDays(1), date.AddDays(1), value, value, value, value);
            PricePeriod p2 = new PricePeriod(date.AddDays(2), date.AddDays(2), value, value, value, value);
            PricePeriod p3 = new PricePeriod(date.AddDays(3), date.AddDays(3), value, value, value, value);
            PricePeriod p4 = new PricePeriod(date.AddDays(4), date.AddDays(4), value, value, value, value);
            PricePeriod p5 = new PricePeriod(date.AddDays(5), date.AddDays(5), value, value, value, value);
            PricePeriod p6 = new PricePeriod(date.AddDays(6), date.AddDays(6), value, value, value, value);
            IPriceSeries series = new PriceSeries(p1, p2, p3, p4, p5, p6);

            const int range = 2;
            MovingAverage ma = new MovingAverage(series, range);

            for (int i = range; i < series.Periods.Count; i++)
            {
                Assert.IsTrue(ma[date.AddDays(i)] == value);
            }
        }

        [TestMethod]
        public void RisingAndFallingMovingAverageReturnsCorrectValues()
        {
            DateTime date = new DateTime(2000, 1, 1);
            PricePeriod p1 = new PricePeriod(date, date, 1, 1, 1, 1);
            PricePeriod p2 = new PricePeriod(date.AddDays(1), date.AddDays(1), 2, 2, 2, 2);
            PricePeriod p3 = new PricePeriod(date.AddDays(2), date.AddDays(2), 3, 3, 3, 3);
            PricePeriod p4 = new PricePeriod(date.AddDays(3), date.AddDays(3), 4, 4, 4, 4);
            PricePeriod p5 = new PricePeriod(date.AddDays(4), date.AddDays(4), 5, 5, 5, 5);
            PricePeriod p6 = new PricePeriod(date.AddDays(5), date.AddDays(5), 4, 4, 4, 4);
            PricePeriod p7 = new PricePeriod(date.AddDays(6), date.AddDays(6), 3, 3, 3, 3);
            PricePeriod p8 = new PricePeriod(date.AddDays(7), date.AddDays(7), 2, 2, 2, 2);
            PricePeriod p9 = new PricePeriod(date.AddDays(8), date.AddDays(8), 1, 1, 1, 1);

            IPriceSeries series = new PriceSeries(p1, p2, p3, p4, p5, p6, p7, p8, p9);

            // create 4 day moving average
            const int range = 4;
            int span = series.Periods.Count - (range - 1);
            MovingAverage avg = new MovingAverage(series, range);
            Assert.IsTrue(avg.Range == range);
            avg.CalculateAll();
            Assert.IsTrue(avg.Last == 2.5m);
            Assert.IsTrue(avg[date.AddDays(3)] == 2.5m);
            Assert.IsTrue(avg[date.AddDays(4)] == 3.5m);
            Assert.IsTrue(avg[date.AddDays(5)] == 4.0m);
            Assert.IsTrue(avg[date.AddDays(6)] == 4.0m);
            Assert.IsTrue(avg[date.AddDays(7)] == 3.5m);
            Assert.IsTrue(avg[date.AddDays(8)] == 2.5m);
            Assert.IsTrue(avg.Span == span);
        }
    }
}
