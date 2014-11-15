using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class PositionAverageCostTests
    {
        private IPositionFactory _positionFactory;
        private ITransactionFactory _transactionFactory;
        private ISecurityBasketCalculator _securityBasketCalculator;

        [SetUp]
        public void Setup()
        {
            _positionFactory = new PositionFactory(new PriceSeriesFactory(), new SecurityBasketCalculator());
            _transactionFactory = new TransactionFactory();
            _securityBasketCalculator = new SecurityBasketCalculator();
        }
        
        [Test]
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
            var actualAverageCost = _securityBasketCalculator.CalculateAverageCost(target, buyDate);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }

        [Test]
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
            var actualAverageCost = _securityBasketCalculator.CalculateAverageCost(target, buyDate);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }

        [Test]
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
            var actualAverageCost = _securityBasketCalculator.CalculateAverageCost(target, buyDate2);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }

        [Test]
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
            var actualAverageCost = _securityBasketCalculator.CalculateAverageCost(target, buyDate2);
            Assert.AreEqual(expectedAverageCost, actualAverageCost);
        }
    }
}