using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Yahoo;

namespace Sonneville.PriceTools.Test
{
    /// <summary>
    ///This is a test class for PositionTest and is intended
    ///to contain all PositionTest Unit Tests
    ///</summary>
    [TestClass]
    public class PositionTest
    {
        [TestMethod]
        public void IndexerReturnsCalculateGrossProfit()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            target.Buy(oDate, oShares, oPrice, oCommission);

            var expected = target.CalculateGrossProfit(oDate);
            decimal? actual = target[oDate];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTickerThrowsException()
        {
            PositionFactory.CreatePosition(null);
        }

        [TestMethod]
        public void CalculateMarketValueTestBuy()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2000, 12, 29);
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 7.95m;   // with $7.95 commission

            target.Buy(buyDate, shares, price, commission);

            // DE price @ 29 Dec 2000 = $45.81
            // invested value should be $45.81 * 5 shares = $229.05
            const decimal currentPrice = 45.81m;
            const decimal expected = (currentPrice * (decimal) shares);
            var actual = target.CalculateMarketValue(new YahooPriceDataProvider(), buyDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMarketValueTestSellHalf()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 1, 2);
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 10;           // 10 shares
            const decimal commission = 7.95m;   // with $7.95 commission

            target.Buy(buyDate, shares, price, commission);
            target.Sell(sellDate, shares / 2, price + 10m, commission);

            // DE price @ 29 Dec 2000 = $44.81
            // invested value should be $44.81 * 5 shares = $224.05
            const decimal currentPrice = 44.81m;
            const decimal expected = (currentPrice*(decimal) (shares/2));
            var actual = target.CalculateMarketValue(new YahooPriceDataProvider(), sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMarketValueTestSellAll()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 10;           // 10 shares
            const decimal commission = 7.95m;   // with $7.95 commission

            target.Buy(buyDate, shares, price, commission);
            target.Sell(sellDate, shares, price + 10m, commission);

            const decimal expected = 0.00m;     // $0.00 currently invested
            var actual = target.CalculateMarketValue(new YahooPriceDataProvider(), sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SellTooManySharesTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 10;           // 10 shares
            const decimal commission = 7.95m;   // with $7.95 commission

            target.Buy(buyDate, shares, price, commission);
            target.Sell(sellDate, shares + 1, price + 10m, commission);
        }

        [TestMethod]
        public void TransactionCountReturnsCorrectTransactionCount()
        {
            const string longTicker = "DE";
            const string shortTicker = "GM";
            // Must create different positions because all transactions must use same ticker
            var longPosition = PositionFactory.CreatePosition(longTicker);
            var shortPosition = PositionFactory.CreatePosition(shortTicker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;   // $100.00 per share
            const decimal sellPrice = 120.00m;  // $110.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            longPosition.Buy(buyDate, shares, buyPrice, commission);
            longPosition.Sell(sellDate, shares, sellPrice, commission);
            shortPosition.SellShort(buyDate, shares, buyPrice, commission);
            shortPosition.BuyToCover(sellDate, shares, sellPrice, commission);

            const int expected = 2;
            var longActual = longPosition.Transactions.Count;
            var shortActual = shortPosition.Transactions.Count;
            Assert.AreEqual(expected, longActual);
            Assert.AreEqual(expected, shortActual);
        }

        [TestMethod]
        public void HasValueTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var purchaseDate = testDate.AddDays(1);
            const decimal buyPrice = 100.00m;   // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            target.Buy(purchaseDate, shares, buyPrice, commission);

            Assert.AreEqual(false, target.HasValueInRange(testDate));
            Assert.AreEqual(true, target.HasValueInRange(purchaseDate));
            Assert.AreEqual(true, target.HasValueInRange(purchaseDate.AddDays(1)));
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod]
        public void TickerTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

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
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

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
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            var sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

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
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

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
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            var sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

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
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

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
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            var sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            const decimal expectedCommissions = 10.00m;
            var actualCommissions = target.CalculateCommissions(sellDate);
            Assert.AreEqual(expectedCommissions, actualCommissions);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ValuesTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var values = target.Values;
        }

        [TestMethod]
        public void ResolutionEqualsResolutionOfPriceSeriesTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var expected = PriceSeriesFactory.CreatePriceSeries(ticker).Resolution;
            var actual = target.Resolution;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TransactionIsValidBuyWithSufficientCash()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2011, 12, 25);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double shares = 9;

            var buy = TransactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission);

            Assert.IsTrue(target.TransactionIsValid(buy));
        }

        [TestMethod]
        public void TransactionIsValidFalse()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2011, 12, 25);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double shares = 9;

            var sell = TransactionFactory.ConstructSell(ticker, buyDate, shares, buyPrice, commission);

            Assert.IsFalse(target.TransactionIsValid(sell));
        }
    }
}
