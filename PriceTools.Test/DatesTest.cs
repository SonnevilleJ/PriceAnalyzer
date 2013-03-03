using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
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

        #region CurrentPeriod Tests

        [TestMethod]
        public void CurrentPeriodOpenTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start;
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
        public virtual void CurrentPeriodCloseTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1 + (long) Resolution);
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

        #endregion

        #region NextPeriod Tests

        [TestMethod]
        public virtual void NextPeriodOpenTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks((long) Resolution);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodOpenTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-1).AddTicks((long) Resolution);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NextPeriodOpenTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(1);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NextPeriodOpenTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(2);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodCloseTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1 + (long) Resolution).AddTicks((long) Resolution);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodCloseTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-2 + (long) Resolution).AddTicks((long) Resolution);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodCloseTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks((long) Resolution);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void NextPeriodCloseTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(1).AddTicks((long) Resolution);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region PreviousPeriod Tests

        [TestMethod]
        public virtual void PreviousPeriodOpenTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(0 - (long) Resolution);
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

            var expected = start.AddTicks(1).AddTicks(0 - (long) Resolution).AddTicks(0 - (long) Resolution);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodOpenTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(2).AddTicks(0 - (long) Resolution).AddTicks(0 - (long) Resolution);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PreviousPeriodCloseTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PreviousPeriodCloseTestFromAfterOpen()
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

            var expected = start.AddTicks(0 - (long) Resolution);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void PreviousPeriodCloseTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(1).AddTicks(0 - (long) Resolution);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region NextTradingPeriod Tests

        [TestMethod]
        public void NextTradingPeriodOpenTestFromOpenIsFuture()
        {
            var target = PeriodHead;

            var result = target.NextTradingPeriodOpen(Resolution);
            Assert.IsTrue(result > target);
        }

        [TestMethod]
        public void NextTradingPeriodOpenTestFromOpenIsOpen()
        {
            var target = PeriodHead;

            var result = target.NextTradingPeriodOpen(Resolution);
            Assert.AreEqual(result.CurrentPeriodOpen(Resolution), result);
        }

        [TestMethod]
        public void NextTradingPeriodOpenTestFromOpenIsInTradingPeriod()
        {
            var target = PeriodHead;

            var result = target.NextTradingPeriodOpen(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void NextTradingPeriodOpenTestFromAfterOpenIsFuture()
        {
            var target = PeriodHead.AddTicks(1);

            var result = target.NextTradingPeriodOpen(Resolution);
            Assert.IsTrue(result > target);
        }

        [TestMethod]
        public void NextTradingPeriodOpenTestFromAfterOpenIsOpen()
        {
            var target = PeriodHead.AddTicks(1);

            var result = target.NextTradingPeriodOpen(Resolution);
            Assert.AreEqual(result.CurrentPeriodOpen(Resolution), result);
        }

        [TestMethod]
        public void NextTradingPeriodOpenTestFromAfterOpenIsInTradingPeriod()
        {
            var target = PeriodHead.AddTicks(1);

            var result = target.NextTradingPeriodOpen(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void NextTradingPeriodOpenTestFromCloseIsFuture()
        {
            var target = PeriodHead.AddTicks(-1);

            var result = target.NextTradingPeriodOpen(Resolution);
            Assert.IsTrue(result > target);
        }

        [TestMethod]
        public void NextTradingPeriodOpenTestFromCloseIsOpen()
        {
            var target = PeriodHead.AddTicks(-1);

            var result = target.NextTradingPeriodOpen(Resolution);
            Assert.AreEqual(result.CurrentPeriodOpen(Resolution), result);
        }

        [TestMethod]
        public void NextTradingPeriodOpenTestFromCloseIsInTradingPeriod()
        {
            var target = PeriodHead.AddTicks(-1);

            var result = target.NextTradingPeriodOpen(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void NextTradingPeriodOpenTestFromBeforeCloseIsFuture()
        {
            var target = PeriodHead.AddTicks(-2);

            var result = target.NextTradingPeriodOpen(Resolution);
            Assert.IsTrue(result > target);
        }

        [TestMethod]
        public void NextTradingPeriodOpenTestFromBeforeCloseIsOpen()
        {
            var target = PeriodHead.AddTicks(-2);

            var result = target.NextTradingPeriodOpen(Resolution);
            Assert.AreEqual(result.CurrentPeriodOpen(Resolution), result);
        }

        [TestMethod]
        public void NextTradingPeriodOpenTestFromBeforeCloseIsInTradingPeriod()
        {
            var target = PeriodHead.AddTicks(-2);

            var result = target.NextTradingPeriodOpen(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void NextTradingPeriodCloseTestFromOpenIsFuture()
        {
            var target = PeriodHead;

            var result = target.NextTradingPeriodClose(Resolution);
            Assert.IsTrue(result > target);
        }

        [TestMethod]
        public void NextTradingPeriodCloseTestFromOpenIsOpen()
        {
            var target = PeriodHead;

            var result = target.NextTradingPeriodClose(Resolution);
            Assert.AreEqual(result.CurrentPeriodClose(Resolution), result);
        }

        [TestMethod]
        public void NextTradingPeriodCloseTestFromOpenIsInTradingPeriod()
        {
            var target = PeriodHead;

            var result = target.NextTradingPeriodClose(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void NextTradingPeriodCloseTestFromAfterOpenIsFuture()
        {
            var target = PeriodHead.AddTicks(1);

            var result = target.NextTradingPeriodClose(Resolution);
            Assert.IsTrue(result > target);
        }

        [TestMethod]
        public void NextTradingPeriodCloseTestFromAfterOpenIsOpen()
        {
            var target = PeriodHead.AddTicks(1);

            var result = target.NextTradingPeriodClose(Resolution);
            Assert.AreEqual(result.CurrentPeriodClose(Resolution), result);
        }

        [TestMethod]
        public void NextTradingPeriodCloseTestFromAfterOpenIsInTradingPeriod()
        {
            var target = PeriodHead.AddTicks(1);

            var result = target.NextTradingPeriodClose(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void NextTradingPeriodCloseTestFromCloseIsFuture()
        {
            var target = PeriodHead.AddTicks(-1);

            var result = target.NextTradingPeriodClose(Resolution);
            Assert.IsTrue(result > target);
        }

        [TestMethod]
        public void NextTradingPeriodCloseTestFromCloseIsOpen()
        {
            var target = PeriodHead.AddTicks(-1);

            var result = target.NextTradingPeriodClose(Resolution);
            Assert.AreEqual(result.CurrentPeriodClose(Resolution), result);
        }

        [TestMethod]
        public void NextTradingPeriodCloseTestFromCloseIsInTradingPeriod()
        {
            var target = PeriodHead.AddTicks(-1);

            var result = target.NextTradingPeriodClose(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void NextTradingPeriodCloseTestFromBeforeCloseIsFuture()
        {
            var target = PeriodHead.AddTicks(-2);

            var result = target.NextTradingPeriodClose(Resolution);
            Assert.IsTrue(result > target);
        }

        [TestMethod]
        public void NextTradingPeriodCloseTestFromBeforeCloseIsOpen()
        {
            var target = PeriodHead.AddTicks(-2);

            var result = target.NextTradingPeriodClose(Resolution);
            Assert.AreEqual(result.CurrentPeriodClose(Resolution), result);
        }

        [TestMethod]
        public void NextTradingPeriodCloseTestFromBeforeCloseIsInTradingPeriod()
        {
            var target = PeriodHead.AddTicks(-2);

            var result = target.NextTradingPeriodClose(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        #endregion

        #region PreviousTradingPeriod Tests

        [TestMethod]
        public void PreviousTradingPeriodOpenTestFromOpenIsPast()
        {
            var target = PeriodHead;

            var result = target.PreviousTradingPeriodOpen(Resolution);
            Assert.IsTrue(result < target);
        }

        [TestMethod]
        public void PreviousTradingPeriodOpenTestFromOpenIsOpen()
        {
            var target = PeriodHead;

            var result = target.PreviousTradingPeriodOpen(Resolution);
            Assert.AreEqual(result.CurrentPeriodOpen(Resolution), result);
        }

        [TestMethod]
        public void PreviousTradingPeriodOpenTestFromOpenIsInTradingPeriod()
        {
            var target = PeriodHead;

            var result = target.PreviousTradingPeriodOpen(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void PreviousTradingPeriodOpenTestFromAfterOpenIsPast()
        {
            var target = PeriodHead.AddTicks(1);

            var result = target.PreviousTradingPeriodOpen(Resolution);
            Assert.IsTrue(result < target);
        }

        [TestMethod]
        public void PreviousTradingPeriodOpenTestFromAfterOpenIsOpen()
        {
            var target = PeriodHead.AddTicks(1);

            var result = target.PreviousTradingPeriodOpen(Resolution);
            Assert.AreEqual(result.CurrentPeriodOpen(Resolution), result);
        }

        [TestMethod]
        public void PreviousTradingPeriodOpenTestFromAfterOpenIsInTradingPeriod()
        {
            var target = PeriodHead.AddTicks(1);

            var result = target.PreviousTradingPeriodOpen(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void PreviousTradingPeriodOpenTestFromCloseIsPast()
        {
            var target = PeriodHead.AddTicks(-1);

            var result = target.PreviousTradingPeriodOpen(Resolution);
            Assert.IsTrue(result < target);
        }

        [TestMethod]
        public void PreviousTradingPeriodOpenTestFromCloseIsOpen()
        {
            var target = PeriodHead.AddTicks(-1);

            var result = target.PreviousTradingPeriodOpen(Resolution);
            Assert.AreEqual(result.CurrentPeriodOpen(Resolution), result);
        }

        [TestMethod]
        public void PreviousTradingPeriodOpenTestFromCloseIsInTradingPeriod()
        {
            var target = PeriodHead.AddTicks(-1);

            var result = target.PreviousTradingPeriodOpen(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void PreviousTradingPeriodOpenTestFromBeforeCloseIsPast()
        {
            var target = PeriodHead.AddTicks(-2);

            var result = target.PreviousTradingPeriodOpen(Resolution);
            Assert.IsTrue(result < target);
        }

        [TestMethod]
        public void PreviousTradingPeriodOpenTestFromBeforeCloseIsOpen()
        {
            var target = PeriodHead.AddTicks(-2);

            var result = target.PreviousTradingPeriodOpen(Resolution);
            Assert.AreEqual(result.CurrentPeriodOpen(Resolution), result);
        }

        [TestMethod]
        public void PreviousTradingPeriodOpenTestFromBeforeCloseIsInTradingPeriod()
        {
            var target = PeriodHead.AddTicks(-2);

            var result = target.PreviousTradingPeriodOpen(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void PreviousTradingPeriodCloseTestFromOpenIsPast()
        {
            var target = PeriodHead;

            var result = target.PreviousTradingPeriodClose(Resolution);
            Assert.IsTrue(result < target);
        }

        [TestMethod]
        public void PreviousTradingPeriodCloseTestFromOpenIsOpen()
        {
            var target = PeriodHead;

            var result = target.PreviousTradingPeriodClose(Resolution);
            Assert.AreEqual(result.CurrentPeriodClose(Resolution), result);
        }

        [TestMethod]
        public void PreviousTradingPeriodCloseTestFromOpenIsInTradingPeriod()
        {
            var target = PeriodHead;

            var result = target.PreviousTradingPeriodClose(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void PreviousTradingPeriodCloseTestFromAfterOpenIsPast()
        {
            var target = PeriodHead.AddTicks(1);

            var result = target.PreviousTradingPeriodClose(Resolution);
            Assert.IsTrue(result < target);
        }

        [TestMethod]
        public void PreviousTradingPeriodCloseTestFromAfterOpenIsOpen()
        {
            var target = PeriodHead.AddTicks(1);

            var result = target.PreviousTradingPeriodClose(Resolution);
            Assert.AreEqual(result.CurrentPeriodClose(Resolution), result);
        }

        [TestMethod]
        public void PreviousTradingPeriodCloseTestFromAfterOpenIsInTradingPeriod()
        {
            var target = PeriodHead.AddTicks(1);

            var result = target.PreviousTradingPeriodClose(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void PreviousTradingPeriodCloseTestFromCloseIsPast()
        {
            var target = PeriodHead.AddTicks(-1);

            var result = target.PreviousTradingPeriodClose(Resolution);
            Assert.IsTrue(result < target);
        }

        [TestMethod]
        public void PreviousTradingPeriodCloseTestFromCloseIsOpen()
        {
            var target = PeriodHead.AddTicks(-1);

            var result = target.PreviousTradingPeriodClose(Resolution);
            Assert.AreEqual(result.CurrentPeriodClose(Resolution), result);
        }

        [TestMethod]
        public void PreviousTradingPeriodCloseTestFromCloseIsInTradingPeriod()
        {
            var target = PeriodHead.AddTicks(-1);

            var result = target.PreviousTradingPeriodClose(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void PreviousTradingPeriodCloseTestFromBeforeCloseIsPast()
        {
            var target = PeriodHead.AddTicks(-2);

            var result = target.PreviousTradingPeriodClose(Resolution);
            Assert.IsTrue(result < target);
        }

        [TestMethod]
        public void PreviousTradingPeriodCloseTestFromBeforeCloseIsOpen()
        {
            var target = PeriodHead.AddTicks(-2);

            var result = target.PreviousTradingPeriodClose(Resolution);
            Assert.AreEqual(result.CurrentPeriodClose(Resolution), result);
        }

        [TestMethod]
        public void PreviousTradingPeriodCloseTestFromBeforeCloseIsInTradingPeriod()
        {
            var target = PeriodHead.AddTicks(-2);

            var result = target.PreviousTradingPeriodClose(Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        #endregion

        #region SeekPeriods Tests

        [TestMethod]
        public virtual void SeekForwardOneTest()
        {
            var start = new DateTime(2012, 10, 15);

            var expected = start.AddTicks((long) Resolution);
            var actual = start.SeekPeriods(1, Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void SeekBackwardOneTest()
        {
            var start = new DateTime(2012, 10, 15);

            var expected = start.AddTicks(0 - (long) Resolution);
            var actual = start.SeekPeriods(-1, Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void SeekForwardTenTest()
        {
            var start = new DateTime(2012, 10, 15);

            var expected = start.AddTicks((long) Resolution*10);
            var actual = start.SeekPeriods(10, Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void SeekBackwardTenTest()
        {
            var start = new DateTime(2012, 10, 15);

            var expected = start.AddTicks(0 - ((long) Resolution*10));
            var actual = start.SeekPeriods(-10, Resolution);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region SeekTradingPeriods Tests

        [TestMethod]
        public void SeekTradingForwardOneTest()
        {
            var start = new DateTime(2012, 10, 15);

            var expected = start.SeekPeriods(1, Resolution);
            var actual = start.SeekTradingPeriods(1, Resolution);
            Assert.IsTrue(actual >= expected);
        }

        [TestMethod]
        public void SeekTradingForwardOneIsInTradingPeriodTest()
        {
            var start = new DateTime(2012, 10, 15);

            var result = start.SeekTradingPeriods(1, Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void SeekTradingBackwardOneTest()
        {
            var start = new DateTime(2012, 10, 15);

            var expected = start.SeekPeriods(-1, Resolution);
            var actual = start.SeekTradingPeriods(-1, Resolution);
            Assert.IsTrue(actual <= expected);
        }

        [TestMethod]
        public void SeekTradingBackwardOneIsInTradingPeriodTest()
        {
            var start = new DateTime(2012, 10, 15);

            var result = start.SeekTradingPeriods(-1, Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void SeekTradingForwardTenTest()
        {
            var start = new DateTime(2012, 10, 15);

            var expected = start.SeekPeriods(10, Resolution);
            var actual = start.SeekTradingPeriods(10, Resolution);
            Assert.IsTrue(actual >= expected);
        }

        [TestMethod]
        public void SeekTradingForwardTenIsInTradingPeriodTest()
        {
            var start = new DateTime(2012, 10, 15);

            var result = start.SeekTradingPeriods(10, Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        [TestMethod]
        public void SeekTradingBackwardTenTest()
        {
            var start = new DateTime(2012, 10, 15);

            var expected = start.SeekPeriods(-10, Resolution);
            var actual = start.SeekTradingPeriods(-10, Resolution);
            Assert.IsTrue(actual <= expected);
        }

        [TestMethod]
        public void SeekTradingBackwardTenIsInTradingPeriodTest()
        {
            var start = new DateTime(2012, 10, 15);

            var result = start.SeekTradingPeriods(-10, Resolution);
            Assert.IsTrue(result.IsInTradingPeriod(Resolution));
        }

        #endregion
    }
}