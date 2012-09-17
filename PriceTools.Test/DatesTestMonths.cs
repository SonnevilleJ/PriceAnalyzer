using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class DatesTestMonths : DatesTest
    {
        protected override Resolution Resolution
        {
            get { return Resolution.Months; }
        }

        [TestMethod]
        public override void NextPeriodOpenTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddMonths(1);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void NextPeriodOpenTestFromBeforeOpen()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(1);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void NextPeriodOpenTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-1).AddMonths(1);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void NextPeriodOpenTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(1);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void NextPeriodOpenTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(2);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void NextPeriodOpenTestFromAfterClose()
        {
            var start = PeriodHead;

            var expected = start.AddMonths(1);
            var actual = start.NextPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void NextPeriodCloseTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1).AddMonths(2);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void NextPeriodCloseTestFromBeforeOpen()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddMonths(1);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void NextPeriodCloseTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-2).AddMonths(2);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void NextPeriodCloseTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddMonths(1);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void NextPeriodCloseTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(1).AddMonths(1);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void NextPeriodCloseTestFromAfterClose()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1).AddMonths(2);
            var actual = start.NextPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void PreviousPeriodOpenTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddMonths(-1);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void PreviousPeriodOpenTestFromBeforeOpen()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(1).AddMonths(-2);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void PreviousPeriodOpenTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-1).AddMonths(-1);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void PreviousPeriodOpenTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddTicks(1).AddMonths(-2);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void PreviousPeriodOpenTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(2).AddMonths(-2);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void PreviousPeriodOpenTestFromAfterClose()
        {
            var start = PeriodHead;

            var expected = start.AddMonths(-1);
            var actual = start.PreviousPeriodOpen(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void PreviousPeriodCloseTestFromOpen()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void PreviousPeriodCloseTestFromBeforeOpen()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddMonths(-1);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void PreviousPeriodCloseTestFromAfterOpen()
        {
            var start = PeriodHead.AddTicks(1);

            var expected = start.AddTicks(-2);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void PreviousPeriodCloseTestFromClose()
        {
            var start = PeriodHead.AddTicks(-1);

            var expected = start.AddMonths(-1);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void PreviousPeriodCloseTestFromBeforeClose()
        {
            var start = PeriodHead.AddTicks(-2);

            var expected = start.AddTicks(1).AddMonths(-1);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void PreviousPeriodCloseTestFromAfterClose()
        {
            var start = PeriodHead;

            var expected = start.AddTicks(-1);
            var actual = start.PreviousPeriodClose(Resolution);
            Assert.AreEqual(expected, actual);
        }
    }
}