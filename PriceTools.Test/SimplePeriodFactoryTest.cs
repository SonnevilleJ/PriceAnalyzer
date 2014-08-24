using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class SimplePeriodFactoryTest
    {
        private readonly ITimePeriodFactory<decimal> _timePeriodFactory;

        public SimplePeriodFactoryTest()
        {
            _timePeriodFactory = new TimePeriodFactory<decimal>();
        }

        [Test]
        public void TestHead()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.AreEqual(head, target.Head);
        }

        [Test]
        public void TestTail()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.AreEqual(tail, target.Tail);
        }

        [Test]
        public void TestValueOfHead()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.AreEqual(value, target[head]);
        }

        [Test]
        public void TestValueOfHeadPlusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.AreEqual(value, target[head.AddTicks(1)]);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestValueOfHeadMinusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            var actual = target[head.AddTicks(-1)];

            Assert.IsNull(actual);
        }

        [Test]
        public void TestValueOfTail()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.AreEqual(value, target[tail]);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestValueOfTailPlusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            var actual = target[tail.AddTicks(1)];

            Assert.IsNull(actual);
        }

        [Test]
        public void TestValueOfTailMinusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.AreEqual(value, target[tail.AddTicks(-1)]);
        }

        [Test]
        public void TestHasValueOfHead()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.IsTrue(target.HasValueInRange(head));
        }

        [Test]
        public void TestHasValueOfHeadPlusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.IsTrue(target.HasValueInRange(head.AddTicks(1)));
        }

        [Test]
        public void TestHasValueOfHeadMinusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.IsFalse(target.HasValueInRange(head.AddTicks(-1)));
        }

        [Test]
        public void TestHasValueOfTail()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.IsTrue(target.HasValueInRange(tail));
        }

        [Test]
        public void TestHasValueOfTailPlusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.IsFalse(target.HasValueInRange(tail.AddTicks(1)));
        }

        [Test]
        public void TestHasValueOfTailMinusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.IsTrue(target.HasValueInRange(tail.AddTicks(-1)));
        }

        [Test]
        public void TestResolution()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            var expected = (Resolution) ((tail - head).Ticks);
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }
    }
}
