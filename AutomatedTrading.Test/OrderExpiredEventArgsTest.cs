using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;

namespace Test.Sonneville.PriceTools.AutomatedTrading
{
    [TestClass]
    public class OrderExpiredEventArgsTest
    {
        private static void GetObjects(out IOrder order, out OrderExpiredEventArgs target)
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const decimal shares = 5;
            const decimal price = 100.00m;
            
            order = OrderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);
            target = new OrderExpiredEventArgs(order);
        }

        [TestMethod]
        public void ExpiredTest()
        {
            IOrder order;
            OrderExpiredEventArgs target;
            GetObjects(out order, out target);

            var expected = order.Expiration;
            var actual = target.Expired;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OrderTest()
        {
            IOrder order;
            OrderExpiredEventArgs target;
            GetObjects(out order, out target);

            var expected = order;
            var actual = target.Order;
            Assert.AreEqual(expected, actual);
        }
    }
}
