using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestClass]
    public class PositionNetProfitTests
    {
        private IPositionFactory _positionFactory;
        private ITransactionFactory _transactionFactory;
        private ISecurityBasketCalculator _securityBasketCalculator;

        [TestInitialize]
        public void Initialize()
        {
            _positionFactory = new PositionFactory();
            _transactionFactory = new TransactionFactory();
            _securityBasketCalculator = new SecurityBasketCalculator();
        }

        [TestMethod]
        public void CalculateNetProfitOpenPosition()
        {
            const string ticker = "DE";
            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m; // bought at $100.00 per share
            const decimal oShares = 5; // bought 5 shares
            const decimal oCommission = 7.95m; // bought with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                            _transactionFactory.ConstructBuy(ticker, oDate, oShares,
                                                                                             oPrice, oCommission));

            // CalculateGrossProfit does not consider open positions - it can only account for closed holdings
            const decimal expected = 0;
            var actual = _securityBasketCalculator.CalculateNetProfit(target, oDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitOneGain()
        {
            const string ticker = "DE";
            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m; // bought at $100.00 per share
            const decimal oShares = 5; // bought 5 shares
            const decimal oCommission = 7.95m; // bought with $7.95 commission

            var cDate = new DateTime(2001, 1, 1);
            const decimal cPrice = 110.00m; // sold at $110.00 per share
            const decimal cShares = 5; // sold 5 shares
            const decimal cCommission = 7.95m; // sold with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                            _transactionFactory.ConstructBuy(ticker, oDate, oShares,
                                                                                             oPrice, oCommission),
                                                            _transactionFactory.ConstructSell(ticker, cDate, cShares,
                                                                                              cPrice, cCommission));

            // No longer hold these shares, so CalculateNetProfit should return total profit with all commissions.
            var expected = CalculationHelper.GetExpectedNetProfit(oPrice, oCommission, cShares, cPrice, oCommission);
            var actual = _securityBasketCalculator.CalculateNetProfit(target, cDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNetProfitOneLoss()
        {
            const string ticker = "DE";
            var oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.00m; // bought at $100.00 per share
            const decimal oShares = 5; // bought 5 shares
            const decimal oCommission = 7.95m; // bought with $7.95 commission

            var cDate = new DateTime(2001, 1, 1);
            const decimal cPrice = 90.00m; // sold at $90.00 per share - $10 per share loss
            const decimal cShares = 5; // sold 5 shares
            const decimal cCommission = 7.95m; // sold with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                            _transactionFactory.ConstructBuy(ticker, oDate, oShares,
                                                                                             oPrice, oCommission),
                                                            _transactionFactory.ConstructSell(ticker, cDate, cShares,
                                                                                              cPrice, cCommission));

            // No longer hold these shares, so CalculateNetProfit should return total profit with all commissions.
            var expected = CalculationHelper.GetExpectedNetProfit(oPrice, oCommission, cShares, cPrice, oCommission);
            var actual = _securityBasketCalculator.CalculateNetProfit(target, cDate);
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var firstProfit = CalculationHelper.GetExpectedNetProfit(firstPriceBought, commission, sharesSold, firstPriceSold, commission);
            var secondProfit = CalculationHelper.GetExpectedNetProfit(secondPriceBought, commission, sharesSold, secondPriceSold, commission);

            var expected = firstProfit + secondProfit;
            var actual = _securityBasketCalculator.CalculateNetProfit(target, secondSellDate);
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            // both holdings will use the original shares, so both must use firstPriceBought
            var firstProfit = CalculationHelper.GetExpectedNetProfit(firstPriceBought, commission, sharesSold, firstPriceSold, commission);
            var secondProfit = CalculationHelper.GetExpectedNetProfit(firstPriceBought, commission, sharesSold, secondPriceSold, commission);

            var expected = firstProfit + secondProfit;
            var actual = _securityBasketCalculator.CalculateNetProfit(target, secondSellDate);
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var firstProfit = CalculationHelper.GetExpectedNetProfit(firstPriceBought, commission, sharesSold, firstPriceSold, commission);
            var secondProfit = CalculationHelper.GetExpectedNetProfit(secondPriceBought, commission, sharesSold, secondPriceSold, commission);

            var expected = firstProfit + secondProfit;
            var actual = _securityBasketCalculator.CalculateNetProfit(target, secondSellDate);
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var firstProfit = CalculationHelper.GetExpectedNetProfit(firstPriceBought, commission, sharesSold, firstPriceSold, commission);
            var secondProfit = CalculationHelper.GetExpectedNetProfit(secondPriceBought, commission, sharesSold, secondPriceSold, commission);

            var expected = firstProfit + secondProfit;
            var actual = _securityBasketCalculator.CalculateNetProfit(target, secondSellDate);
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            // both holdings will use the original shares, so both must use firstPriceBought
            var firstProfit = CalculationHelper.GetExpectedNetProfit(firstPriceBought, commission, sharesSold, firstPriceSold, commission);
            var secondProfit = CalculationHelper.GetExpectedNetProfit(firstPriceBought, commission, sharesSold, secondPriceSold, commission);

            var expected = firstProfit + secondProfit;
            var actual = _securityBasketCalculator.CalculateNetProfit(target, secondSellDate);
            Assert.AreEqual(expected, actual);
        }
    }
}