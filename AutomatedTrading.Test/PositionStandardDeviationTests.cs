﻿using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class PositionStandardDeviationTests
    {
        private IPositionFactory _positionFactory;
        private ITransactionFactory _transactionFactory;
        private ISecurityBasketCalculator _securityBasketCalculator;
        private IHoldingFactory _holdingFactory;

        [SetUp]
        public void Setup()
        {
            _positionFactory = new PositionFactory(new PriceSeriesFactory(), new SecurityBasketCalculator());
            _transactionFactory = new TransactionFactory();
            _securityBasketCalculator = new SecurityBasketCalculator();
            _holdingFactory = new HoldingFactory();
        }

        [Test]
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

            var holdings = _holdingFactory.CalculateHoldings(target, sellDate);
            var expected = CalculationHelper.GetExpectedStandardDeviation(holdings);
            var actual = _securityBasketCalculator.CalculateStandardDeviation(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
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

            var holdings = _holdingFactory.CalculateHoldings(target, sellDate);
            var expected = CalculationHelper.GetExpectedStandardDeviation(holdings);
            var actual = _securityBasketCalculator.CalculateStandardDeviation(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);
            var expected = CalculationHelper.GetExpectedStandardDeviation(holdings);
            var actual = _securityBasketCalculator.CalculateStandardDeviation(target, secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var holdings = _holdingFactory.CalculateHoldings(target, secondSellDate);
            var expected = CalculationHelper.GetExpectedStandardDeviation(holdings);
            var actual = _securityBasketCalculator.CalculateStandardDeviation(target, secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell);

            var holdings = _holdingFactory.CalculateHoldings(target, sellDate);
            var expected = CalculationHelper.GetExpectedStandardDeviation(holdings);
            var actual = _securityBasketCalculator.CalculateStandardDeviation(target, secondSellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
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
            var secondBuy = _transactionFactory.ConstructBuy(de, secondBuyDate, sharesBought, secondPriceBought,
                                                             commission);
            var secondSell = _transactionFactory.ConstructSell(de, secondSellDate, sharesSold, secondPriceSold,
                                                               commission);
            var thirdSell = _transactionFactory.ConstructSell(de, thirdSellDate, ((sharesBought*2) - (sharesSold*2)),
                                                              thirdPriceSold, commission);

            var target = _positionFactory.ConstructPosition(de, firstBuy, firstSell, secondBuy, secondSell, thirdSell);

            var holdings = _holdingFactory.CalculateHoldings(target, thirdSellDate);
            var expected = CalculationHelper.GetExpectedStandardDeviation(holdings);
            var actual = _securityBasketCalculator.CalculateStandardDeviation(target, thirdSellDate);
            Assert.AreEqual(expected, actual);
        }
    }
}