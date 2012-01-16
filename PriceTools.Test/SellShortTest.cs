using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class SellShortTest
    {
        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellShortWithNegativeSharesTest()
        {
            const string ticker = "DE";
            var date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = 100.00m;      // sold at $100.00 per share
            const double shares = -5;           // sold 5 shares
            const decimal commission = 7.95m;  // sold with $7.95 commission
            TransactionFactory.CreateShareTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod]
        public void SellShortWithNegativePriceTest()
        {
            const string ticker = "DE";
            var date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = -100.00m;     // sold at $-100.00 per share - error
            const double shares = 5;            // sold 5 shares
            const decimal commission = 7.95m;   // sold with $7.95 commission
            TransactionFactory.CreateShareTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellShortWithNegativeCommissionTest()
        {
            const string ticker = "DE";
            var date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = 100.00m;      // sold at $100.00 per share
            const double shares = 5;            // sold 5 shares
            const decimal commission = -7.95m;  // sold with $7.95 commission
            TransactionFactory.CreateShareTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod]
        public void SellShortTickerTest()
        {
            const string ticker = "DE";
            var date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = 100.00m;       // bought at $100.00 per share
            const double shares = 5;            // bought 5 shares
            const decimal commission = 7.95m;   // bought with $7.95 commission

            var target = TransactionFactory.CreateShareTransaction(date, type, ticker, price, shares, commission);

            const string expected = ticker;
            var actual = target.Ticker;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [TestMethod]
        public void SellShortSettlementDateTest()
        {
            const string ticker = "DE";
            var date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = 100.00m;       // bought at $100.00 per share
            const double shares = 5;            // bought 5 shares
            const decimal commission = 7.95m;   // bought with $7.95 commission

            var target = TransactionFactory.CreateShareTransaction(date, type, ticker, price, shares, commission);

            var expected = date;
            var actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for OrderType
        ///</summary>
        [TestMethod]
        public void SellShortOrderTypeTest()
        {
            const string ticker = "DE";
            var date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = 100.00m;       // bought at $100.00 per share
            const double shares = 5;            // bought 5 shares
            const decimal commission = 7.95m;   // bought with $7.95 commission

            var target = TransactionFactory.CreateShareTransaction(date, type, ticker, price, shares, commission);

            const OrderType expected = type;
            var actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod]
        public void SellShortPriceTest()
        {
            const string ticker = "DE";
            var date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = 100.00m;      // bought at $100.00 per share
            const double shares = 5;            // bought 5 shares
            const decimal commission = 7.95m;   // bought with $7.95 commission

            var target = TransactionFactory.CreateShareTransaction(date, type, ticker, price, shares, commission);

            const decimal expected = 100.00m;
            var actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod]
        public void SellShortSharesTest()
        {
            const string ticker = "DE";
            var date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = 100.00m;       // bought at $100.00 per share
            const double shares = 5;            // bought 5 shares
            const decimal commission = 7.95m;   // bought with $7.95 commission

            var target = TransactionFactory.CreateShareTransaction(date, type, ticker, price, shares, commission);

            const double expected = shares;
            var actual = target.Shares;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod]
        public void SellShortCommissionTest()
        {
            const string ticker = "DE";
            var date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = 100.00m;       // cover at $100.00 per share
            const double shares = 5;            // cover 5 shares
            const decimal commission = 7.95m;   // $7.95 trading commission

            var target = TransactionFactory.CreateShareTransaction(date, type, ticker, price, shares, commission);

            const decimal expected = commission;
            var actual = target.Commission;
            Assert.AreEqual(expected, actual);
        }
    }
}
