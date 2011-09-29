using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for StaticPricePeriodTest and is intended
    ///to contain all StaticPricePeriodTest Unit Tests
    ///</summary>
    [TestClass]
    public class StaticPricePeriodTest
    {
        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod]
        public void CloseTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal close = 100.00m;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            Assert.AreEqual(close, target.Close);
        }

        /// <summary>
        ///A test for Head
        ///</summary>
        [TestMethod]
        public void HeadTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 100.00m;
            const decimal high = 110.00m;
            const decimal low = 90.00m;
            const decimal close = 100.00m;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close);

            Assert.AreEqual(head, target.Head);
        }

        /// <summary>
        ///A test for Head
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HeadBeforeTailTest()
        {
            DateTime tail = new DateTime(2011, 3, 13);
            DateTime head = tail.AddDays(1);
            const decimal open = 100.00m;
            const decimal high = 110.00m;
            const decimal low = 90.00m;
            const decimal close = 100.00m;

            PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close);
        }

        /// <summary>
        ///A test for High
        ///</summary>
        [TestMethod]
        public void HighTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(high, target.High);
        }

        /// <summary>
        ///A test for High
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HighLessThanOpenTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 9.00m;
            const decimal low = 8.00m;
            const decimal close = 8.00m;
            const long volume = 1000;

            PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        /// <summary>
        ///A test for High
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HighLessThanCloseTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 8.00m;
            const decimal low = 6.00m;
            const decimal close = 9.00m;
            const long volume = 1000;

            PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAtHeadTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(target.Close, (decimal?) target[target.Head]);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAtTailTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(target.Close, (decimal?) target[target.Tail]);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IndexerValueBeforeHeadTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.IsNull(target[target.Head.Subtract(new TimeSpan(1))]);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAfterTailTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            var result = target[target.Tail.Add(new TimeSpan(1))];
        }

        /// <summary>
        ///A test for Low
        ///</summary>
        [TestMethod]
        public void LowTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(low, target.Low);
        }

        /// <summary>
        ///A test for Low
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LowGreaterThanOpenTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 13.00m;
            const decimal low = 11.00m;
            const decimal close = 12.00m;
            const long volume = 1000;

            PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        /// <summary>
        ///A test for Low
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LowGreaterThanCloseTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 8.00m;
            const long volume = 1000;

            PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        /// <summary>
        ///A test for Open
        ///</summary>
        [TestMethod]
        public void OpenTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(open, target.Open);
        }

        /// <summary>
        ///A test for Tail
        ///</summary>
        [TestMethod]
        public void TailTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(tail, target.Tail);
        }

        /// <summary>
        ///A test for Volume
        ///</summary>
        [TestMethod]
        public void VolumeTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        /// <summary>
        ///A test for Resolution
        ///</summary>
        [TestMethod]
        public void ResolutionTestSeconds()
        {
            DateTime head = new DateTime(2011, 9, 28);
            DateTime tail = head.AddSeconds(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Seconds;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Resolution
        ///</summary>
        [TestMethod]
        public void ResolutionTestMinutes()
        {
            DateTime head = new DateTime(2011, 9, 28);
            DateTime tail = head.AddMinutes(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Minutes;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Resolution
        ///</summary>
        [TestMethod]
        public void ResolutionTestHours()
        {
            DateTime head = new DateTime(2011, 9, 28);
            DateTime tail = head.AddHours(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Hours;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Resolution
        ///</summary>
        [TestMethod]
        public void ResolutionTestDays()
        {
            DateTime head = new DateTime(2011, 9, 28);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Days;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Resolution
        ///</summary>
        [TestMethod]
        public void ResolutionTestWeeks()
        {
            DateTime head = new DateTime(2011, 9, 25);
            DateTime tail = head.AddDays(7);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Weeks;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Resolution
        ///</summary>
        [TestMethod]
        public void ResolutionTestMonths()
        {
            DateTime head = new DateTime(2011, 8, 1);
            DateTime tail = head.AddMonths(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Months;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }
    }
}
