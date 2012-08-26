using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.SamplePortfolioData;

namespace Test.Sonneville.PriceTools.SamplePortfolioData
{
    [TestClass]
    public abstract class SamplePortfolioTestBase
    {
        protected static Portfolio Portfolio { get; private set; }

        protected SamplePortfolioTestBase(Portfolio portfolio)
        {
            Portfolio = portfolio;
        }

        [TestMethod]
        public abstract void CashTickerTest();

        [TestMethod]
        public abstract void HeadTest();

        [TestMethod]
        public abstract void TailTest();

        [TestMethod]
        public abstract void ResolutionTest();

        [TestMethod]
        public abstract void TransactionsTest();
    }

    /// <summary>
    /// Contains test methods for BrokerageLink_trades portfolio.
    /// </summary>
    [TestClass]
    public class BrokerageLink_trades_Test : SamplePortfolioTestBase
    {
        public BrokerageLink_trades_Test()
            : base(SamplePortfolios.BrokerageLink_trades)
        {
        }

        [TestMethod]
        public override void CashTickerTest()
        {
            const string expected = "FDRXX";
            var actual = Portfolio.CashTicker;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void HeadTest()
        {
            var expected = new DateTime(2008, 4, 17);
            var actual = Portfolio.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void TailTest()
        {
            var expected = new DateTime(2009, 7, 23);
            var actual = Portfolio.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void ResolutionTest()
        {
            const Resolution expected = Resolution.Days;
            var actual = Portfolio.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void TransactionsTest()
        {
            const int expected = 136;
            var actual = Portfolio.Transactions.Count;
            Assert.AreEqual(expected, actual);
        }
    }

    /// <summary>
    /// Contains test methods for FidelityTransactions portfolio.
    /// </summary>
    [TestClass]
    public class FidelityTransactions_Test : SamplePortfolioTestBase
    {
        public FidelityTransactions_Test()
            : base(SamplePortfolios.FidelityTransactions)
        {
        }

        [TestMethod]
        public override void CashTickerTest()
        {
            const string expected = "FTEXX";
            var actual = Portfolio.CashTicker;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void HeadTest()
        {
            var expected = new DateTime(2010, 5, 17);
            var actual = Portfolio.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void TailTest()
        {
            var expected = new DateTime(2010, 11, 16);
            var actual = Portfolio.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void ResolutionTest()
        {
            const Resolution expected = Resolution.Days;
            var actual = Portfolio.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public override void TransactionsTest()
        {
            const int expected = 26;
            var actual = Portfolio.Transactions.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
