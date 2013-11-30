using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.Yahoo;

namespace Test.Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    ///This is a test class for PositionTest and is intended
    ///to contain all PositionTest Unit Tests
    ///</summary>
    [TestClass]
    public class PositionTest
    {
        private IPositionFactory _positionFactory;
        private ITransactionFactory _transactionFactory;
        private IPriceHistoryCsvFileFactory _priceHistoryCsvFileFactory;

        [TestInitialize]
        public void Setup()
        {
            _positionFactory = new PositionFactory();
            _transactionFactory = new TransactionFactory();
            _priceHistoryCsvFileFactory = new YahooPriceDataProvider();
        }

        [TestMethod]
        public void IndexerReturnsCalculateGrossProfit()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2000, 1, 1);
            const decimal price = 100.00m;     // bought at $100.00 per share
            const decimal shares = 5;           // bought 5 shares
            const decimal commission = 7.95m;  // bought with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission));

            var expected = target.CalculateGrossProfit(buyDate);
            decimal? actual = target[buyDate];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTickerThrowsException()
        {
            _positionFactory.ConstructPosition(null);
        }

        [TestMethod]
        public void CalculateMarketValueTestBuy()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2000, 12, 29);
            const decimal price = 100.00m;      // $100.00 per share
            const decimal shares = 5;            // 5 shares
            const decimal commission = 7.95m;   // with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission));

            // DE price @ 29 Dec 2000 = $45.81
            // invested value should be $45.81 * 5 shares = $229.05
            const decimal currentPrice = 45.81m;
            const decimal expected = (currentPrice * shares);
            IPriceDataProvider provider = new CsvPriceDataProvider(new WebClientWrapper(), new YahooPriceHistoryQueryUrlBuilder());
            var actual = target.CalculateMarketValue(provider, buyDate, _priceHistoryCsvFileFactory);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMarketValueTestSellHalf()
        {
            const string ticker = "DE";

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 1, 2);
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal shares = 10;              // 10 shares
            const decimal commission = 7.95m;       // with $7.95 commission
            const decimal sharesSold = shares/2;
            const decimal sellPrice = buyPrice + 10m;

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission));
            
            // DE price @ 29 Dec 2000 = $44.81
            // invested value should be $44.81 * 5 shares = $224.05
            const decimal currentPrice = 44.81m;
            const decimal expected = (currentPrice*sharesSold);
            IPriceDataProvider provider = new CsvPriceDataProvider(new WebClientWrapper(), new YahooPriceHistoryQueryUrlBuilder());
            var actual = target.CalculateMarketValue(provider, sellDate, _priceHistoryCsvFileFactory);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMarketValueTestSellAll()
        {
            const string ticker = "DE";

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal shares = 10;              // 10 shares
            const decimal commission = 7.95m;       // with $7.95 commission
            const decimal sellPrice = buyPrice + 10m;

            var target = _positionFactory.ConstructPosition(ticker,
                                               _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission),
                                               _transactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            const decimal expected = 0.00m;         // $0.00 currently invested
            IPriceDataProvider provider = new CsvPriceDataProvider(new WebClientWrapper(), new YahooPriceHistoryQueryUrlBuilder());
            var actual = target.CalculateMarketValue(provider, sellDate, _priceHistoryCsvFileFactory);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SellTooManySharesTest()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sharesBought = 10;        // 10 shares
            const decimal commission = 7.95m;       // with $7.95 commission
            const decimal sharesSold = sharesBought + 1;
            const decimal sellPrice = buyPrice + 10m;

            _positionFactory.ConstructPosition(ticker,
                                              _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                              _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission));
        }

        [TestMethod]
        public void LongTransactionCountReturnsCorrectTransactionCount()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;   // $100.00 per share
            const decimal sellPrice = 120.00m;  // $110.00 per share
            const decimal shares = 5;           // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                               _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission),
                                               _transactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            const int expected = 2;
            var longActual = target.Transactions.Count();
            Assert.AreEqual(expected, longActual);
        }

        [TestMethod]
        public void ShortTransactionCountReturnsCorrectTransactionCount()
        {
            const string ticker = "CAT";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;   // $100.00 per share
            const decimal sellPrice = 120.00m;  // $110.00 per share
            const decimal shares = 5;           // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                   _transactionFactory.ConstructSellShort(ticker, buyDate, shares, buyPrice, commission),
                                   _transactionFactory.ConstructBuyToCover(ticker, sellDate, shares, sellPrice, commission));

            const int expected = 2;
            var shortActual = target.Transactions.Count();
            Assert.AreEqual(expected, shortActual);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod]
        public void TickerTest()
        {
            const string ticker = "DE";
            var target = _positionFactory.ConstructPosition(ticker);

            const string expectedTicker = ticker;
            var actualTicker = target.Ticker;
            Assert.AreEqual(expectedTicker, actualTicker);
        }

        /// <summary>
        ///A test for CalculateCost
        ///</summary>
        [TestMethod]
        public void CalculateCostWithBuyOnlyTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission));

            const decimal expectedCosts = 500.00m;
            var actualCosts = target.CalculateCost(buyDate);
            Assert.AreEqual(expectedCosts, actualCosts);
        }

        /// <summary>
        ///A test for CalculateCost
        ///</summary>
        [TestMethod]
        public void CalculateCostWithBuyAndSellTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares
            
            var sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                   _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                   _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission));

            const decimal expectedCosts = 500.00m;
            var actualCosts = target.CalculateCost(sellDate);
            Assert.AreEqual(expectedCosts, actualCosts);
        }

        /// <summary>
        ///A test for CalculateProceeds
        ///</summary>
        [TestMethod]
        public void CalculateProceedsWithBuyOnlyTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission));

            const decimal expectedProceeds = 0.00m;
            var actualProceeds = target.CalculateProceeds(buyDate);
            Assert.AreEqual(expectedProceeds, actualProceeds);
        }

        /// <summary>
        ///A test for CalculateProceeds
        ///</summary>
        [TestMethod]
        public void CalculateProceedsWithBuyAndSellTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares

            var sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                   _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                   _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission));

            const decimal expectedProceeds = 375.00m;
            var actualProceeds = target.CalculateProceeds(sellDate);
            Assert.AreEqual(expectedProceeds, actualProceeds);
        }

        /// <summary>
        ///A test for CalculateCommissions
        ///</summary>
        [TestMethod]
        public void CalculateCommissionsWithBuyOnlyTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission));

            const decimal expectedCommissions = 5.00m;
            var actualCommissions = target.CalculateCommissions(buyDate);
            Assert.AreEqual(expectedCommissions, actualCommissions);
        }

        /// <summary>
        ///A test for CalculateCommissions
        ///</summary>
        [TestMethod]
        public void CalculateCommissionsWithBuyAndSellTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares

            var sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                   _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                   _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission));

            const decimal expectedCommissions = 10.00m;
            var actualCommissions = target.CalculateCommissions(sellDate);
            Assert.AreEqual(expectedCommissions, actualCommissions);
        }
    }
}
