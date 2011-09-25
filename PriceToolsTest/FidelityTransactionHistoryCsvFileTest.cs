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
        private const string Ticker = "FTEXX";

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
            Settings.CanConnectToInternet = true;

            using (Stream csvStream = new ResourceStream(TestData.FidelityTransactions))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                IPortfolio portfolio = new Portfolio(target, Ticker);
                var settlementDate = new DateTime(2010, 11, 16);

                var ALTR = portfolio.Positions.Where(p => p.Ticker == "ALTR").First();
                decimal investedValue = ALTR.CalculateInvestedValue(settlementDate);
                Assert.AreEqual(0.00m, investedValue);

                var NTAP = portfolio.Positions.Where(p => p.Ticker == "NTAP").First();
                investedValue = NTAP.CalculateInvestedValue(settlementDate);
                Assert.AreEqual(0.00m, investedValue);

                var NTCT = portfolio.Positions.Where(p => p.Ticker == "NTCT").First();
                investedValue = NTCT.CalculateInvestedValue(settlementDate);
                Assert.AreEqual(0.00m, investedValue);

                var PG = portfolio.Positions.Where(p => p.Ticker == "PG").First();
                investedValue = PG.CalculateInvestedValue(settlementDate);
                Assert.AreEqual(0.00m, investedValue);

                const decimal expectedAvailableCash = 2848.43m;
                var availableCash = portfolio.GetAvailableCash(settlementDate);
                Assert.AreEqual(expectedAvailableCash, availableCash);

                const decimal expectedValue = 2848.43m;
                var actualValue = portfolio.CalculateValue(new DateTime(2010, 11, 16));
                Assert.AreEqual(expectedValue, actualValue);
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

            IPortfolio target = new Portfolio(csvFile, Ticker);

            Assert.AreEqual(2848.43m, target.GetAvailableCash(new DateTime(2010, 11, 16)));
        }
    }
}
