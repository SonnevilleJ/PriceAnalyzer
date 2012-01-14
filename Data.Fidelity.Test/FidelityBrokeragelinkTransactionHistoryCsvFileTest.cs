using System;
using System.IO;
using System.Linq;
using Data.Yahoo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamplePortfolioData;
using Sonneville.PriceTools.SamplePriceData;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools.Data.Fidelity.Test
{
    /// <summary>
    ///This is a test class for FidelityTransactionHistoryCsvFileTest and is intended
    ///to contain all FidelityTransactionHistoryCsvFileTest Unit Tests
    ///</summary>
    [TestClass]
    public class FidelityBrokeragelinkTransactionHistoryCsvFileTest
    {
        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioTest()
        {
            using (Stream csvStream = new ResourceStream(PortfolioCsv.BrokerageLink_trades))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                IPortfolio portfolio = new Portfolio(target, "FDRXX");
                var settlementDate = new DateTime(2009, 7, 23);
                IPriceDataProvider provider = new YahooPriceDataProvider();

                var FCNTX = portfolio.Positions.Where(p => p.Ticker == "FCNTX").First();
                decimal investedValue = FCNTX.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(530.24044m, investedValue);

                var FDLSX = portfolio.Positions.Where(p => p.Ticker == "FDLSX").First();
                investedValue = FDLSX.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(1780.07445m, investedValue);

                var FEMEX = portfolio.Positions.Where(p => p.Ticker == "FEMEX").First();
                investedValue = FEMEX.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(800.00325m, investedValue);

                var FEMKX = portfolio.Positions.Where(p => p.Ticker == "FEMKX").First();
                investedValue = FEMKX.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(543.33666m, investedValue);

                var FHKCX = portfolio.Positions.Where(p => p.Ticker == "FHKCX").First();
                investedValue = FHKCX.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(558.50175m, investedValue);

                var FICDX = portfolio.Positions.Where(p => p.Ticker == "FICDX").First();
                investedValue = FICDX.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(919.53195m, investedValue);

                var FLATX = portfolio.Positions.Where(p => p.Ticker == "FLATX").First();
                investedValue = FLATX.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(1379.28336m, investedValue);

                var FSAGX = portfolio.Positions.Where(p => p.Ticker == "FSAGX").First();
                investedValue = FSAGX.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(0m, investedValue);

                var FSCHX = portfolio.Positions.Where(p => p.Ticker == "FSCHX").First();
                investedValue = FSCHX.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(792.87264m, investedValue);

                var FSLBX = portfolio.Positions.Where(p => p.Ticker == "FSLBX").First();
                investedValue = FSLBX.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(3376.71644m, investedValue);

                var FSNGX = portfolio.Positions.Where(p => p.Ticker == "FSNGX").First();
                investedValue = FSNGX.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(1966.2302m, investedValue);

                var FTRNX = portfolio.Positions.Where(p => p.Ticker == "FTRNX").First();
                investedValue = FTRNX.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(597.02433m, investedValue);

                const decimal expectedAvailableCash = 1050.00m;
                var availableCash = portfolio.GetAvailableCash(settlementDate);
                Assert.AreEqual(expectedAvailableCash, availableCash);

                const decimal expectedValue = 14293.81547m;
                var actualValue = portfolio.CalculateTotalValue(provider, settlementDate);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        /// <summary>
        /// Ensure that price and shares are used correctly in determining final transaction value.
        /// </summary>
        [TestMethod]
        public void TransactionPriceRoundingTest()
        {
            using (Stream csvStream = new ResourceStream(PortfolioCsv.BrokerageLink_TransactionPriceRounding))
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
            using (var csvStream = new ResourceStream(PortfolioCsv.BrokerageLink_trades))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);

                IPortfolio portfolio = new Portfolio(target, "FDRXX");

                Assert.AreEqual("FDRXX", portfolio.CashTicker);
            }
        }

        [TestMethod]
        public void PositionsTest()
        {
            using (var csvStream = new ResourceStream(PortfolioCsv.BrokerageLink_trades))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);

                IPortfolio portfolio = new Portfolio(target, "FDRXX");

                Assert.AreEqual(12, portfolio.Positions.Count);
            }
        }

        [TestMethod]
        public void AvailableCashTest()
        {
            using (var csvStream = new ResourceStream(PortfolioCsv.BrokerageLink_trades))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);

                IPortfolio portfolio = new Portfolio(target, "FDRXX");

                Assert.AreEqual(1050.00m, portfolio.GetAvailableCash(new DateTime(2009, 7, 23)));
            }
        }
    }
}