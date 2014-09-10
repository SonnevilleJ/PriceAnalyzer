using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class TradeManagerTest
    {
        private TradeManager _tradeManager;
        private Mock<IBrokerage> _brokerageMock;
        private Order _order;

        [SetUp]
        public void Setup()
        {
            _order = new Order();
            _brokerageMock = new Mock<IBrokerage>();
            _tradeManager = new TradeManager(_brokerageMock.Object);
        }

        [Test]
        public void SubmitOrderSubmitsToBrokerage()
        {
            _tradeManager.Submit(_order);

            OrderWasSubmittedToBrokerage(_order);
        }

        [Test]
        public void GetOpenOrdersReturnsPreviouslySubmittedOrder()
        {
            _tradeManager.Submit(_order);

            var openOrders = _tradeManager.GetOpenOrders();

            CollectionAssert.Contains(openOrders, _order);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetOpenOrdersReturnsReadOnlyList()
        {
            _tradeManager.Submit(_order);
            var openOrders = _tradeManager.GetOpenOrders();

            (openOrders as IList<Order>).Add(new Order());
        }

        [Test]
        public void CancelOrderSubmitsToBrokerage()
        {
            _tradeManager.Submit(_order);
            var openOrders = _tradeManager.GetOpenOrders();
            CollectionAssert.Contains(openOrders, _order);

            _tradeManager.CancelOrder(_order);

            _brokerageMock.Verify(brokerage => brokerage.CancelOrder(_order));
            openOrders = _tradeManager.GetOpenOrders();
            CollectionAssert.DoesNotContain(openOrders, _order);
        }

        [Test]
        public void CancelOrderDoesNotSubmitUnsubmittedOrderToBrokerage()
        {
            _tradeManager.CancelOrder(_order);

            _brokerageMock.Verify(brokerage => brokerage.CancelOrder(_order), Times.Never());
            var openOrders = _tradeManager.GetOpenOrders();
            CollectionAssert.DoesNotContain(openOrders, _order);
        }

        [Test]
        public void RefreshTradesQueriesFromBroker()
        {
            _brokerageMock.Setup(brokerage => brokerage.GetOpenOrders()).Returns(new List<Order> {_order});

            _tradeManager.RefreshFromBrokerage();

            var openOrders = _tradeManager.GetOpenOrders();
            CollectionAssert.Contains(openOrders, _order);
        }

        [Test]
        public void RefreshTradesDoesNotDuplicateOrders()
        {
            _brokerageMock.Setup(brokerage => brokerage.GetOpenOrders()).Returns(new List<Order> {_order});
            _tradeManager.Submit(_order);

            _tradeManager.RefreshFromBrokerage();

            var openOrders = _tradeManager.GetOpenOrders();
            CollectionAssert.Contains(openOrders, _order);
            Assert.AreEqual(1, openOrders.Count());
        }

        private void OrderWasSubmittedToBrokerage(Order order)
        {
            _brokerageMock.Verify(brokerage=>brokerage.SubmitOrders(It.Is<IEnumerable<Order>>(list=>list.Contains(order))));
        }
    }
}