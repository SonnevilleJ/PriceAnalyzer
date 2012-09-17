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
        /// A DateTime guaranteed to be on the open-side border of a period with resolution <see cref="Resolution"/>.
        /// </summary>
        protected static DateTime PeriodHead
        {
            get { return new DateTime(2012, 1, 1, 0, 0, 0, 0); }
        }

        [TestMethod]
        public virtual void CurrentPeriodOpenTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start;
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void CurrentPeriodOpenTestFromBeforeOpen()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(1 - (long) Resolution);
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void CurrentPeriodOpenTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-1);
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void CurrentPeriodOpenTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1 + (long) Resolution);

            var expected = start.AddTicks(1 - (long) Resolution);
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void CurrentPeriodOpenTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(2 - (long) Resolution);
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void CurrentPeriodOpenTestFromAfterClose()
        {
            var start = PeriodHead.AddTicks(0 + (long) Resolution);

            var expected = start;
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void CurrentPeriodCloseTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1 + (long)Resolution);
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void CurrentPeriodCloseTestFromBeforeOpen()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start;
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void CurrentPeriodCloseTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-2 + (long) Resolution);
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void CurrentPeriodCloseTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start;
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void CurrentPeriodCloseTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(1);
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void CurrentPeriodCloseTestFromAfterClose()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1 + (long) Resolution);
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodOpenTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks((long)Resolution);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodOpenTestFromBeforeOpen()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(1 - (long)Resolution).AddTicks((long)Resolution);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodOpenTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-1).AddTicks((long)Resolution);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodOpenTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1 + (long)Resolution);

            var expected = start.AddTicks(1 - (long)Resolution).AddTicks((long)Resolution);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodOpenTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(2 - (long)Resolution).AddTicks((long)Resolution);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodOpenTestFromAfterClose()
        {
            var start = PeriodHead.AddTicks(0 + (long)Resolution);

            var expected = start.AddTicks((long)Resolution);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodCloseTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1 + (long)Resolution).AddTicks((long)Resolution);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodCloseTestFromBeforeOpen()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks((long)Resolution);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodCloseTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-2 + (long)Resolution).AddTicks((long)Resolution);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodCloseTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks((long)Resolution);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodCloseTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(1).AddTicks((long)Resolution);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodCloseTestFromAfterClose()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1 + (long)Resolution).AddTicks((long)Resolution);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodOpenTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(0 - (long) Resolution);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodOpenTestFromBeforeOpen()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(1).AddTicks(0 - (long)Resolution).AddTicks(0 - (long)Resolution);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodOpenTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-1).AddTicks(0 - (long) Resolution);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodOpenTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(1).AddTicks(0 - (long)Resolution).AddTicks(0 - (long)Resolution);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodOpenTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(2).AddTicks(0 - (long)Resolution).AddTicks(0 - (long)Resolution);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodOpenTestFromAfterClose()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(0 - (long)Resolution);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodCloseTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodCloseTestFromBeforeOpen()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(0 - (long)Resolution);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodCloseTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-2);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodCloseTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(0 - (long)Resolution);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodCloseTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(1).AddTicks(0 - (long)Resolution);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodCloseTestFromAfterClose()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }
    }
}