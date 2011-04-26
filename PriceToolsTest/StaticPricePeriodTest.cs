using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sonneville.Utilities;

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

            const decimal expected = close;
            decimal actual = target.Close;
            Assert.AreEqual(expected, actual);
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

            DateTime expected = head;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
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

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close);

            DateTime expected = head;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
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

            const decimal expected = high;
            decimal? actual = target.High;
            Assert.AreEqual(expected, actual);
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

            decimal expected = target.Close;
            decimal? actual = target[head];
            Assert.AreEqual(expected, actual);
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

            decimal expected = target.Close;
            decimal? actual = target[tail];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
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

            Assert.IsNull(target[head.Subtract(new TimeSpan(1))]);
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

            Assert.IsNull(target[tail.Add(new TimeSpan(1))]);
        }

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod]
        public void LastTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal close = 100.00m;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            decimal expected = target.Close;
            decimal actual = target.Last;
            Assert.AreEqual(expected, actual);
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

            const decimal expected = low;
            decimal? actual = target.Low;
            Assert.AreEqual(expected, actual);
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

            const decimal expected = open;
            decimal? actual = target.Open;
            Assert.AreEqual(expected, actual);
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

            DateTime expected = tail;
            DateTime actual = target.Tail;
            Assert.AreEqual(expected, actual);
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

            const long expected = volume;
            long? actual = target.Volume;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializeStaticPricePeriodTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            IPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            StaticPricePeriod actual = ((StaticPricePeriod)TestUtilities.Serialize(target));
            Assert.AreEqual(target, actual);
        }
    }
}
