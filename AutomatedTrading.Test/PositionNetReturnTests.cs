﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;

namespace Test.Sonneville.PriceTools.AutomatedTrading
{
    public class PositionNetReturnTests
    {
        private readonly IPositionFactory _positionFactory;
        private readonly ITransactionFactory _transactionFactory;

        public PositionNetReturnTests()
        {
            _positionFactory = new PositionFactory();
            _transactionFactory = new TransactionFactory();
        }

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

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission), _transactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

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

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, buyDate, shares, buyPrice, commission), _transactionFactory.ConstructSell(ticker, sellDate, shares, sellPrice, commission));

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

            var target = _positionFactory.ConstructPosition(ticker, _transactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission));

            Assert.IsNull(target.CalculateNetReturn(sellDate));
        }
    }
}