using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class SimplePeriodFactoryTest
    {
        private readonly ITimePeriodFactory _timePeriodFactory;

        public SimplePeriodFactoryTest()
        {
            _timePeriodFactory = new TimePeriodFactory();
        }

        [TestMethod]
        public void TestHead()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.AreEqual(head, target.Head);
        }

        [TestMethod]
        public void TestTail()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.AreEqual(tail, target.Tail);
        }

        [TestMethod]
        public void TestValueOfHead()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.AreEqual(value, target[head]);
        }

        [TestMethod]
        public void TestValueOfHeadPlusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.AreEqual(value, target[head.AddTicks(1)]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestValueOfHeadMinusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            var actual = target[head.AddTicks(-1)];
        }

        [TestMethod]
        public void TestValueOfTail()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.AreEqual(value, target[tail]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestValueOfTailPlusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            var actual = target[tail.AddTicks(1)];
        }

        [TestMethod]
        public void TestValueOfTailMinusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.AreEqual(value, target[tail.AddTicks(-1)]);
        }

        [TestMethod]
        public void TestHasValueOfHead()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.IsTrue(target.HasValueInRange(head));
        }

        [TestMethod]
        public void TestHasValueOfHeadPlusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.IsTrue(target.HasValueInRange(head.AddTicks(1)));
        }

        [TestMethod]
        public void TestHasValueOfHeadMinusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.IsFalse(target.HasValueInRange(head.AddTicks(-1)));
        }

        [TestMethod]
        public void TestHasValueOfTail()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.IsTrue(target.HasValueInRange(tail));
        }

        [TestMethod]
        public void TestHasValueOfTailPlusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.IsFalse(target.HasValueInRange(tail.AddTicks(1)));
        }

        [TestMethod]
        public void TestHasValueOfTailMinusOne()
        {
            var head = new DateTime(2012, 9, 13);
            var tail = new DateTime(2012, 9, 14);
            const decimal value = 5m;

            var target = _timePeriodFactory.ConstructTimePeriod(head, tail, value);

            Assert.IsTrue(target.HasValueInRange(tail.AddTicks(-1)));
        }

        [TestMethod]
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
