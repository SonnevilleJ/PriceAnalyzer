using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Moq;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;
using Sonneville.PriceTools.SampleData;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class AnalysisEngineTest
    {
        private AnalysisEngine _analysisEngine;
        private Mock<IPortfolio> _portfolioMock;
        private IPriceSeries _dePriceSeries;
        private Mock<IPosition> _dePositionMock;
        private Mock<ISecurityBasketCalculator> _securityBasketCalculatorMock;
        private List<ITransaction> _deTransactions;
        private List<Order> _openOrders;

        [SetUp]
        public void Setup()
        {
            _openOrders = new List<Order>();

            _dePriceSeries = SamplePriceDatas.Deere.PriceSeries;
            
            _deTransactions = new List<ITransaction>();
            
            _dePositionMock = new Mock<IPosition>();
            _dePositionMock.Setup(x => x.Transactions).Returns(_deTransactions);
            
            _portfolioMock = new Mock<IPortfolio>();
            _portfolioMock.Setup(x => x.GetPosition(_dePriceSeries.Ticker)).Returns(_dePositionMock.Object);
            
            _securityBasketCalculatorMock = new Mock<ISecurityBasketCalculator>();
            _analysisEngine = new AnalysisEngine(_securityBasketCalculatorMock.Object);
        }

        [Test]
        public void ShouldNotTradeIfInsufficientFunds()
        {
            var startDate = new DateTime(2011, 1, 4);
            var endDate = new DateTime(2011, 1, 5);
            var justLessThanTodaysPrice = _dePriceSeries[endDate] - 0.01m;
            _portfolioMock.Setup(x => x.GetAvailableCash(endDate)).Returns(justLessThanTodaysPrice);

            var orders = _analysisEngine.DetermineOrdersFor(_portfolioMock.Object, _dePriceSeries, startDate, endDate, _openOrders).ToList();

            Assert.AreEqual(0, orders.Count, "An order was returned when there were insufficient funds.");
        }

        [Test]
        public void ShouldCreateBuyOrderWhenPreviousDayIsPositive()
        {
            var startDate = new DateTime(2011, 1, 4);
            var endDate = new DateTime(2011, 1, 5);
            _portfolioMock.Setup(x => x.GetAvailableCash(endDate)).Returns(1000);

            var orders = _analysisEngine.DetermineOrdersFor(_portfolioMock.Object, _dePriceSeries, startDate, endDate, _openOrders).ToList();

            Assert.AreEqual(1, orders.Count);
            var order = orders.ElementAt(0);
            Assert.AreEqual(OrderType.Buy, order.OrderType);
            Assert.AreEqual(1, order.Shares);
            Assert.AreEqual(endDate, order.Issued);
            Assert.AreEqual(_dePriceSeries.Ticker, order.Ticker);
        }

        [Test]
        public void ShouldCreateNoOrderWhenPreviousDayIsNegativeAndNoSharesHeld()
        {
            var startDate = new DateTime(2011, 1, 3);
            var endDate = new DateTime(2011, 1, 4);
            _portfolioMock.Setup(x => x.GetAvailableCash(endDate)).Returns(1000);
            _portfolioMock.Setup(x => x.GetPosition(_dePriceSeries.Ticker)).Returns((IPosition) null);

            var orders = _analysisEngine.DetermineOrdersFor(_portfolioMock.Object, _dePriceSeries, startDate, endDate, _openOrders).ToList();

            Assert.AreEqual(0, orders.Count);
        }

        [Test]
        public void ShouldCreateSellOrderWhenPreviousDayIsNegative()
        {
            var startDate = new DateTime(2011, 1, 3);
            var endDate = new DateTime(2011, 1, 4);
            _securityBasketCalculatorMock.Setup(x => x.GetHeldShares(It.IsAny<IEnumerable<IShareTransaction>>(), endDate))
                .Returns(2);
            var orders = _analysisEngine.DetermineOrdersFor(_portfolioMock.Object, _dePriceSeries, startDate, endDate, _openOrders).ToList();

            Assert.AreEqual(1, orders.Count);
            var order = orders.ElementAt(0);
            Assert.AreEqual(OrderType.Sell, order.OrderType);
            Assert.AreEqual(2, order.Shares);
            Assert.AreEqual(endDate, order.Issued);
            Assert.AreEqual(_dePriceSeries.Ticker, order.Ticker);
        }

        [Test]
        public void ShouldCreateSellOrderWhenPreviousDayIsNegative5AndOpenSellOrder()
        {
            const int sharesToSell = 5;
            _openOrders.Add(new Order{OrderType = OrderType.Sell, Shares = 2, Ticker = _dePriceSeries.Ticker});
            var startDate = new DateTime(2011, 1, 3);
            var endDate = new DateTime(2011, 1, 4);
            _securityBasketCalculatorMock.Setup(x => x.GetHeldShares(It.IsAny<IEnumerable<IShareTransaction>>(), endDate))
                .Returns(sharesToSell);
            var orders = _analysisEngine.DetermineOrdersFor(_portfolioMock.Object, _dePriceSeries, startDate, endDate, _openOrders).ToList();

            Assert.AreEqual(1, orders.Count);
            var order = orders.ElementAt(0);
            Assert.AreEqual(OrderType.Sell, order.OrderType);
            Assert.AreEqual(3, order.Shares);
            Assert.AreEqual(endDate, order.Issued);
            Assert.AreEqual(_dePriceSeries.Ticker, order.Ticker);
        }
    }
}
