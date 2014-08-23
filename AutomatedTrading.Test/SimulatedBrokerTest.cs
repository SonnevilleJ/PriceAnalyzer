using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestClass]
    public class SimulatedBrokerTest
    {
        private SimulatedBroker _simulatedBroker;
        private Order _deOrder;
        private Order _ibmOrder;

        [TestInitialize]
        public void Setup()
        {
            _deOrder = new Order { Issued = DateTime.Now, OrderType = OrderType.Buy, Price = 5, Shares = 1, Ticker = "DE" };
            _ibmOrder = new Order { Issued = DateTime.Now, OrderType = OrderType.Sell, Price = 7, Shares = 3, Ticker = "IBM" };
            _simulatedBroker = new SimulatedBroker();
        }

        [TestMethod]
        public void SubmitOrderReturnsDeOrderStatus()
        {
            var submitTime = DateTime.Now;
            var orderStatus = _simulatedBroker.SubmitOrder(_deOrder);

            Assert.IsNotNull(orderStatus);
            Assert.AreEqual(_deOrder.Ticker, orderStatus.Ticker);
            Assert.AreEqual(_deOrder.Price, orderStatus.Price);
            Assert.AreEqual(_deOrder.Shares, orderStatus.Shares);
            Assert.AreEqual(_deOrder.OrderType, orderStatus.OrderType);
            Assert.IsTrue(orderStatus.SubmitTime - submitTime < new TimeSpan(0, 0, 0, 0, 50));
        }

        [TestMethod]
        public void SubmitOrderReturnsIbmOrderStatus()
        {
            var submitTime = DateTime.Now;
            var orderStatus = _simulatedBroker.SubmitOrder(_ibmOrder);

            Assert.IsNotNull(orderStatus);
            Assert.AreEqual(_ibmOrder.Ticker, orderStatus.Ticker);
            Assert.AreEqual(_ibmOrder.Price, orderStatus.Price);
            Assert.AreEqual(_ibmOrder.Shares, orderStatus.Shares);
            Assert.AreEqual(_ibmOrder.OrderType, orderStatus.OrderType);
            Assert.IsTrue(orderStatus.SubmitTime - submitTime < new TimeSpan(0, 0, 0, 0, 50));
        }

        [TestMethod]
        public void OrderStatusIdsShouldBeUnique()
        {
            var deOrderStatus = _simulatedBroker.SubmitOrder(_deOrder);
            var ibmOrderStatus = _simulatedBroker.SubmitOrder(_ibmOrder);

            Assert.AreNotEqual(Guid.Empty, deOrderStatus.Id);
            Assert.AreNotEqual(Guid.Empty, ibmOrderStatus.Id);
            Assert.AreNotEqual(deOrderStatus.Id, ibmOrderStatus.Id);
        }
    }
}
