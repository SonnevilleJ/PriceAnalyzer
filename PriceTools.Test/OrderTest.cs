using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Implementation;
using Sonneville.PriceTools.TestUtilities;
using Sonneville.Utilities.Serialization;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class OrderTest
    {
        private readonly IOrderFactory _orderFactory;

        public OrderTest()
        {
            _orderFactory = new OrderFactory();
        }

        [TestMethod]
        public void SerializeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);

            var xml = XmlSerializer.SerializeToXml(target);
            var result = XmlSerializer.DeserializeFromXml<Order>(xml);

            GenericTestUtilities.AssertSameState(target, result);
        }

        [TestMethod]
        public void IssuedTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);

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
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);

            var expected = expired;
            var actual = target.Expiration;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IssuedAfterExpiredFails()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddTicks(-1);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);
        }

        [TestMethod]
        public void TickerTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);

            var expected = ticker;
            var actual = target.Ticker;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PriceTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);

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
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);

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
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;
            const PricingType pricingType = PricingType.Limit;

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);

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
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);

            const decimal expected = shares;
            var actual = target.Shares;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MarketPricingTypeByDefaultTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);

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
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = -100.00m;

            _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeSharesTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = -5m;
            const decimal price = 100.00m;

            _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DepositTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Deposit;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WithdrawalTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Withdrawal;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReceiptTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.DividendReceipt;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.DividendReinvestment;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);
        }

        /// <summary>
        /// Verifies that an order cannot be created with a binary and-ed OrderType, including bounds testing
        /// </summary>
        [TestMethod]
        public void BinaryAndedOrderTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            var ticker = "DE";
            const decimal shares = 5m;
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
                    _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);

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

        /// <summary>
        /// Verifies that an order cannot be created with a binary and-ed PricingType, including bounds testing
        /// </summary>
        [TestMethod]
        public void BinaryAndedPricingTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            var errors = new List<int>();
            var min = Enum.GetValues(typeof(PricingType)).Cast<int>().Min() - 1;
            var max = Enum.GetValues(typeof(PricingType)).Cast<int>().Max() + 1;
            for (var i = min; i < max; i++)
            {
                if (Enum.IsDefined(typeof(PricingType), i)) continue;

                var pricingType = (PricingType)i;
                try
                {
                    _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);

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

        /// <summary>
        /// Verifies that an order cannot be created with a binary and-ed market and limit PricingType
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ManualMarketLimitPricingTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;
            const PricingType pricingType = PricingType.Market | PricingType.Limit;

            _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);
        }

        /// <summary>
        /// Verifies that a stop order can be created
        /// </summary>
        [TestMethod]
        public void ManualStopMarketPricingTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;
            const PricingType pricingType = PricingType.StopMarket;

            var order = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);

            const PricingType expected = PricingType.StopMarket;
            var actual = order.PricingType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Verifies that a stop limit order can be created
        /// </summary>
        [TestMethod]
        public void ManualStopLimitPricingTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;
            const PricingType pricingType = PricingType.Stop | PricingType.Limit;

            var order = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);

            const PricingType expected = PricingType.StopLimit;
            var actual = order.PricingType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Verifies that an order constructed with <see cref="PricingType.Stop"/> converts automatically to <see cref="PricingType.StopMarket"/>
        /// </summary>
        [TestMethod]
        public void StopPricingTypeTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;
            const PricingType pricingType = PricingType.Stop;

            var order = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price, pricingType);

            const PricingType expected = PricingType.StopMarket;
            var actual = order.PricingType;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToStringTest()
        {
            var issued = new DateTime(2011, 12, 6);
            var expired = issued.AddMinutes(30);
            const OrderType orderType = OrderType.Buy;
            var ticker = "DE";
            const decimal shares = 5m;
            const decimal price = 100.00m;

            var target = _orderFactory.ConstructOrder(issued, expired, orderType, ticker, shares, price);

            var actual = target.ToString();

            Assert.IsTrue(actual.Contains(Enum.GetName(typeof (OrderType), orderType)));
            Assert.IsTrue(actual.Contains(ticker));
        }
    }
}
