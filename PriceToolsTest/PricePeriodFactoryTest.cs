using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class PricePeriodFactoryTest
    {
        [TestMethod]
        public void CreateStaticPricePeriodHeadAndWeeklyResolution()
        {
            DateTime head = new DateTime(2011, 7, 4);
            DateTime tail = new DateTime(2011, 7, 8, 23, 59, 59, 999);
            const decimal open = 50.00m;
            const decimal high = 65.00m;
            const decimal low = 45.00m;
            const decimal close = 60.00m;
            const long volume = 100;

            var target = PricePeriodFactory.CreateStaticPricePeriod(head, Resolution.Weeks, open, high, low, close, volume);

            Assert.AreEqual(head, target.Head);
            Assert.AreEqual(tail, target.Tail);
            Assert.AreEqual(open, target.Open);
            Assert.AreEqual(high, target.High);
            Assert.AreEqual(low, target.Low);
            Assert.AreEqual(close, target.Close);
            Assert.AreEqual(volume, target.Volume);
        }

        /// <summary>
        ///A test for CreateStaticPricePeriod
        ///</summary>
        [TestMethod]
        public void CreateStaticPricePeriodTest1()
        {
            DateTime head = new DateTime(2011, 7, 4);
            DateTime tail = new DateTime(2011, 7, 4, 23, 59, 59, 999);
            const decimal open = 50.00m;
            const decimal high = 65.00m;
            const decimal low = 45.00m;
            const decimal close = 60.00m;
            const long volume = 100;

            var target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(head, target.Head);
            Assert.AreEqual(tail, target.Tail);
            Assert.AreEqual(open, target.Open);
            Assert.AreEqual(high, target.High);
            Assert.AreEqual(low, target.Low);
            Assert.AreEqual(close, target.Close);
            Assert.AreEqual(volume, target.Volume);
        }

        /// <summary>
        ///A test for CreateStaticPricePeriod
        ///</summary>
        [TestMethod]
        public void CreateStaticPricePeriodTest2()
        {
            DateTime head = new DateTime(2011, 7, 4);
            DateTime tail = new DateTime(2011, 7, 4, 23, 59, 59, 999);
            const decimal close = 60.00m;

            var target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, null, null, null, close);

            Assert.AreEqual(head, target.Head);
            Assert.AreEqual(tail, target.Tail);
            Assert.AreEqual(close, target.Open);
            Assert.AreEqual(close, target.High);
            Assert.AreEqual(close, target.Low);
            Assert.AreEqual(close, target.Close);
            Assert.AreEqual(null, target.Volume);
        }
    }
}
