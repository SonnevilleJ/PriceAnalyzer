using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestClass]
    public class PositionAnnualGrossReturnTests
    {
        private readonly IPositionFactory _positionFactory;
        private readonly ITransactionFactory _transactionFactory;

        public PositionAnnualGrossReturnTests()
        {
            _positionFactory = new PositionFactory();
            _transactionFactory = new TransactionFactory();
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnOneLoss()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m; // $100.00 per share
            const decimal sellPrice = 90.00m; // $90.00 per share
            const decimal shares = 5; // 5 shares
            const decimal commission = 5.00m; // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                            _transactionFactory.ConstructBuy(ticker, buyDate, shares,
                                                                                             buyPrice, commission),
                                                            _transactionFactory.ConstructSell(ticker, sellDate, shares,
                                                                                              sellPrice, commission));

            const decimal expectedReturn = -0.1m;
            // -10% return; loss = $50 after commissions; initial investment = $500
            var actualReturn = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = -0.5m; // -50% annual rate return
            var actual = target.CalculateAnnualGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnOneGain()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m; // $100.00 per share
            const decimal sellPrice = 110.00m; // $110.00 per share
            const decimal shares = 5; // 5 shares
            const decimal commission = 5.00m; // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                            _transactionFactory.ConstructBuy(ticker, buyDate, shares,
                                                                                             buyPrice, commission),
                                                            _transactionFactory.ConstructSell(ticker, sellDate, shares,
                                                                                              sellPrice, commission));

            const decimal expectedReturn = 0.1m;
            // 10% return; profit = $50 after commissions; initial investment = $500
            var actualReturn = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = 0.5m; // 50% annual rate return
            var actual = target.CalculateAnnualGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateAnnualGrossReturnOpenPosition()
        {
            const string ticker = "DE";
            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal price = 100.00m; // $100.00 per share
            const decimal shares = 5; // 5 shares
            const decimal commission = 5.00m; // with $5 commission

            var target = _positionFactory.ConstructPosition(ticker,
                                                            _transactionFactory.ConstructBuy(ticker, buyDate, shares,
                                                                                             price, commission));

            Assert.IsNull(target.CalculateAnnualGrossReturn(sellDate));
        }
    }
}