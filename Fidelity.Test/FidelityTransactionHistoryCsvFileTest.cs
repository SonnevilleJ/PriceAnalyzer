using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.SampleData;
using Sonneville.PriceTools.Yahoo;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Fidelity.Test
{
    /// <summary>
    ///This is a test class for FidelityTransactionHistoryCsvFileTest and is intended
    ///to contain all FidelityTransactionHistoryCsvFileTest Unit Tests
    ///</summary>
    [TestClass]
    public class FidelityTransactionHistoryCsvFileTest
    {
        private IPortfolioFactory _portfolioFactory;
        private IPriceHistoryCsvFileFactory _priceHistoryCsvFileFactory;

        [TestInitialize]
        public void Setup()
        {
            _portfolioFactory = new PortfolioFactory();
            _priceHistoryCsvFileFactory = new YahooPriceDataProvider();
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
                var provider = new CsvPriceDataProvider(new WebClientWrapper(), new YahooPriceHistoryQueryUrlBuilder());

                var altr = portfolio.Positions.First(p => p.Ticker == "ALTR");
                var investedValue = AutomatedTrading.SecurityBasketExtensions.CalculateMarketValue(altr, provider, settlementDate, _priceHistoryCsvFileFactory);
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
                var provider = new CsvPriceDataProvider(new WebClientWrapper(), new YahooPriceHistoryQueryUrlBuilder());

                var ntap = portfolio.Positions.First(p => p.Ticker == "NTAP");
                var investedValue = AutomatedTrading.SecurityBasketExtensions.CalculateMarketValue(ntap, provider, settlementDate, _priceHistoryCsvFileFactory);
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
                var provider = new CsvPriceDataProvider(new WebClientWrapper(), new YahooPriceHistoryQueryUrlBuilder());

                var ntct = portfolio.Positions.First(p => p.Ticker == "NTCT");
                var investedValue = AutomatedTrading.SecurityBasketExtensions.CalculateMarketValue(ntct, provider, settlementDate, _priceHistoryCsvFileFactory);
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
                var provider = new CsvPriceDataProvider(new WebClientWrapper(), new YahooPriceHistoryQueryUrlBuilder());

                var pg = portfolio.Positions.First(p => p.Ticker == "PG");
                var investedValue = AutomatedTrading.SecurityBasketExtensions.CalculateMarketValue(pg, provider, settlementDate, _priceHistoryCsvFileFactory);
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
