using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Yahoo;
using Sonneville.PriceTools.SamplePortfolioData;
using Sonneville.PriceTools.SamplePriceData;

namespace Sonneville.PriceTools.Fidelity.Test
{
    /// <summary>
    ///This is a test class for FidelityTransactionHistoryCsvFileTest and is intended
    ///to contain all FidelityTransactionHistoryCsvFileTest Unit Tests
    ///</summary>
    [TestClass]
    public class FidelityTransactionHistoryCsvFileTest
    {
        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioAltrTest()
        {
            using (Stream csvStream = new ResourceStream(PortfolioCsv.FidelityTransactions))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                var portfolio = PortfolioFactory.ConstructPortfolio(target, "FTEXX");
                var settlementDate = new DateTime(2010, 11, 16);
                var provider = new YahooPriceDataProvider();

                var altr = portfolio.Positions.Where(p => p.Ticker == "ALTR").First();
                var investedValue = altr.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(0.00m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioNtapTest()
        {
            using (Stream csvStream = new ResourceStream(PortfolioCsv.FidelityTransactions))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                var portfolio = PortfolioFactory.ConstructPortfolio(target, "FTEXX");
                var settlementDate = new DateTime(2010, 11, 16);
                var provider = new YahooPriceDataProvider();

                var ntap = portfolio.Positions.Where(p => p.Ticker == "NTAP").First();
                var investedValue = ntap.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(0.00m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioNtctTest()
        {
            using (Stream csvStream = new ResourceStream(PortfolioCsv.FidelityTransactions))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                var portfolio = PortfolioFactory.ConstructPortfolio(target, "FTEXX");
                var settlementDate = new DateTime(2010, 11, 16);
                var provider = new YahooPriceDataProvider();

                var ntct = portfolio.Positions.Where(p => p.Ticker == "NTCT").First();
                var investedValue = ntct.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(0.00m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioPgTest()
        {
            using (Stream csvStream = new ResourceStream(PortfolioCsv.FidelityTransactions))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                var portfolio = PortfolioFactory.ConstructPortfolio(target, "FTEXX");
                var settlementDate = new DateTime(2010, 11, 16);
                var provider = new YahooPriceDataProvider();

                var pg = portfolio.Positions.Where(p => p.Ticker == "PG").First();
                var investedValue = pg.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(0.00m, investedValue);
            }
        }

        /// <summary>
        ///A test for ParsePortfolio
        ///</summary>
        [TestMethod]
        public void ParsePortfolioAvailableCashTest()
        {
            using (Stream csvStream = new ResourceStream(PortfolioCsv.FidelityTransactions))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                var portfolio = PortfolioFactory.ConstructPortfolio(target, "FTEXX");
                var settlementDate = new DateTime(2010, 11, 16);

                const decimal expectedAvailableCash = 2848.43m;
                var availableCash = portfolio.GetAvailableCash(settlementDate);
                Assert.AreEqual(expectedAvailableCash, availableCash);
            }
        }

        [TestMethod]
        public void TickerTest()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new ResourceStream(PortfolioCsv.FidelityTransactions));
            var ticker = String.Empty;

            Portfolio target = PortfolioFactory.ConstructPortfolio(csvFile, ticker);

            Assert.AreEqual(ticker, target.CashTicker);
        }

        [TestMethod]
        public void PositionsTest()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new ResourceStream(PortfolioCsv.FidelityTransactions));
            var ticker = String.Empty;

            Portfolio target = PortfolioFactory.ConstructPortfolio(csvFile, ticker);

            Assert.AreEqual(5, target.Positions.Count);
        }

        [TestMethod]
        public void AvailableCashTest()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new ResourceStream(PortfolioCsv.FidelityTransactions));

            Portfolio target = PortfolioFactory.ConstructPortfolio(csvFile, "FTEXX");

            Assert.AreEqual(2848.43m, target.GetAvailableCash(new DateTime(2010, 11, 16)));
        }
    }
}
