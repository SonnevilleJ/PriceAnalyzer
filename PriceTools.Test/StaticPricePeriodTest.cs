using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class StaticPricePeriodTest
    {
        [Test]
        public void CloseTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var target = new PricePeriod(head, tail, close);

            Assert.AreEqual(close, target.Close);
        }

        [Test]
        public void HeadTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 100.00m;
            const decimal high = 110.00m;
            const decimal low = 90.00m;
            const decimal close = 100.00m;

            var target = new PricePeriod(head, tail, open, high, low, close, null);

            Assert.AreEqual(head, target.Head);
        }

        [Test]
        public void HighTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(high, target.High);
        }

        [Test]
        public void IndexerValueAtHeadTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(target.Close, (decimal?) target[target.Head]);
        }

        [Test]
        public void IndexerValueAtTailTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(target.Close, (decimal?) target[target.Tail]);
        }

        [Test]
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

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.IsNull(target[target.Head.Subtract(new TimeSpan(1))]);
        }

        [Test]
        public void IndexerValueAfterTailTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            var result = target[target.Tail.Add(new TimeSpan(1))];

            Assert.IsNotNull(result);
        }

        [Test]
        public void LowTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(low, target.Low);
        }

        [Test]
        public void OpenTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(open, target.Open);
        }

        [Test]
        public void TailTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(tail, target.Tail);
        }

        [Test]
        public void VolumeTest()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            Assert.AreEqual(volume, target.Volume);
        }

        [Test]
        public void ResolutionTestSeconds()
        {
            var head = new DateTime(2011, 9, 28);
            var tail = head.AddSeconds(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Seconds;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolutionTestMinutes()
        {
            var head = new DateTime(2011, 9, 28);
            var tail = head.AddMinutes(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Minutes;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolutionTestHours()
        {
            var head = new DateTime(2011, 9, 28);
            var tail = head.AddHours(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Hours;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolutionTestDays()
        {
            var head = new DateTime(2011, 9, 28);
            var tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Days;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolutionTestWeeks()
        {
            var head = new DateTime(2011, 9, 25);
            var tail = head.AddDays(7);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Weeks;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolutionTestMonths()
        {
            var head = new DateTime(2011, 8, 1);
            var tail = head.AddMonths(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            var target = new PricePeriod(head, tail, open, high, low, close, volume);

            const Resolution expected = Resolution.Months;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }
    }
}
