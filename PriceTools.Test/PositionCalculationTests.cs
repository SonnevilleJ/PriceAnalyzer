using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PositionCalculationTests
    {
        [TestMethod]
        public void PositionCalculateAverageAnnualReturnTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sellPrice = 112.00m;      // $112.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            target.Buy(buyDate, shares, buyPrice, commission);
            target.Sell(sellDate, shares, sellPrice, commission);

            const decimal expectedReturn = 0.1m;    // 10% return; profit = $50 after commissions; initial investment = $500
            var actualReturn = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = 0.5m;          // 50% annual rate return
            var actual = target.CalculateAverageAnnualReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateAverageAnnualReturnWithoutProceedsTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal price = 100.00m;          // $100.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            target.Buy(buyDate, shares, price, commission);

            Assert.IsNull(target.CalculateAverageAnnualReturn(sellDate));
        }
        
        #region Average Cost

        [TestMethod]
        public void CalculateAverageCostBuy()
        {
            const string ticker = "DE";
            const decimal commission = 5.00m;   // with $5 commission
            var target = PositionFactory.CreatePosition(ticker);

            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 10;     // 10 shares

            target.Buy(buyDate, sharesBought, buyPrice, commission);

            const decimal expectedAverageCost = buyPrice;
            var actualAverageCost = target.CalculateAverageCost(buyDate);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }

        [TestMethod]
        public void CalculateAverageCostBuySell()
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

            const decimal expectedAverageCost = buyPrice;
            var actualAverageCost = target.CalculateAverageCost(buyDate);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }

        [TestMethod]
        public void CalculateAverageCostBuySellBuyHigher()
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

            var buyDate2 = testDate.AddDays(3);
            const decimal buyPrice2 = 100.00m;  // $100.00 per share
            const double sharesBought2 = 5;     // 5 shares

            target.Buy(buyDate2, sharesBought2, buyPrice2, commission);

            const double originalShares = sharesBought - sharesSold;
            const double newShares = sharesBought2;
            const decimal expectedAverageCost = (((decimal)originalShares * buyPrice) + (decimal)newShares * buyPrice2) / (decimal)(originalShares + newShares);
            var actualAverageCost = target.CalculateAverageCost(buyDate2);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }

        [TestMethod]
        public void CalculateAverageCostBuySellBuyLower()
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
            const decimal sellPrice = 25.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.Sell(sellDate, sharesSold, sellPrice, commission);

            var buyDate2 = testDate.AddDays(3);
            const decimal buyPrice2 = 20.00m;  // $100.00 per share
            const double sharesBought2 = 10;     // 5 shares

            target.Buy(buyDate2, sharesBought2, buyPrice2, commission);

            const double originalShares = sharesBought - sharesSold;
            const double newShares = sharesBought2;
            const decimal expectedAverageCost = (((decimal)originalShares * buyPrice) + (decimal)newShares * buyPrice2) / (decimal) (originalShares + newShares);
            var actualAverageCost = target.CalculateAverageCost(buyDate2);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }

        #endregion

        #region Gross Return

        [TestMethod]
        public void PositionCalculateGrossReturnAfterLoss()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const decimal priceBought = 100.00m;    // $100.00 per share
            const double sharesBought = 10;         // 10 shares
            const decimal commission = 7.95m;       // with $7.95 commission
            const decimal decrease = -0.10m;        // 10% price decrease when sold
            const decimal priceSold = priceBought * (1 + decrease);
            const double sharesSold = sharesBought - 2;
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, priceBought, sharesBought, commission);
            var sell = TransactionFactory.ConstructSell(sellDate, ticker, priceSold, sharesSold, commission);

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = decrease;
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateGrossReturnAfterGain()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const decimal priceBought = 100.00m;    // $100.00 per share
            const double sharesBought = 10;         // 10 shares
            const decimal commission = 7.95m;       // with $7.95 commission
            const decimal increase = 0.10m;         // 10% price increase when sold
            const decimal priceSold = priceBought * (1 + increase);
            const double sharesSold = sharesBought - 2;
            var buy = TransactionFactory.ConstructBuy(buyDate, ticker, priceBought, sharesBought, commission);
            var sell = TransactionFactory.ConstructSell(sellDate, ticker, priceSold, sharesSold, commission);

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = increase;
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossReturnOpenPosition()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 7.95m;   // with $7.95 commission

            target.Buy(buyDate, shares, price, commission);

            Assert.IsNull(target.CalculateGrossReturn(sellDate));
        }

        #endregion

        #region Net Return

        [TestMethod]
        public void PositionCalculateNetReturnAfterLoss()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission

            target.Buy(buyDate, shares, price, commission);
            target.Sell(sellDate, shares, price - 2.00m, commission);

            const decimal expected = -0.04m;      // -4% return; 96% of original investment
            var actual = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateNetReturnAfterGain()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission

            target.Buy(buyDate, shares, price, commission);
            target.Sell(sellDate, shares, price * 2m, commission);

            const decimal expected = 0.98m;      // 98% return; 198% of original investment
            var actual = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateNetReturnOpenPosition()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission

            target.Buy(buyDate, shares, price, commission);

            Assert.IsNull(target.CalculateNetReturn(sellDate));
        }

        #endregion

        #region Gross Profit

        [TestMethod]
        public void CalculateGrossProfitOpenPosition()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            target.Buy(oDate, oShares, oPrice, oCommission);

            // CalculateGrossProfit does not consider open positions - it can only account for closed holdings
            const decimal expected = 0;
            var actual = target.CalculateGrossProfit(oDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitAfterGain()
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

            // No longer hold these shares, so CalculateGrossProfit should return total value without any commissions.
            var expected = GetExpectedGrossProfit(oShares, oPrice, cShares, cPrice);
            var actual = target.CalculateGrossProfit(cDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitAfterLoss()
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

            // No longer hold these shares, so CalculateGrossProfit should return total value without any commissions.
            var expected = GetExpectedGrossProfit(oShares, oPrice, cShares, cPrice);
            var actual = target.CalculateGrossProfit(cDate);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Net Profit

        [TestMethod]
        public void CalculateNetProfitOpenPosition()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            target.Buy(oDate, oShares, oPrice, oCommission);

            // CalculateGrossProfit does not consider open positions - it can only account for closed holdings
            const decimal expected = 0;
            var actual = target.CalculateNetProfit(oDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitAfterGain()
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

            // No longer hold these shares, so CalculateNetProfit should return total profit with all commissions.
            var expected = GetExpectedNetProfit(oShares, oPrice, oCommission, cShares, cPrice, oCommission);
            var actual = target.CalculateNetProfit(cDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitAfterLoss()
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

            // No longer hold these shares, so CalculateNetProfit should return total profit with all commissions.
            var expected = GetExpectedNetProfit(oShares, oPrice, oCommission, cShares, cPrice, oCommission);
            var actual = target.CalculateNetProfit(cDate);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        /// <summary>
        /// Calculates the expected result of a call to CalculateNetProfit on a single Position.
        /// </summary>
        private static decimal GetExpectedNetProfit(double openingShares, decimal openingPrice, decimal openingCommission, double closingShares, decimal closingPrice, decimal closingCommission)
        {
            return ((closingPrice * (decimal)closingShares) - closingCommission) - ((openingPrice * (decimal)openingShares) + openingCommission);
        }

        /// <summary>
        /// Calculates the expected result of a call to CalculateGrossProfit on a single Position.
        /// </summary>
        private static decimal GetExpectedGrossProfit(double openingShares, decimal openingPrice, double closingShares, decimal closingPrice)
        {
            return (closingPrice * (decimal)closingShares) - (openingPrice * (decimal)openingShares);
        }
    }
}
