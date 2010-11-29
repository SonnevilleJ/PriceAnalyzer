using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Sonneville.PriceToolsTest
{
    [TestClass()]
    public class MovingAverageTest
    {
        [TestMethod]
        public void FlatPeriodReturnsSameAverage()
        {
            DateTime head1 = new DateTime(2000, 1, 1);
            PricePeriod p1 = new PricePeriod(head1, head1.AddDays(1), 1, 1, 1, 1);
            PricePeriod p2 = new PricePeriod(head1.AddDays(2), head1.AddDays(2), 2, 2, 2, 2);
            PricePeriod p3 = new PricePeriod(head1.AddDays(3), head1.AddDays(3), 3, 3, 3, 3);
            PricePeriod p4 = new PricePeriod(head1.AddDays(4), head1.AddDays(4), 4, 4, 4, 4);
            PricePeriod p5 = new PricePeriod(head1.AddDays(5), head1.AddDays(5), 5, 5, 5, 5);
            PricePeriod p6 = new PricePeriod(head1.AddDays(6), head1.AddDays(6), 6, 6, 6, 6);
            PricePeriod p7 = new PricePeriod(head1.AddDays(7), head1.AddDays(7), 7, 7, 7, 7);
            PricePeriod p8 = new PricePeriod(head1.AddDays(8), head1.AddDays(8), 8, 8, 8, 8);
            PricePeriod p9 = new PricePeriod(head1.AddDays(9), head1.AddDays(9), 9, 9, 9, 9);
            PricePeriod p10 = new PricePeriod(head1.AddDays(10), head1.AddDays(10), 10, 10, 10, 10);

            PriceSeries series = new PriceSeries(PriceSeriesResolution.Days, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);

            // create 4 day moving average
            MovingAverage avg = new MovingAverage(series, 4, MovingAverageMethod.Simple);
            avg.CalculateAll();
            Assert.IsTrue(avg.Last == 8.5m);
            Assert.IsTrue(avg[0] == 8.5m);
            Assert.IsTrue(avg[-1] == 7.5m);
            Assert.IsTrue(avg[-2] == 6.5m);
            Assert.IsTrue(avg[-3] == 5.5m);
            Assert.IsTrue(avg[-4] == 4.5m);
            Assert.IsTrue(avg[-5] == 3.5m);
            Assert.IsTrue(avg[-6] == 2.5m);
        }
    }
}
