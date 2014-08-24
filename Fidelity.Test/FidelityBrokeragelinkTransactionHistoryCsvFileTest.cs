using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Implementation;
using Sonneville.PriceTools.SampleData;
using Sonneville.PriceTools.Yahoo;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Fidelity.Test
{
    [TestFixture]
    public class FidelityBrokeragelinkTransactionHistoryCsvFileTest
    {
        private IPortfolioFactory _portfolioFactory;
        private IPriceHistoryCsvFileFactory _priceHistoryCsvFileFactory;
        private ISecurityBasketCalculator _securityBasketCalculator;

        [SetUp]
        public void Setup()
        {
            _portfolioFactory = new PortfolioFactory();
            _priceHistoryCsvFileFactory = new YahooPriceHistoryCsvFileFactory();
            _securityBasketCalculator = new SecurityBasketCalculator();
        }

        [Test]
        public void ParsePortfolioFcntxTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fcntx = portfolio.Positions.First(p => p.Ticker == "FCNTX");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(fcntx, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(530.24044m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioFdlsxTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fdlsx = portfolio.Positions.First(p => p.Ticker == "FDLSX");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(fdlsx, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(1780.07445m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioFemexTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var femex = portfolio.Positions.First(p => p.Ticker == "FEMEX");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(femex, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(800.00325m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioFemkxTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var femkx = portfolio.Positions.First(p => p.Ticker == "FEMKX");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(femkx, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(543.33666m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioFhkcxTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fhkcx = portfolio.Positions.First(p => p.Ticker == "FHKCX");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(fhkcx, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(558.50175m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioFicdxTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var ficdx = portfolio.Positions.First(p => p.Ticker == "FICDX");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(ficdx, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(919.53195m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioFlatxTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var flatx = portfolio.Positions.First(p => p.Ticker == "FLATX");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(flatx, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(1379.28336m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioFsagxTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fsagx = portfolio.Positions.First(p => p.Ticker == "FSAGX");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(fsagx, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(0m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioFschxTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fschx = portfolio.Positions.First(p => p.Ticker == "FSCHX");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(fschx, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(792.87264m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioFslbxTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fslbx = portfolio.Positions.First(p => p.Ticker == "FSLBX");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(fslbx, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(3376.71644m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioFsngxTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var fsngx = portfolio.Positions.First(p => p.Ticker == "FSNGX");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(fsngx, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(1966.2302m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioFtrnxTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);
                var provider = GetProvider();

                var ftrnx = portfolio.Positions.First(p => p.Ticker == "FTRNX");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(ftrnx, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(597.02433m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioAvailableCashTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLink.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);
                var settlementDate = new DateTime(2009, 7, 23);

                const decimal expectedAvailableCash = 1050.00m;
                var availableCash = portfolio.GetAvailableCash(settlementDate);
                Assert.AreEqual(expectedAvailableCash, availableCash);
            }
        }

        [Test]
        public void TransactionPriceRoundingTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityBrokerageLinkTransactionPriceRounding.CsvString))
            {
                var target = new FidelityBrokerageLinkTransactionHistoryCsvFile(csvStream);

                const decimal expected = 500.00m;
                var actual = ((ShareTransaction)target.Transactions.First()).TotalValue;
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void TickerTest()
        {
            var target = SamplePortfolios.FidelityBrokerageLink.TransactionHistory;

            var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);

            Assert.AreEqual("FDRXX", portfolio.CashTicker);
        }

        [Test]
        public void PositionsTest()
        {
            var target = SamplePortfolios.FidelityBrokerageLink.TransactionHistory;

            var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);

            Assert.AreEqual(12, portfolio.Positions.Count());
        }

        [Test]
        public void AvailableCashTest()
        {
            var target = SamplePortfolios.FidelityBrokerageLink.TransactionHistory;

            var portfolio = _portfolioFactory.ConstructPortfolio("FDRXX", target.Transactions);

            Assert.AreEqual(1050.00m, portfolio.GetAvailableCash(new DateTime(2009, 7, 23)));
        }

        private static IPriceDataProvider GetProvider()
        {
            var webClientMock = new Mock<IWebClient>();
            webClientMock.Setup(x => x.OpenRead(It.IsAny<string>())).Returns<string>(GetPriceDataStream);
            return new PriceDataProvider(webClientMock.Object, new YahooPriceHistoryQueryUrlBuilder(), new YahooPriceHistoryCsvFileFactory());
        }

        private static Stream GetPriceDataStream(string arg)
        {
            var ticker = arg.Split('?', '&').ElementAt(1).Remove(0, 2);

            var resourceSet = CsvPriceData.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                if (entry.Key as string == ticker)
                    return new MemoryStream(Encoding.Default.GetBytes(entry.Value as string));
            }
            return new MemoryStream();
        }
    }
}