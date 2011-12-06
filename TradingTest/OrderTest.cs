using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace TradingTest
{
    [TestClass]
    public class OrderTest
    {
        [TestMethod]
        public void IssuedTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = 100.00m;

            var target = new Order(issued, expired, orderType, ticker, shares, price);

            var expected = issued;
            var actual = target.Issued;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExpiredTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = 100.00m;

            var target = new Order(issued, expired, orderType, ticker, shares, price);

            var expected = expired;
            var actual = target.Expiration;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TickerTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = 100.00m;

            var target = new Order(issued, expired, orderType, ticker, shares, price);

            const string expected = ticker;
            var actual = target.Ticker;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PriceTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = 100.00m;

            var target = new Order(issued, expired, orderType, ticker, shares, price);

            const decimal expected = price;
            var actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OrderTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = 100.00m;

            var target = new Order(issued, expired, orderType, ticker, shares, price);

            const OrderType expected = orderType;
            var actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PricingTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = 100.00m;
            const PricingType pricingType = PricingType.Limit;

            var target = new Order(issued, expired, orderType, ticker, shares, price, pricingType);

            const PricingType expected = pricingType;
            var actual = target.PricingType;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SharesTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = 100.00m;

            var target = new Order(issued, expired, orderType, ticker, shares, price);

            const double expected = shares;
            var actual = target.Shares;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MarketPricingTypeByDefaultTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = 100.00m;

            var target = new Order(issued, expired, orderType, ticker, shares, price);

            const PricingType expected = PricingType.Market;
            var actual = target.PricingType;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NegativePriceTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = -100.00m;

            new Order(issued, expired, orderType, ticker, shares, price);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NegativeSharesTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const double shares = -5.0;
            const decimal price = 100.00m;

            new Order(issued, expired, orderType, ticker, shares, price);
        }
    }
}
