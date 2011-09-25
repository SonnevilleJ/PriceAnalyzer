using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Services;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for FidelityTransactionHistoryCsvFileTest and is intended
    ///to contain all FidelityTransactionHistoryCsvFileTest Unit Tests
    ///</summary>
    [TestClass]
    public class FidelityTransactionHistoryCsvFileTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Settings.SetDefaultSettings();
            Settings.CanConnectToInternet = false;
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioTest()
        {
            using (Stream csvStream = new ResourceStream(TestData.FidelityTransactions))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                IPortfolio portfolio = new Portfolio(target, "FTEXX");

                const decimal expected = 2848.43m;
                var actual = portfolio.GetValue(new DateTime(2010, 11, 16));
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void TickerTest()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new ResourceStream(TestData.FidelityTransactions));
            var ticker = String.Empty;

            IPortfolio target = new Portfolio(csvFile, ticker);

            Assert.AreEqual(ticker, target.CashTicker);
        }

        [TestMethod]
        public void PositionsTest()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new ResourceStream(TestData.FidelityTransactions));
            var ticker = String.Empty;

            IPortfolio target = new Portfolio(csvFile, ticker);

            Assert.AreEqual(5, target.Positions.Count);
        }

        [TestMethod]
        public void AvailableCashTest()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new ResourceStream(TestData.FidelityTransactions));
            var ticker = String.Empty;

            IPortfolio target = new Portfolio(csvFile, ticker);

            Assert.AreEqual(2848.43m, target.GetAvailableCash(new DateTime(2010, 11, 17)));
        }
    }
}
