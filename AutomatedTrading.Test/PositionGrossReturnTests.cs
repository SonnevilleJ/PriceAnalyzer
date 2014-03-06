using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestClass]
    public class PositionGrossReturnTests
    {
        private readonly IPositionFactory _positionFactory;
        private readonly ITransactionFactory _transactionFactory;

        public PositionGrossReturnTests()
        {
            _positionFactory = new PositionFactory();
            _transactionFactory = new TransactionFactory();
        }

        [TestMethod]
        public void PositionCalculateGrossReturnOneLoss()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const decimal buyPrice = 100.00m; // $100.00 per share
            const decimal sharesBought = 10; // 10 shares
            const decimal commission = 7.95m; // with $7.95 commission
            const decimal decrease = -0.10m; // 10% price decrease when sold
            const decimal sellPrice = buyPrice*(1 + decrease);
            const decimal sharesSold = sharesBought - 2;

            var target = _positionFactory.ConstructPosition(ticker,
                                                            _transactionFactory.ConstructBuy(ticker, buyDate,
                                                                                             sharesBought, buyPrice,
                                                                                             commission),
                                                            _transactionFactory.ConstructSell(ticker, sellDate,
                                                                                              sharesSold, sellPrice,
                                                                                              commission));

            const decimal expected = decrease;
            var actual = SecurityBasketExtensions.CalculateGrossReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateGrossReturnOneGain()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const decimal buyPrice = 100.00m; // $100.00 per share
            const decimal sharesBought = 10; // 10 shares
            const decimal commission = 7.95m; // with $7.95 commission
            const decimal increase = 0.10m; // 10% price increase when sold
            const decimal sellPrice = buyPrice*(1 + increase);
            const decimal sharesSold = sharesBought - 2;

            var target = _positionFactory.ConstructPosition(ticker,
                                                            _transactionFactory.ConstructBuy(ticker, buyDate,
                                                                                             sharesBought, buyPrice,
                                                                                             commission),
                                                            _transactionFactory.ConstructSell(ticker, sellDate,
                                                                                              sharesSold, sellPrice,
                                                                                              commission));

            const decimal expected = increase;
            var actual = SecurityBasketExtensions.CalculateGrossReturn(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateGrossReturnOpenPosition()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m; // $100.00 per share
            const decimal shares = 5; // 5 shares
            const decimal commission = 7.95m; // with $7.95 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                            _transactionFactory.ConstructBuy(ticker, buyDate, shares,
                                                                                             price, commission));

            Assert.IsNull(SecurityBasketExtensions.CalculateGrossReturn(target, sellDate));
        }
    }
}