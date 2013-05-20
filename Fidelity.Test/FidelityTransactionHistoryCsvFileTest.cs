using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleData;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Fidelity;
using Sonneville.PriceTools.Yahoo;
using Sonneville.Utilities;

namespace Test.Sonneville.PriceTools.Fidelity
{
    /// <summary>
    ///This is a test class for FidelityTransactionHistoryCsvFileTest and is intended
    ///to contain all FidelityTransactionHistoryCsvFileTest Unit Tests
    ///</summary>
    [TestClass]
    public class FidelityTransactionHistoryCsvFileTest
    {
        private readonly IPortfolioFactory _portfolioFactory;

        public FidelityTransactionHistoryCsvFileTest()
        {
            _portfolioFactory = new PortfolioFactory();
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioAltrTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FTEXX", target.Transactions);
                var settlementDate = new DateTime(2010, 11, 16);
                var provider = new YahooPriceDataProvider();

                var altr = portfolio.Positions.First(p => p.Ticker == "ALTR");
                var investedValue = altr.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(0.00m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioNtapTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FTEXX", target.Transactions);
                var settlementDate = new DateTime(2010, 11, 16);
                var provider = new YahooPriceDataProvider();

                var ntap = portfolio.Positions.First(p => p.Ticker == "NTAP");
                var investedValue = ntap.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(0.00m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioNtctTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FTEXX", target.Transactions);
                var settlementDate = new DateTime(2010, 11, 16);
                var provider = new YahooPriceDataProvider();

                var ntct = portfolio.Positions.First(p => p.Ticker == "NTCT");
                var investedValue = ntct.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(0.00m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioPgTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FTEXX", target.Transactions);
                var settlementDate = new DateTime(2010, 11, 16);
                var provider = new YahooPriceDataProvider();

                var pg = portfolio.Positions.First(p => p.Ticker == "PG");
                var investedValue = pg.CalculateMarketValue(provider, settlementDate);
                Assert.AreEqual(0.00m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioAvailableCashTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FTEXX", target.Transactions);
                var settlementDate = new DateTime(2010, 11, 16);

                const decimal expectedAvailableCash = 2848.43m;
                var availableCash = portfolio.GetAvailableCash(settlementDate);
                Assert.AreEqual(expectedAvailableCash, availableCash);
            }
        }

        [TestMethod]
        public void TickerTest()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString));
            var ticker = String.Empty;

            var target = _portfolioFactory.ConstructPortfolio(ticker, csvFile.Transactions);

            Assert.AreEqual(ticker, target.CashTicker);
        }

        [TestMethod]
        public void PositionsTest()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString));
            var ticker = String.Empty;

            var target = _portfolioFactory.ConstructPortfolio(ticker, csvFile.Transactions);

            Assert.AreEqual(5, target.Positions.Count());
        }

        [TestMethod]
        public void AvailableCashTest()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString));

            var target = _portfolioFactory.ConstructPortfolio("FTEXX", csvFile.Transactions);

            Assert.AreEqual(2848.43m, target.GetAvailableCash(new DateTime(2010, 11, 16)));
        }
    }
}
