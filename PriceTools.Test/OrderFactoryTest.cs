using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class OrderFactoryTest
    {
        private readonly IOrderFactory _orderFactory;

        public OrderFactoryTest()
        {
            _orderFactory = new OrderFactory();
        }

        [Test]
        public void IssuedTest()
        {
            var issued = GetIssueDate();
            var expired = GetExpiration(issued);
            var orderType = GetOrderType();
            var ticker = GetTicker();
            var shares = GetValidShares();
            var price = GetValidPrice();
            var pricingType = GetPricingType();

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);

            Assert.AreEqual(issued, target.Issued);
        }

        [Test]
        public void ExpirationTest()
        {
            var issued = GetIssueDate();
            var expired = GetExpiration(issued);
            var orderType = GetOrderType();
            var ticker = GetTicker();
            var shares = GetValidShares();
            var price = GetValidPrice();
            var pricingType = GetPricingType();

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);

            Assert.AreEqual(expired, target.Expiration);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExpirationBeforeIssuedTest()
        {
            var issued = GetIssueDate();
            var expired = issued.Subtract(new TimeSpan(1));
            var orderType = GetOrderType();
            var ticker = GetTicker();
            var shares = GetValidShares();
            var price = GetValidPrice();
            var pricingType = GetPricingType();

            _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);
        }

        [Test]
        public void OrderTypeTest()
        {
            var issued = GetIssueDate();
            var expired = GetExpiration(issued);
            var orderType = GetOrderType();
            var ticker = GetTicker();
            var shares = GetValidShares();
            var price = GetValidPrice();
            var pricingType = GetPricingType();

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);

            Assert.AreEqual(orderType, target.OrderType);
        }

        [Test]
        public void TickerTest()
        {
            var issued = GetIssueDate();
            var expired = GetExpiration(issued);
            var orderType = GetOrderType();
            var ticker = GetTicker();
            var shares = GetValidShares();
            var price = GetValidPrice();
            var pricingType = GetPricingType();

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);

            Assert.AreEqual(ticker, target.Ticker);
        }

        [Test]
        public void ValidSharesTest()
        {
            var issued = GetIssueDate();
            var expired = GetExpiration(issued);
            var orderType = GetOrderType();
            var ticker = GetTicker();
            var shares = GetValidShares();
            var price = GetValidPrice();
            var pricingType = GetPricingType();

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);

            Assert.AreEqual(shares, target.Shares);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidSharesTest()
        {
            var issued = GetIssueDate();
            var expired = GetExpiration(issued);
            var orderType = GetOrderType();
            var ticker = GetTicker();
            var shares = GetInvalidShares();
            var price = GetValidPrice();
            var pricingType = GetPricingType();

            _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);
        }

        [Test]
        public void ValidPriceTest()
        {
            var issued = GetIssueDate();
            var expired = GetExpiration(issued);
            var orderType = GetOrderType();
            var ticker = GetTicker();
            var shares = GetValidShares();
            var price = GetValidPrice();
            var pricingType = GetPricingType();

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);

            Assert.AreEqual(price, target.Price);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidPriceTest()
        {
            var issued = GetIssueDate();
            var expired = GetExpiration(issued);
            var orderType = GetOrderType();
            var ticker = GetTicker();
            var shares = GetValidShares();
            var price = GetInvalidPrice();
            var pricingType = GetPricingType();

            _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);
        }

        [Test]
        public void PricingTypeDefaultTest()
        {
            var issued = GetIssueDate();
            var expired = GetExpiration(issued);
            var orderType = GetOrderType();
            var ticker = GetTicker();
            var shares = GetValidShares();
            var price = GetValidPrice();

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);

            Assert.AreEqual(PricingType.Market, target.PricingType);
        }

        [Test]
        public void PricingTypeTest()
        {
            var issued = GetIssueDate();
            var expired = GetExpiration(issued);
            var orderType = GetOrderType();
            var ticker = GetTicker();
            var shares = GetValidShares();
            var price = GetValidPrice();
            var pricingType = GetPricingType();

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);

            Assert.AreEqual(pricingType, target.PricingType);
        }

        private static DateTime GetIssueDate()
        {
            return new DateTime(2011, 12, 6);
        }

        private static DateTime GetExpiration(DateTime issued)
        {
            return issued.AddMinutes(30);
        }

        private static OrderType GetOrderType()
        {
            return OrderType.Buy;
        }

        private static string GetTicker()
        {
            return "DE";
        }

        private static decimal GetValidShares()
        {
            return 5m;
        }

        private static decimal GetInvalidShares()
        {
            return -GetValidShares();
        }

        private static decimal GetValidPrice()
        {
            return 100.00m;
        }

        private static decimal GetInvalidPrice()
        {
            return -GetValidPrice();
        }

        private static PricingType GetPricingType()
        {
            return PricingType.Market;
        }
    }
}
