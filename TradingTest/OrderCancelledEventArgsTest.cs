using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace Sonneville.TradingTest
{
    [TestClass]
    public class OrderCancelledEventArgsTest
    {
        private static void GetObjects(out DateTime cancelled, out Order order, out OrderCancelledEventArgs target)
        {
            var issued = new DateTime(2011, 12, 6);
            cancelled = issued.AddMinutes(3);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = 100.00m;

            order = new Order(issued, expired, orderType, ticker, shares, price);
            target = new OrderCancelledEventArgs(cancelled, order);
        }

        [TestMethod]
        public void ExpiredTest()
        {
            DateTime cancelled;
            Order order;
            OrderCancelledEventArgs target;
            GetObjects(out cancelled, out order, out target);

            var expected = cancelled;
            var actual = target.Cancelled;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OrderTest()
        {
            DateTime cancelled;
            Order order;
            OrderCancelledEventArgs target;
            GetObjects(out cancelled, out order, out target);

            var expected = order;
            var actual = target.Order;
            Assert.AreEqual(expected, actual);
        }
    }
}
