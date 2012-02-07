using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PositionCalculationTests
    {
        #region Annual Gross Return

        [TestMethod]
        public void CalculateAnnualGrossReturnAfterLoss()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sellPrice = 90.00m;       // $90.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            target.Buy(buyDate, shares, buyPrice, commission);
            target.Sell(sellDate, shares, sellPrice, commission);

            const decimal expectedReturn = -0.1m;    // -10% return; loss = $50 after commissions; initial investment = $500
            var actualReturn = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = -0.5m;          // -50% annual rate return
            var actual = target.CalculateAnnualGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnAfterGain()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sellPrice = 110.00m;      // $110.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            target.Buy(buyDate, shares, buyPrice, commission);
            target.Sell(sellDate, shares, sellPrice, commission);

            const decimal expectedReturn = 0.1m;    // 10% return; profit = $50 after commissions; initial investment = $500
            var actualReturn = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = 0.5m;          // 50% annual rate return
            var actual = target.CalculateAnnualGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnOpenPosition()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal price = 100.00m;          // $100.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            target.Buy(buyDate, shares, price, commission);

            Assert.IsNull(target.CalculateAnnualGrossReturn(sellDate));
        }

        #endregion
        
        #region Annual Net Return

        [TestMethod]
        public void CalculateAnnualNetReturnAfterLoss()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sellPrice = 92.00m;       // $92.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            target.Buy(buyDate, shares, buyPrice, commission);
            target.Sell(sellDate, shares, sellPrice, commission);

            const decimal expectedReturn = -0.1m;    // -10% return; loss = $50 after commissions; initial investment = $500
            var actualReturn = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = -0.5m;          // -50% annual rate return
            var actual = target.CalculateAnnualNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnAfterGain()
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
            var actual = target.CalculateAnnualNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnOpenPosition()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal price = 100.00m;          // $100.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            target.Buy(buyDate, shares, price, commission);

            Assert.IsNull(target.CalculateAnnualNetReturn(sellDate));
        }

        #endregion
        
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
            target.Buy(buyDate, sharesBought, priceBought, commission);
            target.Sell(sellDate, sharesSold, priceSold, commission);

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
            target.Buy(buyDate, sharesBought, priceBought, commission);
            target.Sell(sellDate, sharesSold, priceSold, commission);
            
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

        #region Holdings Tests

        [TestMethod]
        public void CalculateHoldingsTestWithOneBuyOneSellCount()
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
        }

        [TestMethod]
        public void CalculateHoldingsTestWithOneBuyOneSellValues()
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

            var expected = new Holding
            {
                Ticker = ticker,
                Head = buyDate,
                Tail = sellDate,
                Shares = sharesSold,
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
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
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = sellDate,
                Shares = sharesBought,
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
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
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = buyDate,
                Tail = firstSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
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
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
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
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
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
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = secondSellDate,
                Shares = 4,
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
            };
            var expected3 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = 5,
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
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
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = secondSellDate,
                Shares = 4,
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
            };
            var expected3 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = 5,
                OpenPrice = buyPrice,
                OpenCommission = commission,
                ClosePrice = sellPrice,
                CloseCommission = commission
            };
            var holding1 = holdings[0];
            var holding2 = holdings[1];
            var holding3 = holdings[2];
            Assert.AreEqual(expected1, holding1);
            Assert.AreEqual(expected2, holding2);
            Assert.AreEqual(expected3, holding3);
        }

        #endregion
    }
}
