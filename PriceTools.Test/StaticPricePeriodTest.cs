using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    /// <summary>
    ///This is a test class for StaticPricePeriodTest and is intended
    ///to contain all StaticPricePeriodTest Unit Tests
    ///</summary>
    [TestClass]
    public class StaticPricePeriodTest
    {
        private readonly IPricePeriodFactory _pricePeriodFactory;

        public StaticPricePeriodTest()
        {
            _pricePeriodFactory = new PricePeriodFactory();
        }

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod]
        public void CloseTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, close);

            Assert.AreEqual(close, target.Close);
        }

        /// <summary>
        ///A test for Head
        ///</summary>
        [TestMethod]
        public void HeadTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 100.00m;
            const decimal high = 110.00m;
            const decimal low = 90.00m;
            const decimal close = 100.00m;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close);

            Assert.AreEqual(head, target.Head);
        }

        /// <summary>
        ///A test for Head
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HeadBeforeTailTest()
        {
            var tail = new DateTime(2011, 3, 13);
            var head = tail.AddDays(1);
            const decimal open = 100.00m;
            const decimal high = 110.00m;
            const decimal low = 90.00m;
            const decimal close = 100.00m;

            _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close);
        }

        /// <summary>
        ///A test for High
        ///</summary>
        [TestMethod]
        public void HighTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(high, target.High);
        }

        /// <summary>
        ///A test for High
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HighLessThanOpenTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 9.00m;
            const decimal low = 8.00m;
            const decimal close = 8.00m;
            const long volume = 1000;

            _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        /// <summary>
        ///A test for High
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HighLessThanCloseTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 7.00m;
            const decimal high = 8.00m;
            const decimal low = 6.00m;
            const decimal close = 9.00m;
            const long volume = 1000;

            _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAtHeadTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(target.Close, (decimal?) target[target.Head]);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAtTailTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(target.Close, (decimal?) target[target.Tail]);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IndexerValueBeforeHeadTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.IsNull(target[target.Head.Subtract(new TimeSpan(1))]);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAfterTailTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            var result = target[target.Tail.Add(new TimeSpan(1))];
        }

        /// <summary>
        ///A test for Low
        ///</summary>
        [TestMethod]
        public void LowTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(low, target.Low);
        }

        /// <summary>
        ///A test for Low
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LowGreaterThanOpenTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 13.00m;
            const decimal low = 11.00m;
            const decimal close = 12.00m;
            const long volume = 1000;

            _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        /// <summary>
        ///A test for Low
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LowGreaterThanCloseTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 8.00m;
            const long volume = 1000;

            _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        /// <summary>
        ///A test for Open
        ///</summary>
        [TestMethod]
        public void OpenTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(open, target.Open);
        }

        /// <summary>
        ///A test for Tail
        ///</summary>
        [TestMethod]
        public void TailTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(tail, target.Tail);
        }

        /// <summary>
        ///A test for Volume
        ///</summary>
        [TestMethod]
        public void VolumeTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        /// <summary>
        ///A test for Resolution
        ///</summary>
        [TestMethod]
        public void ResolutionTestSeconds()
        {
            var head = new DateTime(2011, 9, 28);
            var tail = head.AddSeconds(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

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
            var head = new DateTime(2011, 9, 28);
            var tail = head.AddMinutes(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

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
            var head = new DateTime(2011, 9, 28);
            var tail = head.AddHours(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

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
            var head = new DateTime(2011, 9, 28);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

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
            var head = new DateTime(2011, 9, 25);
            var tail = head.AddDays(7);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

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
            var head = new DateTime(2011, 8, 1);
            var tail = head.AddMonths(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Months;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }
    }
}
