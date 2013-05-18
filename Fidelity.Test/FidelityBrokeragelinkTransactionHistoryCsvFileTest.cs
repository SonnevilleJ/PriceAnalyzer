using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Fidelity;
using Sonneville.PriceTools.Yahoo;
using Sonneville.Utilities;
using Test.Sonneville.PriceTools.PortfolioData;

namespace Test.Sonneville.PriceTools.Fidelity
{
    /// <summary>
    ///This is a test class for FidelityTransactionHistoryCsvFileTest and is intended
    ///to contain all FidelityTransactionHistoryCsvFileTest Unit Tests
    ///</summary>
    [TestClass]
    public class FidelityBrokeragelinkTransactionHistoryCsvFileTest
    {
        private IPortfolioFactory _portfolioFactory;

        [TestInitialize]
        public void Initialize()
        {
            _portfolioFactory = new PortfolioFactory();
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioFcntxTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fcntx = portfolio.Positions.First(p => p.Ticker == "FCNTX");
                var investedValue = fcntx.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(530.24044m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioFdlsxTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fdlsx = portfolio.Positions.First(p => p.Ticker == "FDLSX");
                var investedValue = fdlsx.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(1780.07445m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioFemexTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var femex = portfolio.Positions.First(p => p.Ticker == "FEMEX");
                var investedValue = femex.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(800.00325m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioFemkxTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var femkx = portfolio.Positions.First(p => p.Ticker == "FEMKX");
                var investedValue = femkx.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(543.33666m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioFhkcxTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fhkcx = portfolio.Positions.First(p => p.Ticker == "FHKCX");
                var investedValue = fhkcx.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(558.50175m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioFicdxTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var ficdx = portfolio.Positions.First(p => p.Ticker == "FICDX");
                var investedValue = ficdx.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(919.53195m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioFlatxTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var flatx = portfolio.Positions.First(p => p.Ticker == "FLATX");
                var investedValue = flatx.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(1379.28336m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioFsagxTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fsagx = portfolio.Positions.First(p => p.Ticker == "FSAGX");
                var investedValue = fsagx.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(0m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioFschxTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fschx = portfolio.Positions.First(p => p.Ticker == "FSCHX");
                var investedValue = fschx.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(792.87264m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioFslbxTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fslbx = portfolio.Positions.First(p => p.Ticker == "FSLBX");
                var investedValue = fslbx.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(3376.71644m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioFsngxTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fsngx = portfolio.Positions.First(p => p.Ticker == "FSNGX");
                var investedValue = fsngx.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(1966.2302m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioFtrnxTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var ftrnx = portfolio.Positions.First(p => p.Ticker == "FTRNX");
                var investedValue = ftrnx.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(597.02433m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioAvailableCashTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);

                const decimal expectedAvailableCash = 1050.00m;
                var availableCash = portfolio.GetAvailableCash(settlementDate);
                Assert.AreEqual(expectedAvailableCash, availableCash);
            }
        }

        /// <summary>
        /// Ensure that price and shares are used correctly in determining final transaction value.
        /// </summary>
        [TestMethod]
        public void TransactionPriceRoundingTest()
        {
            using (Stream csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLinkTransactionPriceRounding.CsvString))
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
            using (var csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);

                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);

                Assert.AreEqual("FDRXX", portfolio.CashTicker);
            }
        }

        [TestMethod]
        public void PositionsTest()
        {
            using (var csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);

                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);

                Assert.AreEqual(12, portfolio.Positions.Count());
            }
        }

        [TestMethod]
        public void AvailableCashTest()
        {
            using (var csvStream = new ResourceStream(TestPortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);

                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);

                Assert.AreEqual(1050.00m, portfolio.GetAvailableCash(new DateTime(2009, 7, 23)));
            }
        }

        private static IPriceDataProvider GetProvider()
        {
            return new YahooPriceDataProvider();
        }
    }
}