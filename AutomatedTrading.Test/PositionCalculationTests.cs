using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Implementation;
using Statistics;

namespace Test.Sonneville.PriceTools.AutomatedTrading
{
    [TestClass]
    public class PositionCalculationTests
    {
        private readonly IPositionFactory _positionFactory;
        private readonly ITransactionFactory _transactionFactory;

        public PositionCalculationTests()
        {
            _positionFactory = new PositionFactory();
            _transactionFactory = new TransactionFactory();
        }

        #region Annual Gross Return

        [TestMethod]
        public void CalculateAnnualGrossReturnOneLoss()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sellPrice = 90.00m;       // $90.00 per share
            const decimal shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            const decimal expectedReturn = -0.1m;    // -10% return; loss = $50 after commissions; initial investment = $500
            var actualReturn = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = -0.5m;          // -50% annual rate return
            var actual = target.CalculateAnnualGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnOneGain()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sellPrice = 110.00m;      // $110.00 per share
            const decimal shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

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
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal price = 100.00m;          // $100.00 per share
            const decimal shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission));

            Assert.IsNull(target.CalculateAnnualGrossReturn(sellDate));
        }

        #endregion
        
        #region Annual Net Return

        [TestMethod]
        public void CalculateAnnualNetReturnOneLoss()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sellPrice = 92.00m;       // $92.00 per share
            const decimal shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            const decimal expectedReturn = -0.1m;    // -10% return; loss = $50 after commissions; initial investment = $500
            var actualReturn = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = -0.5m;          // -50% annual rate return
            var actual = target.CalculateAnnualNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualNetReturnOneGain()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sellPrice = 112.00m;      // $112.00 per share
            const decimal shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

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
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal price = 100.00m;          // $100.00 per share
            const decimal shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission));

            Assert.IsNull(target.CalculateAnnualNetReturn(sellDate));
        }

        #endregion
        
        #region Average Cost

        [TestMethod]
        public void CalculateAverageCostBuy()
        {
            const string ticker = "DE";
            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares
            const decimal commission = 5.00m;   // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission));

            const decimal expectedAverageCost = buyPrice;
            var actualAverageCost = target.CalculateAverageCost(buyDate);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }

        [TestMethod]
        public void CalculateAverageCostBuySell()
        {
            const string ticker = "DE";
            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares
            const decimal commission = 5.00m;   // with $5 commission
            var sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission));

            const decimal expectedAverageCost = buyPrice;
            var actualAverageCost = target.CalculateAverageCost(buyDate);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }

        [TestMethod]
        public void CalculateAverageCostBuySellBuyHigher()
        {
            const string ticker = "DE";
            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares
            const decimal commission = 5.00m;   // with $5 commission

            var sellDate = testDate.AddDays(2);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var buyDate2 = testDate.AddDays(3);
            const decimal buyPrice2 = 100.00m;  // $100.00 per share
            const decimal sharesBought2 = 5;     // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission),
                                                           _transactionFactory.ConstructBuy(ticker, buyDate2, sharesBought2, buyPrice2, commission));

            const decimal originalShares = sharesBought - sharesSold;
            const decimal newShares = sharesBought2;
            const decimal expectedAverageCost = ((originalShares * buyPrice) + newShares * buyPrice2) / (originalShares + newShares);
            var actualAverageCost = target.CalculateAverageCost(buyDate2);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }

        [TestMethod]
        public void CalculateAverageCostBuySellBuyLower()
        {
            const string ticker = "DE";
            var testDate = new DateTime(2001, 1, 1);
            var buyDate = testDate.AddDays(1);
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const decimal sharesBought = 10;     // 10 shares
            const decimal commission = 5.00m;   // with $5 commission

            var sellDate = testDate.AddDays(2);
            const decimal sellPrice = 25.00m;   // $75.00 per share
            const decimal sharesSold = 5;        // 5 shares

            var buyDate2 = testDate.AddDays(3);
            const decimal buyPrice2 = 20.00m;  // $100.00 per share
            const decimal sharesBought2 = 10;     // 5 shares

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                                           _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission),
                                                           _transactionFactory.ConstructBuy(ticker, buyDate2, sharesBought2, buyPrice2, commission));

            const decimal originalShares = sharesBought - sharesSold;
            const decimal newShares = sharesBought2;
            const decimal expectedAverageCost = ((originalShares * buyPrice) + newShares * buyPrice2) / (originalShares + newShares);
            var actualAverageCost = target.CalculateAverageCost(buyDate2);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }

        #endregion

        #region Gross Return

        [TestMethod]
        public void PositionCalculateGrossReturnOneLoss()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sharesBought = 10;        // 10 shares
            const decimal commission = 7.95m;       // with $7.95 commission
            const decimal decrease = -0.10m;        // 10% price decrease when sold
            const decimal sellPrice = buyPrice * (1 + decrease);
            const decimal sharesSold = sharesBought - 2;

            var target = _positionFactory.ConstructPosition(ticker,
                                               _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                               _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission));

            const decimal expected = decrease;
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateGrossReturnOneGain()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sharesBought = 10;        // 10 shares
            const decimal commission = 7.95m;       // with $7.95 commission
            const decimal increase = 0.10m;         // 10% price increase when sold
            const decimal sellPrice = buyPrice * (1 + increase);
            const decimal sharesSold = sharesBought - 2;

            var target = _positionFactory.ConstructPosition(ticker,
                                               _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, buyPrice, commission),
                                               _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, sellPrice, commission));

            const decimal expected = increase;
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossReturnOpenPosition()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;          // $100.00 per share
            const decimal shares = 5;               // 5 shares
            const decimal commission = 7.95m;       // with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission));

            Assert.IsNull(target.CalculateGrossReturn(sellDate));
        }

        #endregion

        #region Net Return

        [TestMethod]
        public void PositionCalculateNetReturnOneLoss()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal shares = 5;               // 5 shares
            const decimal commission = 5.00m;       // with $5 commission
            const decimal sellPrice = buyPrice - 2.00m;

            var target = _positionFactory.ConstructPosition(ticker,
                                               _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission),
                                               _transactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            const decimal expected = -0.04m;        // -4% return; 96% of original investment
            var actual = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateNetReturnOneGain()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal shares = 5;               // 5 shares
            const decimal commission = 5.00m;       // with $5 commission
            const decimal sellPrice = buyPrice*2m;

            var target = _positionFactory.ConstructPosition(ticker,
                                               _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission),
                                               _transactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

            const decimal expected = 0.98m;         // 98% return; 198% of original investment
            var actual = target.CalculateNetReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateNetReturnOpenPosition()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;       // $100.00 per share
            const decimal shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission));

            Assert.IsNull(target.CalculateNetReturn(sellDate));
        }

        #endregion

        #region Gross Profit

        [TestMethod]
        public void CalculateGrossProfitOpenPosition()
        {
            const string ticker = "DE";
            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const decimal oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, oDate, oShares, oPrice, oCommission));

            // CalculateGrossProfit does not consider open positions - it can only account for closed holdings
            const decimal expected = 0;
            var actual = target.CalculateGrossProfit(oDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitOneGain()
        {
            const string ticker = "DE";
            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const decimal oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission

            var cDate = new DateTime(2001, 1, 1);
            const decimal cPrice = 110.00m;     // sold at $110.00 per share
            const decimal cShares = 5;           // sold 5 shares
            const decimal cCommission = 7.95m;  // sold with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                           _transactionFactory.ConstructBuy(ticker, oDate, oShares, oPrice, oCommission),
                                                           _transactionFactory.ConstructSell(ticker, cDate, cShares, cPrice, cCommission));

            // No longer hold these shares, so CalculateGrossProfit should return total value without any commissions.
            var expected = GetExpectedGrossProfit(oPrice, cShares, cPrice);
            var actual = target.CalculateGrossProfit(cDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitOneLoss()
        {
            const string ticker = "DE";
            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const decimal oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission

            var cDate = new DateTime(2001, 1, 1);
            const decimal cPrice = 90.00m;      // sold at $90.00 per share - $10 per share loss
            const decimal cShares = 5;           // sold 5 shares
            const decimal cCommission = 7.95m;  // sold with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                               _transactionFactory.ConstructBuy(ticker, oDate, oShares, oPrice, oCommission),
                                               _transactionFactory.ConstructSell(ticker, cDate, cShares, cPrice, cCommission));

            // No longer hold these shares, so CalculateGrossProfit should return total value without any commissions.
            var expected = GetExpectedGrossProfit(oPrice, cShares, cPrice);
            var actual = target.CalculateGrossProfit(cDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitTwoGains()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 60.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var firstProfit = GetExpectedGrossProfit(firstPriceBought, sharesSold, firstPriceSold);
            var secondProfit = GetExpectedGrossProfit(secondPriceBought, sharesSold, secondPriceSold);

            var expected = firstProfit + secondProfit;
            var actual = target.CalculateGrossProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitTwoGainsFIFO()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 60.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            // both holdings will use the original shares, so both must use firstPriceBought
            var firstProfit = GetExpectedGrossProfit(firstPriceBought, sharesSold, firstPriceSold);
            var secondProfit = GetExpectedGrossProfit(firstPriceBought, sharesSold, secondPriceSold);

            var expected = firstProfit + secondProfit;
            var actual = target.CalculateGrossProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitOneGainOneLoss()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 40.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var firstProfit = GetExpectedGrossProfit(firstPriceBought, sharesSold, firstPriceSold);
            var secondProfit = GetExpectedGrossProfit(secondPriceBought, sharesSold, secondPriceSold);

            var expected = firstProfit + secondProfit;
            var actual = target.CalculateGrossProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitTwoLosses()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 90.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 40.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var firstProfit = GetExpectedGrossProfit(firstPriceBought, sharesSold, firstPriceSold);
            var secondProfit = GetExpectedGrossProfit(secondPriceBought, sharesSold, secondPriceSold);

            var expected = firstProfit + secondProfit;
            var actual = target.CalculateGrossProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossProfitTwoLossesFIFO()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 90.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 40.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            // both holdings will use the original shares, so both must use firstPriceBought
            var firstProfit = GetExpectedGrossProfit(firstPriceBought, sharesSold, firstPriceSold);
            var secondProfit = GetExpectedGrossProfit(firstPriceBought, sharesSold, secondPriceSold);

            var expected = firstProfit + secondProfit;
            var actual = target.CalculateGrossProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Net Profit

        [TestMethod]
        public void CalculateNetProfitOpenPosition()
        {
            const string ticker = "DE";
            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const decimal oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, oDate, oShares, oPrice, oCommission));

            // CalculateGrossProfit does not consider open positions - it can only account for closed holdings
            const decimal expected = 0;
            var actual = target.CalculateNetProfit(oDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitOneGain()
        {
            const string ticker = "DE";
            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const decimal oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission

            var cDate = new DateTime(2001, 1, 1);
            const decimal cPrice = 110.00m;     // sold at $110.00 per share
            const decimal cShares = 5;           // sold 5 shares
            const decimal cCommission = 7.95m;  // sold with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                               _transactionFactory.ConstructBuy(ticker, oDate, oShares, oPrice, oCommission),
                                               _transactionFactory.ConstructSell(ticker, cDate, cShares, cPrice, cCommission));

            // No longer hold these shares, so CalculateNetProfit should return total profit with all commissions.
            var expected = GetExpectedNetProfit(oPrice, oCommission, cShares, cPrice, oCommission);
            var actual = target.CalculateNetProfit(cDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitOneLoss()
        {
            const string ticker = "DE";
            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m;     // bought at $100.00 per share
            const decimal oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission

            var cDate = new DateTime(2001, 1, 1);
            const decimal cPrice = 90.00m;      // sold at $90.00 per share - $10 per share loss
            const decimal cShares = 5;           // sold 5 shares
            const decimal cCommission = 7.95m;  // sold with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                               _transactionFactory.ConstructBuy(ticker, oDate, oShares, oPrice, oCommission),
                                               _transactionFactory.ConstructSell(ticker, cDate, cShares, cPrice, cCommission));

            // No longer hold these shares, so CalculateNetProfit should return total profit with all commissions.
            var expected = GetExpectedNetProfit(oPrice, oCommission, cShares, cPrice, oCommission);
            var actual = target.CalculateNetProfit(cDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitTwoGains()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 60.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var firstProfit = GetExpectedNetProfit(firstPriceBought, commission, sharesSold, firstPriceSold, commission);
            var secondProfit = GetExpectedNetProfit(secondPriceBought, commission, sharesSold, secondPriceSold, commission);

            var expected = firstProfit + secondProfit;
            var actual = target.CalculateNetProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitTwoGainsFIFO()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 60.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            // both holdings will use the original shares, so both must use firstPriceBought
            var firstProfit = GetExpectedNetProfit(firstPriceBought, commission, sharesSold, firstPriceSold, commission);
            var secondProfit = GetExpectedNetProfit(firstPriceBought, commission, sharesSold, secondPriceSold, commission);

            var expected = firstProfit + secondProfit;
            var actual = target.CalculateNetProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitOneGainOneLoss()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 40.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var firstProfit = GetExpectedNetProfit(firstPriceBought, commission, sharesSold, firstPriceSold, commission);
            var secondProfit = GetExpectedNetProfit(secondPriceBought, commission, sharesSold, secondPriceSold, commission);

            var expected = firstProfit + secondProfit;
            var actual = target.CalculateNetProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitTwoLosses()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 90.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 40.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var firstProfit = GetExpectedNetProfit(firstPriceBought, commission, sharesSold, firstPriceSold, commission);
            var secondProfit = GetExpectedNetProfit(secondPriceBought, commission, sharesSold, secondPriceSold, commission);

            var expected = firstProfit + secondProfit;
            var actual = target.CalculateNetProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitTwoLossesFIFO()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 90.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 40.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            // both holdings will use the original shares, so both must use firstPriceBought
            var firstProfit = GetExpectedNetProfit(firstPriceBought, commission, sharesSold, firstPriceSold, commission);
            var secondProfit = GetExpectedNetProfit(firstPriceBought, commission, sharesSold, secondPriceSold, commission);

            var expected = firstProfit + secondProfit;
            var actual = target.CalculateNetProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Average Profit

        [TestMethod]
        public void CalculateAverageProfitOneGain()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell);

            var firstProfit = GetExpectedGrossProfit(firstPriceBought, sharesSold, firstPriceSold);

            var expected = (firstProfit) / 1;
            var actual = target.CalculateAverageProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAverageProfitOneLoss()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 90.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell);

            var firstProfit = GetExpectedGrossProfit(firstPriceBought, sharesSold, firstPriceSold);

            var expected = (firstProfit) / 1;
            var actual = target.CalculateAverageProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAverageProfitTwoGains()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 60.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var firstProfit = GetExpectedGrossProfit(firstPriceBought, sharesSold, firstPriceSold);
            var secondProfit = GetExpectedGrossProfit(secondPriceBought, sharesSold, secondPriceSold);

            var expected = (firstProfit + secondProfit) / 2;
            var actual = target.CalculateAverageProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAverageProfitOneGainOneLoss()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 40.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var firstProfit = GetExpectedGrossProfit(firstPriceBought, sharesSold, firstPriceSold);
            var secondProfit = GetExpectedGrossProfit(secondPriceBought, sharesSold, secondPriceSold);

            var expected = (firstProfit + secondProfit) / 2;
            var actual = target.CalculateAverageProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAverageProfitTwoLosses()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 90.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 40.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var firstProfit = GetExpectedGrossProfit(firstPriceBought, sharesSold, firstPriceSold);
            var secondProfit = GetExpectedGrossProfit(secondPriceBought, sharesSold, secondPriceSold);
            
            var expected = (firstProfit + secondProfit) / 2;
            var actual = target.CalculateAverageProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Median Profit

        [TestMethod]
        public void CalculateMedianProfitOneGain()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(sellDate));
            var actual = target.CalculateMedianProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitOneLoss()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 90.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(sellDate));
            var actual = target.CalculateMedianProfit(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitTwoGains()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 55.00m;
            const decimal secondPriceSold = 60.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(secondSellDate));
            var actual = target.CalculateMedianProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitOneGainOneLoss()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 40.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(secondSellDate));
            var actual = target.CalculateMedianProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitTwoLosses()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 90.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 40.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(sellDate));
            var actual = target.CalculateMedianProfit(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMedianProfitThreeHoldings()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            var thirdSellDate = secondSellDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 60.00m;
            const decimal thirdPriceSold = 90.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);
            var thirdSell = _transactionFactory.ConstructSell(de, thirdSellDate, ((sharesBought*2) - (sharesSold*2)), thirdPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell, thirdSell);

            var expected = GetExpectedMedianProfit(target.CalculateHoldings(thirdSellDate));
            var actual = target.CalculateMedianProfit(thirdSellDate);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Standard Deviation

        [TestMethod]
        public void CalculateStandardDeviationOneGain()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell);

            var expected = GetExpectedStandardDeviation(target.CalculateHoldings(sellDate));
            var actual = target.CalculateStandardDeviation(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateStandardDeviationOneLoss()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 90.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell);

            var expected = GetExpectedStandardDeviation(target.CalculateHoldings(sellDate));
            var actual = target.CalculateStandardDeviation(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateStandardDeviationTwoGains()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 55.00m;
            const decimal secondPriceSold = 60.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var expected = GetExpectedStandardDeviation(target.CalculateHoldings(secondSellDate));
            var actual = target.CalculateStandardDeviation(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateStandardDeviationOneGainOneLoss()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 40.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var expected = GetExpectedStandardDeviation(target.CalculateHoldings(secondSellDate));
            var actual = target.CalculateStandardDeviation(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateStandardDeviationTwoLosses()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 90.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 40.00m;
            const decimal sharesBought = 5;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var expected = GetExpectedStandardDeviation(target.CalculateHoldings(sellDate));
            var actual = target.CalculateStandardDeviation(secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateStandardDeviationThreeHoldings()
        {
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            var secondBuyDate = sellDate.AddDays(1);
            var secondSellDate = secondBuyDate.AddDays(1);
            var thirdSellDate = secondSellDate.AddDays(1);
            const string de = "DE";
            const decimal firstPriceBought = 100.00m;
            const decimal firstPriceSold = 110.00m;
            const decimal secondPriceBought = 50.00m;
            const decimal secondPriceSold = 60.00m;
            const decimal thirdPriceSold = 90.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = 5;

            var firstBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, firstPriceBought, commission);
            var firstSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, firstPriceSold, commission);
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought, commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold, commission);
            var thirdSell = _transactionFactory.ConstructSell(de, thirdSellDate, ((sharesBought*2) - (sharesSold*2)), thirdPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell, thirdSell);

            var expected = GetExpectedStandardDeviation(target.CalculateHoldings(thirdSellDate));
            var actual = target.CalculateStandardDeviation(thirdSellDate);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Calculates the expected result of a call to CalculateStandardDeviation on a <see cref="ISecurityBasket"/>.
        /// </summary>
        /// <param name="holdings"></param>
        /// <returns></returns>
        private static decimal GetExpectedStandardDeviation(IEnumerable<Holding> holdings)
        {
            var values = holdings.Select(h => h.GrossProfit()).ToArray();
            if (values.Count() <= 1) return 0;

            var average = values.Average();
            var squares = values.Select(value => (value - average) * (value - average));
            var sum = squares.Sum();
            return ((sum / values.Count()) - 1).SquareRoot();
        }

        /// <summary>
        /// Calculates the expected result of a call to CalculateMedianProfit on a <see cref="ISecurityBasket"/>.
        /// </summary>
        /// <param name="holdings"></param>
        /// <returns></returns>
        private static decimal GetExpectedMedianProfit(IEnumerable<Holding> holdings)
        {
            var list = holdings.OrderBy(holding => holding.GrossProfit()).ToList();
            if (list.Count == 0) return 0.00m;

            var midpoint = (list.Count / 2);
            if (list.Count % 2 == 0)
            {
                return (list[midpoint - 1].GrossProfit() + list[midpoint].GrossProfit()) / 2;
            }
            return list[midpoint].GrossProfit();
        }

        /// <summary>
        /// Calculates the expected result of a call to CalculateNetProfit on a single Position.
        /// </summary>
        private static decimal GetExpectedNetProfit(decimal openingPrice, decimal openingCommission, decimal closingShares, decimal closingPrice, decimal closingCommission)
        {
            return ((closingPrice - openingPrice ) * closingShares) - openingCommission - closingCommission;
        }

        /// <summary>
        /// Calculates the expected result of a call to CalculateGrossProfit on a single Position.
        /// </summary>
        private static decimal GetExpectedGrossProfit(decimal openingPrice, decimal shares, decimal closingPrice)
        {
            return (closingPrice - openingPrice) * shares;
        }

        #endregion
    }
}
