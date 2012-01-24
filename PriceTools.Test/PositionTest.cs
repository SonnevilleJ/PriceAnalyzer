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
        public void CalculateValueReturnsCorrectWithoutCommissionsOpenPosition()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            target.Buy(oDate, oShares, oPrice, oCommission);

            // Shares are still held, so net value (excluding commissions) is not changed.
            const decimal expected = 0.00m;
            var actual = target.CalculateValue(oDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateValueReturnsCorrectWithoutCommissionsAfterGain()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            target.Buy(oDate, oShares, oPrice, oCommission);

            var cDate = new DateTime(2001, 1, 1);
            const decimal cPrice = 110.00m;     // sold at $110.00 per share
            const double cShares = 5;           // sold 5 shares
            const decimal cCommission = 7.95m;  // sold with $7.95 commission
            target.Sell(cDate, cShares, cPrice, cCommission);

            // No longer hold these shares, so CalculateValue should return total value without any commissions.
            var expected = GetExpectedValue(oShares, oPrice, cShares, cPrice);
            var actual = target.CalculateValue(cDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateValueReturnsCorrectWithoutCommissionsAfterLoss()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            target.Buy(oDate, oShares, oPrice, oCommission);

            var cDate = new DateTime(2001, 1, 1);
            const decimal cPrice = 90.00m;      // sold at $90.00 per share - $10 per share loss
            const double cShares = 5;           // sold 5 shares
            const decimal cCommission = 7.95m;  // sold with $7.95 commission
            target.Sell(cDate, cShares, cPrice, cCommission);

            // No longer hold these shares, so CalculateValue should return total value without any commissions.
            var expected = GetExpectedValue(oShares, oPrice, cShares, cPrice);
            var actual = target.CalculateValue(cDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateTotalValueReturnsCorrectWithCommissionsAfterGain()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            target.Buy(oDate, oShares, oPrice, oCommission);

            var cDate = new DateTime(2001, 1, 1);
            const decimal cPrice = 110.00m;     // sold at $110.00 per share
            const double cShares = 5;           // sold 5 shares
            const decimal cCommission = 7.95m;  // sold with $7.95 commission
            target.Sell(cDate, cShares, cPrice, cCommission);

            // No longer hold these shares, so CalculateTotalValue should return total value minus any commissions.
            var expected = GetExpectedTotalValue(cPrice, cShares, cCommission, oPrice, oShares, oCommission);
            var actual = target.CalculateTotalValue(new YahooPriceDataProvider(), cDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateTotalValueReturnsCorrectWithCommissionsAfterLoss()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            target.Buy(oDate, oShares, oPrice, oCommission);

            var cDate = new DateTime(2001, 1, 1);
            const decimal cPrice = 90.00m;      // sold at $90.00 per share - $10 per share loss
            const double cShares = 5;           // sold 5 shares
            const decimal cCommission = 7.95m;  // sold with $7.95 commission
            target.Sell(cDate, cShares, cPrice, cCommission);

            // No longer hold these shares, so CalculateTotalValue should return total value minus any commissions.
            var expected = GetExpectedTotalValue(cPrice, cShares, cCommission, oPrice, oShares, oCommission);
            var actual = target.CalculateTotalValue(new YahooPriceDataProvider(), cDate);
            Assert.AreEqual(expected, actual);
        }

        private static decimal GetExpectedValue(double openingShares, decimal openingPrice, double closingShares, decimal closingPrice)
        {
            return GetExpectedTotalValue(closingPrice, closingShares, 0.00m, openingPrice, openingShares, 0.00m);
        }

        private static decimal GetExpectedTotalValue(decimal closingPrice, double closingShares, decimal closingCommission, decimal openingPrice, double openingShares, decimal openingCommission)
        {
            return ((closingPrice * (decimal)closingShares) - closingCommission) - ((openingPrice * (decimal)openingShares) + openingCommission);
        }

        [TestMethod]
        public void IndexerReturnsCalculateValue()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            target.Buy(oDate, oShares, oPrice, oCommission);

            var expected = target.CalculateValue(oDate);
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
        public void CalculateInvestedValueTestBuy()
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
            const decimal expected = 229.05m;
            var actual = target.CalculateInvestedValue(new YahooPriceDataProvider(), buyDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateInvestedValueTestSellHalf()
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
            const decimal expected = 224.05m;
            var actual = target.CalculateInvestedValue(new YahooPriceDataProvider(), sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateInvestedValueTestSellAll()
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
            var actual = target.CalculateInvestedValue(new YahooPriceDataProvider(), sellDate);
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

        [TestMethod]
        public void CalculateAverageCostTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            var expectedAverageCost = 50.00m;   // 10 shares at $50.00
            var actualAverageCost = target.CalculateAverageCost(buyDate);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);

            var sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            expectedAverageCost = 50.00m;       // 5 shares at $50.00
            actualAverageCost = target.CalculateAverageCost(sellDate);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);

            var buyDate2 = testDate.AddDays(3);
            const decimal buyPrice2 = 100.00m;  // $100.00 per share
            const double sharesBought2 = 5;     // 5 shares

            target.Buy(buyDate2, sharesBought2, buyPrice2, commission);

            expectedAverageCost = 75.00m;       // 5 shares at $50.00 and 5 shares at $100.00 = $75.00 average cost
            actualAverageCost = target.CalculateAverageCost(buyDate2);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
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
        public void CalculateHoldingsTestWithOneBuyOneSell()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            var sellDate = buyDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(sellDate);

            Assert.AreEqual(1, holdings.Count);

            var expected = new Holding
                               {
                                   Ticker = ticker,
                                   Head = buyDate,
                                   Tail = sellDate,
                                   Shares = sharesSold,
                                   OpenPrice = buyPrice*(decimal) sharesSold,
                                   ClosePrice = sellPrice*(decimal) sharesSold
                               };

            Assert.IsTrue(holdings.Contains(expected));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysOneSellCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 3;      // 3 shares

            target.Buy(firstBuyDate, sharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, sharesBought, buyPrice, commission);

            var sellDate = secondBuyDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 6;        // 6 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(sellDate);

            Assert.AreEqual(2, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysOneSellValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 3;      // 3 shares

            target.Buy(firstBuyDate, sharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, sharesBought, buyPrice, commission);

            var sellDate = secondBuyDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 6;        // 6 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(sellDate);

            var expected1 = new Holding
            {
                Ticker = ticker,
                Head = secondBuyDate,
                Tail = sellDate,
                Shares = sharesBought,
                OpenPrice = buyPrice * (decimal)sharesBought,
                ClosePrice = sellPrice * (decimal)sharesBought
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = sellDate,
                Shares = sharesBought,
                OpenPrice = buyPrice * (decimal)sharesBought,
                ClosePrice = sellPrice * (decimal)sharesBought
            };

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithOneBuyTwoSellsCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            var firstSellDate = buyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithOneBuyTwoSellsValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            var firstSellDate = buyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            const double sharesInHolding = sharesSold;

            var expected1 = new Holding
            {
                Ticker = ticker,
                Head = buyDate,
                Tail = secondSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = buyDate,
                Tail = firstSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 5;      // 5 shares

            target.Buy(firstBuyDate, sharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, sharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 5;      // 5 shares

            target.Buy(firstBuyDate, sharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, sharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate.AddDays(2);
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            const double sharesInHolding = sharesSold;
            var expected1 = new Holding
            {
                Ticker = ticker,
                Head = secondBuyDate,
                Tail = secondSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 5;      // 5 shares

            target.Buy(firstBuyDate, sharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, sharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 5;      // 5 shares

            target.Buy(firstBuyDate, sharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, sharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            const double sharesInHolding = sharesSold;
            var expected1 = new Holding
            {
                Ticker = ticker,
                Head = secondBuyDate,
                Tail = secondSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingUnevenCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double firstSharesBought = 9;
            const double secondSharesBought = 1;

            target.Buy(firstBuyDate, firstSharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, secondSharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(3, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingUnevenValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double firstSharesBought = 9;
            const double secondSharesBought = 1;

            target.Buy(firstBuyDate, firstSharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, secondSharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            var expected1 = new Holding
            {
                Ticker = ticker,
                Head = secondBuyDate,
                Tail = secondSellDate,
                Shares = 1,
                OpenPrice = buyPrice,
                ClosePrice = sellPrice
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = secondSellDate,
                Shares = 4,
                OpenPrice = buyPrice * 4,
                ClosePrice = sellPrice * 4
            };
            var expected3 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = 5,
                OpenPrice = buyPrice * 5,
                ClosePrice = sellPrice * 5
            };


            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
            Assert.IsTrue(holdings.Contains(expected3));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingUnevenSortOrderCount()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double firstSharesBought = 9;
            const double secondSharesBought = 1;

            target.Buy(firstBuyDate, firstSharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, secondSharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(3, holdings.Count);
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoBuysTwoSellsOverlappingUnevenSortOrderValues()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var firstBuyDate = testDate.AddDays(1);
            var secondBuyDate = firstBuyDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double firstSharesBought = 9;
            const double secondSharesBought = 1;

            target.Buy(firstBuyDate, firstSharesBought, buyPrice, commission);
            target.Buy(secondBuyDate, secondSharesBought, buyPrice, commission);

            var firstSellDate = secondBuyDate;
            var secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(firstSellDate, sharesSold, sellPrice, commission);
            target.Sell(secondSellDate, sharesSold, sellPrice, commission);

            var holdings = target.CalculateHoldings(secondSellDate);

            var expected1 = new Holding
            {
                Ticker = ticker,
                Head = secondBuyDate,
                Tail = secondSellDate,
                Shares = 1,
                OpenPrice = buyPrice,
                ClosePrice = sellPrice
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = secondSellDate,
                Shares = 4,
                OpenPrice = buyPrice * 4,
                ClosePrice = sellPrice * 4
            };
            var expected3 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = 5,
                OpenPrice = buyPrice * 5,
                ClosePrice = sellPrice * 5
            };
            var holding1 = holdings[0];
            var holding2 = holdings[1];
            var holding3 = holdings[2];
            Assert.AreEqual(expected1, holding1);
            Assert.AreEqual(expected2, holding2);
            Assert.AreEqual(expected3, holding3);
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

            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, buyPrice, shares, commission);

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

            var sell = TransactionFactory.ConstructSell(buyDate, ticker, buyPrice, shares, commission);

            Assert.IsFalse(target.TransactionIsValid(sell));
        }
    }
}
