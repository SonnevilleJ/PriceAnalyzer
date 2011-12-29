using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.SamplePriceData;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceToolsTest
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
        public void ParsePortfolioTest()
        {
            using (Stream csvStream = new ResourceStream(TestData.FidelityTransactions))
            {
                var target = new FidelityTransactionHistoryCsvFile(csvStream);
                IPortfolio portfolio = new Portfolio(target, "FTEXX");
                var settlementDate = new DateTime(2010, 11, 16);
                PriceHistoryCsvFileProvider provider = new YahooPriceHistoryCsvFileProvider();

                var ALTR = portfolio.Positions.Where(p => p.Ticker == "ALTR").First();
                decimal investedValue = ALTR.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(0.00m, investedValue);

                var NTAP = portfolio.Positions.Where(p => p.Ticker == "NTAP").First();
                investedValue = NTAP.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(0.00m, investedValue);

                var NTCT = portfolio.Positions.Where(p => p.Ticker == "NTCT").First();
                investedValue = NTCT.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(0.00m, investedValue);

                var PG = portfolio.Positions.Where(p => p.Ticker == "PG").First();
                investedValue = PG.CalculateInvestedValue(provider, settlementDate);
                Assert.AreEqual(0.00m, investedValue);

                const decimal expectedAvailableCash = 2848.43m;
                var availableCash = portfolio.GetAvailableCash(settlementDate);
                Assert.AreEqual(expectedAvailableCash, availableCash);

                const decimal expectedValue = 2848.43m;
                var actualValue = portfolio.CalculateTotalValue(provider, new DateTime(2010, 11, 16));
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

            IPortfolio target = new Portfolio(csvFile, "FTEXX");

            Assert.AreEqual(2848.43m, target.GetAvailableCash(new DateTime(2010, 11, 16)));
        }
    }
}
