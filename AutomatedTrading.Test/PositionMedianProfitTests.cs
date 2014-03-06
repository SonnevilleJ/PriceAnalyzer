using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestClass]
    public class PositionMedianProfitTests
    {
        private readonly IPositionFactory _positionFactory;
        private readonly ITransactionFactory _transactionFactory;

        public PositionMedianProfitTests()
        {
            _positionFactory = new PositionFactory();
            _transactionFactory = new TransactionFactory();
        }

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

            var expected = CalculationHelper.GetExpectedMedianProfit(target.CalculateHoldings(sellDate));
            var actual = SecurityBasketExtensions.CalculateMedianProfit(target, sellDate);
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

            var expected = CalculationHelper.GetExpectedMedianProfit(target.CalculateHoldings(sellDate));
            var actual = SecurityBasketExtensions.CalculateMedianProfit(target, sellDate);
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var expected = CalculationHelper.GetExpectedMedianProfit(target.CalculateHoldings(secondSellDate));
            var actual = SecurityBasketExtensions.CalculateMedianProfit(target, secondSellDate);
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var expected = CalculationHelper.GetExpectedMedianProfit(target.CalculateHoldings(secondSellDate));
            var actual = SecurityBasketExtensions.CalculateMedianProfit(target, secondSellDate);
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var expected = CalculationHelper.GetExpectedMedianProfit(target.CalculateHoldings(sellDate));
            var actual = SecurityBasketExtensions.CalculateMedianProfit(target, secondSellDate);
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);
            var thirdSell = _transactionFactory.ConstructSell(de, thirdSellDate, ((sharesBought*2) - (sharesSold*2)),
                                                              thirdPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell, thirdSell);

            var expected = CalculationHelper.GetExpectedMedianProfit(target.CalculateHoldings(thirdSellDate));
            var actual = SecurityBasketExtensions.CalculateMedianProfit(target, thirdSellDate);
            Assert.AreEqual(expected, actual);
        }
    }
}