using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class DatesTest
    {
        public DatesTest()
        {
            _periodOpen = date => date.CurrentPeriodOpen(Resolution);
            _periodClose = date => date.CurrentPeriodClose(Resolution);
            _forwardShiftToOpen = date => date.NextPeriodOpen(Resolution);
            _backwardShiftToOpen = date => date.PreviousPeriodOpen(Resolution);
            _forwardShiftToClose = date => date.NextPeriodClose(Resolution);
            _backwardShiftToClose = date => date.PreviousPeriodClose(Resolution);
        }

        #region Private Members

        private readonly DateShifter _forwardShiftToOpen;
        private readonly DateShifter _backwardShiftToOpen;
        private readonly DateShifter _forwardShiftToClose;
        private readonly DateShifter _backwardShiftToClose;
        private readonly DateTime _periodHead = new DateTime(2012, 1, 1, 0, 0, 0, 0);
        private readonly DateShifter _periodOpen;
        private readonly DateShifter _periodClose;

        private delegate DateTime DateShifter(DateTime dateTime);

        private Resolution Resolution
        {
            get { return Resolution.Days; }
        }

        private DateShifter ForwardShiftToOpen
        {
            get { return _forwardShiftToOpen; }
        }

        private DateShifter BackwardShiftToOpen
        {
            get { return _backwardShiftToOpen; }
        }

        private DateShifter ForwardShiftToClose
        {
            get { return _forwardShiftToClose; }
        }

        private DateShifter BackwardShiftToClose
        {
            get { return _backwardShiftToClose; }
        }

        private DateTime PeriodHead
        {
            get { return _periodHead; }
        }

        private DateShifter PeriodOpen
        {
            get { return _periodOpen; }
        }

        private DateShifter PeriodClose
        {
            get { return _periodClose; }
        }

        #endregion

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