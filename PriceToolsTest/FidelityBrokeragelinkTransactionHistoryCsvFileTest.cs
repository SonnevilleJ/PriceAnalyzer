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
    public class FidelityBrokeragelinkTransactionHistoryCsvFileTest
    {
        private const string Ticker = "FDRXX";

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

            using (Stream csvStream = new ResourceStream(TestData.BrokerageLink_trades))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                IPortfolio portfolio = new Portfolio(target, Ticker);
                var settlementDate = new DateTime(2009, 7, 23);

                var FCNTX = portfolio.Positions.Where(p => p.Ticker == "FCNTX").First();
                decimal investedValue = FCNTX.GetInvestedValue(settlementDate);
                Assert.AreEqual(530.24044m, investedValue);

                var FDLSX = portfolio.Positions.Where(p => p.Ticker == "FDLSX").First();
                investedValue = FDLSX.GetInvestedValue(settlementDate);
                Assert.AreEqual(1780.07445m, investedValue);

                var FEMEX = portfolio.Positions.Where(p => p.Ticker == "FEMEX").First();
                investedValue = FEMEX.GetInvestedValue(settlementDate);
                Assert.AreEqual(800.00325m, investedValue);

                var FEMKX = portfolio.Positions.Where(p => p.Ticker == "FEMKX").First();
                investedValue = FEMKX.GetInvestedValue(settlementDate);
                Assert.AreEqual(543.33666m, investedValue);

                var FHKCX = portfolio.Positions.Where(p => p.Ticker == "FHKCX").First();
                investedValue = FHKCX.GetInvestedValue(settlementDate);
                Assert.AreEqual(558.50175m, investedValue);

                var FICDX = portfolio.Positions.Where(p => p.Ticker == "FICDX").First();
                investedValue = FICDX.GetInvestedValue(settlementDate);
                Assert.AreEqual(919.53195m, investedValue);

                var FLATX = portfolio.Positions.Where(p => p.Ticker == "FLATX").First();
                investedValue = FLATX.GetInvestedValue(settlementDate);
                Assert.AreEqual(1379.28336m, investedValue);

                var FSAGX = portfolio.Positions.Where(p => p.Ticker == "FSAGX").First();
                investedValue = FSAGX.GetInvestedValue(settlementDate);
                Assert.AreEqual(0m, investedValue);

                var FSCHX = portfolio.Positions.Where(p => p.Ticker == "FSCHX").First();
                investedValue = FSCHX.GetInvestedValue(settlementDate);
                Assert.AreEqual(792.87264m, investedValue);

                var FSLBX = portfolio.Positions.Where(p => p.Ticker == "FSLBX").First();
                investedValue = FSLBX.GetInvestedValue(settlementDate);
                Assert.AreEqual(3376.71644m, investedValue);

                var FSNGX = portfolio.Positions.Where(p => p.Ticker == "FSNGX").First();
                investedValue = FSNGX.GetInvestedValue(settlementDate);
                Assert.AreEqual(1966.2302m, investedValue);

                var FTRNX = portfolio.Positions.Where(p => p.Ticker == "FTRNX").First();
                investedValue = FTRNX.GetInvestedValue(settlementDate);
                Assert.AreEqual(597.02433m, investedValue);

                const decimal expected = 1040.24m;
                var actual = portfolio.GetAvailableCash(settlementDate);
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Ensure that price and shares are used correctly in determining final transaction value.
        /// </summary>
        [TestMethod]
        public void TransactionPriceRoundingTest()
        {
            using (Stream csvStream = new ResourceStream(TestData.BrokerageLink_TransactionPriceRounding))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);

                const decimal expected = 500.00m;
                var actual = ((IShareTransaction)target.Transactions.First()).TotalValue;
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void TickerTest()
        {
            using (var csvStream = new ResourceStream(TestData.BrokerageLink_trades))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);

                IPortfolio portfolio = new Portfolio(target, Ticker);

                Assert.AreEqual(Ticker, portfolio.CashTicker);
            }
        }

        [TestMethod]
        public void PositionsTest()
        {
            using (var csvStream = new ResourceStream(TestData.BrokerageLink_trades))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);

                IPortfolio portfolio = new Portfolio(target, Ticker);

                Assert.AreEqual(12, portfolio.Positions.Count);
            }
        }

        [TestMethod]
        public void AvailableCashTest()
        {
            using (var csvStream = new ResourceStream(TestData.BrokerageLink_trades))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);

                IPortfolio portfolio = new Portfolio(target, Ticker);

                Assert.AreEqual(1116.18m, portfolio.GetAvailableCash(new DateTime(2009, 7, 23)));
            }
        }
    }
}