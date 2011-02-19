using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{


    /// <summary>
    ///This is a test class for PositionTest and is intended
    ///to contain all PositionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PositionTest
    {
        [TestMethod()]
        public void GetValueReturnsCorrectProfitWithoutCommissions()
        {
            const string ticker = "DE";
            IPosition target = new Position(ticker);

            DateTime oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;      // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            target.Buy(oDate, oShares, oPrice, oCommission);

            DateTime cDate = new DateTime(2001, 1, 1);
            const decimal cPrice = 110.00m;      // sold at $110.00 per share
            const double cShares = 5;           // sold 5 shares
            const decimal cCommission = 7.95m;  // sold with $7.95 commission
            target.Sell(cDate, cShares, cPrice, cCommission);

            // No longer hold these shares, so GetValue should return total profit (or negative loss) minus any commissions.
            const decimal expected = 550.00m;   // sold all shares for $550.00
            decimal actual = target.GetValue(cDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullTickerThrowsException()
        {
            new Position(null);
        }

        [TestMethod]
        public void RawReturnTest()
        {
            const string ticker = "DE";
            IPosition target = new Position(ticker);

            DateTime buyDate = new DateTime(2001, 1, 1);
            DateTime sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 7.95m;   // with $7.95 commission

            target.Buy(buyDate, shares, price, commission);
            target.Sell(sellDate, shares, price + 10m, commission);

            const decimal expected = 0.1m;      // 10% raw return on investment
            decimal actual = target.GetRawReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TotalReturnTest()
        {
            const string ticker = "DE";
            IPosition target = new Position(ticker);

            DateTime buyDate = new DateTime(2001, 1, 1);
            DateTime sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission

            target.Buy(buyDate, shares, price, commission);
            target.Sell(sellDate, shares, price + 2.00m, commission);
            
            const decimal expected = 0.00m;      // 0% return; 100% of original investment
            decimal actual = target.GetTotalReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TotalAnnualReturnTest()
        {
            const string ticker = "DE";
            IPosition target = new Position(ticker);

            DateTime buyDate = new DateTime(2001, 1, 1);
            DateTime sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;        // $100.00 per share
            const decimal sellPrice = 112.00m;       // $102.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;        // with $5 commission

            target.Buy(buyDate, shares, buyPrice, commission);
            target.Sell(sellDate, shares, sellPrice, commission);

            const decimal expectedReturn = 0.1m;    // 10% return; profit = $50 after commissions; initial investment = $500
            decimal actualReturn = target.GetTotalReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = 0.5m;          // 50% annual rate return
            decimal actual = target.GetTotalAnnualReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TransactionCountReturnsCorrectTransactionCount()
        {
            const string longTicker = "DE";
            const string shortTicker = "GM";
            // Must create different positions because all transactions must use same ticker
            IPosition longPosition = new Position(longTicker);
            IPosition shortPosition = new Position(shortTicker);

            DateTime buyDate = new DateTime(2001, 1, 1);
            DateTime sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;    // $100.00 per share
            const decimal sellPrice = 120.00m;   // $110.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.00m;    // with $5 commission

            longPosition.Buy(buyDate, shares, buyPrice, commission);
            longPosition.Sell(sellDate, shares, sellPrice, commission);
            shortPosition.SellShort(buyDate, shares, buyPrice, commission);
            shortPosition.BuyToCover(sellDate, shares, sellPrice, commission);

            const int expected = 2;
            int longActual = longPosition.Transactions.Count;
            int shortActual = shortPosition.Transactions.Count;
            Assert.AreEqual(expected, longActual);
            Assert.AreEqual(expected, shortActual);
        }

        [TestMethod]
        public void HasValueTest()
        {
            const string ticker = "DE";
            IPosition target = new Position(ticker);

            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime purchaseDate = testDate.AddDays(1);
            const decimal buyPrice = 100.00m;    // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.00m;    // with $5 commission

            target.Buy(purchaseDate, shares, buyPrice, commission);

            Assert.AreEqual(false, target.HasValue(testDate));
            Assert.AreEqual(true, target.HasValue(purchaseDate));
            Assert.AreEqual(true, target.HasValue(purchaseDate.AddDays(1)));
        }

        [TestMethod]
        public void AverageCostTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            IPosition target = new Position(ticker);

            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;     // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            decimal expectedAverageCost = 50.00m;   // 10 shares at $50.00
            decimal actualAverageCost = target.GetAverageCost(buyDate);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);

            DateTime sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;    // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            expectedAverageCost = 50.00m;       // 5 shares at $50.00
            actualAverageCost = target.GetAverageCost(sellDate);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);

            DateTime buyDate2 = testDate.AddDays(3);
            const decimal buyPrice2 = 100.00m;   // $100.00 per share
            const double sharesBought2 = 5;     // 5 shares

            target.Buy(buyDate2, sharesBought2, buyPrice2, commission);

            expectedAverageCost = 75.00m;       // 5 shares at $50.00 and 5 shares at $100.00 = $75.00 average cost
            actualAverageCost = target.GetAverageCost(buyDate2);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }

        [TestMethod]
        public void SerializePositionTest()
        {
            const string ticker = "DE";
            IPosition target = new Position(ticker);

            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime purchaseDate = testDate.AddDays(1);
            const decimal buyPrice = 100.00m;    // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.00m;    // with $5 commission

            target.Buy(purchaseDate, shares, buyPrice, commission);

            decimal expected = target.GetValue(purchaseDate);
            decimal actual = ((IPosition) TestUtilities.Serialize(target)).GetValue(purchaseDate);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod()]
        public void TickerTest()
        {
            const string ticker = "DE";
            IPosition target = new Position(ticker);

            const string expectedTicker = ticker;
            string actualTicker = target.Ticker;
            Assert.AreEqual(expectedTicker, actualTicker);
        }

        /// <summary>
        ///A test for GetCost
        ///</summary>
        [TestMethod()]
        public void GetCostWithBuyOnlyTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            IPosition target = new Position(ticker);

            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            const decimal expectedCosts = 500.00m;
            decimal actualCosts = target.GetCost(buyDate);
            Assert.AreEqual(expectedCosts, actualCosts);
        }

        /// <summary>
        ///A test for GetCost
        ///</summary>
        [TestMethod()]
        public void GetCostWithBuyAndSellTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            IPosition target = new Position(ticker);

            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            DateTime sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            const decimal expectedCosts = 500.00m;
            decimal actualCosts = target.GetCost(sellDate);
            Assert.AreEqual(expectedCosts, actualCosts);
        }

        /// <summary>
        ///A test for GetProceeds
        ///</summary>
        [TestMethod()]
        public void GetProceedsWithBuyOnlyTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            IPosition target = new Position(ticker);

            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            const decimal expectedProceeds = 0.00m;
            decimal actualProceeds = target.GetProceeds(buyDate);
            Assert.AreEqual(expectedProceeds, actualProceeds);
        }

        /// <summary>
        ///A test for GetProceeds
        ///</summary>
        [TestMethod()]
        public void GetProceedsWithBuyAndSellTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            IPosition target = new Position(ticker);

            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            DateTime sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            const decimal expectedProceeds = 375.00m;
            decimal actualProceeds = target.GetProceeds(sellDate);
            Assert.AreEqual(expectedProceeds, actualProceeds);
        }

        /// <summary>
        ///A test for GetCommissions
        ///</summary>
        [TestMethod()]
        public void GetCommissionsWithBuyOnlyTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            IPosition target = new Position(ticker);

            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            const decimal expectedCommissions = 5.00m;
            decimal actualCommissions = target.GetCommissions(buyDate);
            Assert.AreEqual(expectedCommissions, actualCommissions);
        }

        /// <summary>
        ///A test for GetCommissions
        ///</summary>
        [TestMethod()]
        public void GetCommissionsWithBuyAndSellTest()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            IPosition target = new Position(ticker);

            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            DateTime sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            const decimal expectedCommissions = 10.00m;
            decimal actualCommissions = target.GetCommissions(sellDate);
            Assert.AreEqual(expectedCommissions, actualCommissions);
        }
    }
}
