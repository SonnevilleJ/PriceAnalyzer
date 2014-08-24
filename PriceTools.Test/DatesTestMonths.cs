using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class DatesTestMonths : DatesTest
    {
        protected override Resolution Resolution
        {
            get { return Resolution.Months; }
        }

        [Test]
        public override void CurrentPeriodOpenTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1).AddMonths(1);

            var expected = start.AddTicks(1).AddMonths(-1);
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void CurrentPeriodOpenTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(2).AddMonths(-1);
            var actual = start.CurrentPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void CurrentPeriodCloseTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1).AddMonths(1);
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void CurrentPeriodCloseTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-2).AddMonths(1);
            var actual = start.CurrentPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void NextPeriodOpenTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddMonths(1);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void NextPeriodOpenTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-1).AddMonths(1);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void NextPeriodCloseTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1).AddMonths(2);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void NextPeriodCloseTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-2).AddMonths(2);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void NextPeriodCloseTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddMonths(1);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void NextPeriodCloseTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(1).AddMonths(1);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void PreviousPeriodOpenTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddMonths(-1);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void PreviousPeriodOpenTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-1).AddMonths(-1);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void PreviousPeriodOpenTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(1).AddMonths(-2);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void PreviousPeriodOpenTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(2).AddMonths(-2);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void PreviousPeriodCloseTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddMonths(-1);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void PreviousPeriodCloseTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(1).AddMonths(-1);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void SeekForwardOneTest()
        {
            var start = new DateTime(2012, 10, 15);

            var expected = start.AddMonths(1);
            var actual = start.SeekPeriods(1, Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void SeekBackwardOneTest()
        {
            var start = new DateTime(2012, 10, 15);

            var expected = start.AddMonths(-1);
            var actual = start.SeekPeriods(-1, Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void SeekForwardTenTest()
        {
            var start = new DateTime(2012, 10, 15);

            var expected = start.AddMonths(10);
            var actual = start.SeekPeriods(10, Resolution);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public override void SeekBackwardTenTest()
        {
            var start = new DateTime(2012, 10, 15);

            var expected = start.AddMonths(-10);
            var actual = start.SeekPeriods(-10, Resolution);
            Assert.AreEqual(expected, actual);
        }
    }
}