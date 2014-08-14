using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;
using Sonneville.PriceTools.SampleData;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestClass]
    public class TradingEngineTest
    {
        private TradingEngine _tradingEngine;
        private Mock<IPortfolio> _portfolioMock;
        private IPriceSeries _dePriceSeries;
        private Mock<IPosition> _dePositionMock;
        private Mock<ISecurityBasketCalculator> _securityBasketCalculatorMock;
        private List<ITransaction> _deTransactions;

        [TestInitialize]
        public void Initialize()
        {
            _portfolioMock = new Mock<IPortfolio>();

            _dePriceSeries = SamplePriceDatas.Deere.PriceSeries;
            _deTransactions = new List<ITransaction>();
            _dePositionMock = new Mock<IPosition>();
            _dePositionMock.Setup(x => x.Transactions).Returns(_deTransactions);
            _portfolioMock.Setup(x => x.GetPosition(_dePriceSeries.Ticker)).Returns(_dePositionMock.Object);
            
            _securityBasketCalculatorMock = new Mock<ISecurityBasketCalculator>();
            _tradingEngine = new TradingEngine(_securityBasketCalculatorMock.Object, _portfolioMock.Object);
        }

        [TestMethod]
        public void ShouldNotTradeIfInsufficientFunds()
        {
            var startDate = new DateTime(2011, 1, 4);
            var endDate = new DateTime(2011, 1, 5);
            var justLessThanTodaysPrice = _dePriceSeries[endDate] - 0.01m;
            _portfolioMock.Setup(x => x.GetAvailableCash(endDate)).Returns(justLessThanTodaysPrice);

            var orders = _tradingEngine.DetermineOrdersFor(_dePriceSeries, startDate, endDate);

            Assert.AreEqual(0, orders.Count, "An order was returned when there were insufficient funds.");
        }

        [TestMethod]
        public void ShouldCreateBuyOrderWhenPreviousDayIsPositive()
        {
            var startDate = new DateTime(2011, 1, 4);
            var endDate = new DateTime(2011, 1, 5);
            _portfolioMock.Setup(x => x.GetAvailableCash(endDate)).Returns(1000);
            
            var orders = _tradingEngine.DetermineOrdersFor(_dePriceSeries, startDate, endDate);

            Assert.AreEqual(1, orders.Count);
            var order = orders.ElementAt(0);
            Assert.AreEqual(OrderType.Buy, order.OrderType);
            Assert.AreEqual(1, order.Shares);
            Assert.AreEqual(endDate, order.Issued);
            Assert.AreEqual(_dePriceSeries.Ticker, order.Ticker);
        }

        [TestMethod]
        public void ShouldCreateNoOrderWhenPreviousDayIsNegativeAndNoSharesHeld()
        {
            var startDate = new DateTime(2011, 1, 3);
            var endDate = new DateTime(2011, 1, 4);
            _portfolioMock.Setup(x => x.GetAvailableCash(endDate)).Returns(1000);
            _portfolioMock.Setup(x => x.GetPosition(_dePriceSeries.Ticker)).Returns((IPosition) null);

            var orders = _tradingEngine.DetermineOrdersFor(_dePriceSeries, startDate, endDate);

            Assert.AreEqual(0, orders.Count);
        }

        [TestMethod]
        public void ShouldCreateSellOrderWhenPreviousDayIsNegative2()
        {
            var sharesToSell = 2;
            RunSellAllSharesTest(sharesToSell);
        }

        [TestMethod]
        public void ShouldCreateSellOrderWhenPreviousDayIsNegative5()
        {
            var sharesToSell = 5;
            RunSellAllSharesTest(sharesToSell);
        }

        private void RunSellAllSharesTest(int sharesToSell)
        {
            var startDate = new DateTime(2011, 1, 3);
            var endDate = new DateTime(2011, 1, 4);
            _securityBasketCalculatorMock.Setup(x => x.GetHeldShares(It.IsAny<IEnumerable<IShareTransaction>>(), endDate))
                .Returns(sharesToSell);
            var orders = _tradingEngine.DetermineOrdersFor(_dePriceSeries, startDate, endDate);

            Assert.AreEqual(1, orders.Count);
            var order = orders.ElementAt(0);
            Assert.AreEqual(OrderType.Sell, order.OrderType);
            Assert.AreEqual(sharesToSell, order.Shares);
            Assert.AreEqual(endDate, order.Issued);
            Assert.AreEqual(_dePriceSeries.Ticker, order.Ticker);
        }
    }
}
