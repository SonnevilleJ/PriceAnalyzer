using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class OrderFactoryTest
    {
        [TestMethod]
        public void ConstructOrder()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = 100.00m;

            var target = OrderFactory.Instance.ConstructOrder(issued, expired, orderType, ticker, shares, price);

            Assert.AreEqual(issued, target.Issued);
            Assert.AreEqual(expired, target.Expiration);
            Assert.AreEqual(orderType, target.OrderType);
            Assert.AreEqual(ticker, target.Ticker);
            Assert.AreEqual(shares, target.Shares);
            Assert.AreEqual(price, target.Price);
        }
    }
}
