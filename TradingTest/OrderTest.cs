using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace Sonneville.TradingTest
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DepositTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Deposit;
            const string ticker = "DE";
            const double shares = -5.0;
            const decimal price = 100.00m;

            new Order(issued, expired, orderType, ticker, shares, price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WithdrawalTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Withdrawal;
            const string ticker = "DE";
            const double shares = -5.0;
            const decimal price = 100.00m;

            new Order(issued, expired, orderType, ticker, shares, price);
        }

        /// <summary>
        /// Verifies that an order cannot be created with a binary and-ed OrderType, including bounds testing
        /// </summary>
        [TestMethod]
        public void BinaryAndedOrderTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const string ticker = "DE";
            const double shares = 5.0;
            const decimal price = 100.00m;

            var errors = new List<int>();
            var min = Enum.GetValues(typeof(OrderType)).Cast<int>().Min() - 1;
            var max = Enum.GetValues(typeof(OrderType)).Cast<int>().Max() + 1;
            for (var i = min; i < max; i++)
            {
                if (Enum.IsDefined(typeof (OrderType), i)) continue;

                var orderType = (OrderType) i;
                try
                {
                    new Order(issued, expired, orderType, ticker, shares, price);

                    // Order class did not validate appropriately
                    errors.Add(i);
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Order class validated appropriately
                }
            }

            const int expected = 0;
            var actual = errors.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
