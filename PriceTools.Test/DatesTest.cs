using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;

namespace Test.Sonneville.PriceTools
{
    public abstract class DatesTest
    {
        protected abstract Resolution Resolution { get; }

        /// <summary>
        /// A DateTime guaranteed to be on the open-side border of every period <see cref="Resolution"/>.
        /// </summary>
        private static DateTime PeriodHead
        {
            get { return new DateTime(2012, 1, 1, 0, 0, 0, 0); }
        }

        [TestMethod]
        public void CurrentPeriodOpenTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start;
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CurrentPeriodOpenTestFromBeforeOpen()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(1 - (long) Resolution);
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CurrentPeriodOpenTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-1);
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CurrentPeriodOpenTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1 + (long) Resolution);

            var expected = start.AddTicks(1 - (long) Resolution);
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CurrentPeriodOpenTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(2 - (long) Resolution);
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CurrentPeriodOpenTestFromAfterClose()
        {
            var start = PeriodHead.AddTicks(0 + (long) Resolution);

            var expected = start;
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CurrentPeriodCloseTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1 + (long)Resolution);
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CurrentPeriodCloseTestFromBeforeOpen()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start;
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CurrentPeriodCloseTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-2 + (long) Resolution);
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CurrentPeriodCloseTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start;
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CurrentPeriodCloseTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(1);
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CurrentPeriodCloseTestFromAfterClose()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1 + (long) Resolution);
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }
    }
}