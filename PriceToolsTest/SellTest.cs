using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class SellTest
    {
        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellWithNegativeSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 100.00m;      // sold at $100.00 per share
            const double shares = -5;           // sold 5 shares
            const decimal commission = 7.95m;   // sold with $7.95 commission
            new Sell(date, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellWithNegativePriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = -100.00m;     // sold at $-100.00 per share - error
            const double shares = 5;            // sold 5 shares
            const decimal commission = 7.95m;   // sold with $7.95 commission
            new Sell(date, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellWithNegativeCommissionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 100.00m;      // sold at $100.00 per share
            const double shares = 5;            // sold 5 shares
            const decimal commission = -7.95m;  // sold with $7.95 commission
            new Sell(date, ticker, price, shares, commission);
        }

        [TestMethod()]
        public void SerializeSellTransactionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 1);
            const decimal price = 100.00m;   // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            IShareTransaction target = new Sell(date, ticker, price, shares, commission);

            TestUtilities.VerifySerialization(target);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod()]
        public void SellTickerTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 100.00m;      // sold at $100.00 per share
            const double shares = 5;            // sold 5 shares
            const decimal commission = 7.95m;   // sold with $7.95 commission

            IShareTransaction target = new Sell(date, ticker, price, shares, commission);

            const string expected = ticker;
            string actual = target.Ticker;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [TestMethod()]
        public void SellSettlementDateTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 100.00m;      // sold at $100.00 per share
            const double shares = 5;            // sold 5 shares
            const decimal commission = 7.95m;   // sold with $7.95 commission

            IShareTransaction target = new Sell(date, ticker, price, shares, commission);

            DateTime expected = date;
            DateTime actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for OrderType
        ///</summary>
        [TestMethod()]
        public void SellOrderTypeTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Sell;
            const decimal price = 100.00m;      // sold at $100.00 per share
            const double shares = 5;            // sold 5 shares
            const decimal commission = 7.95m;   // sold with $7.95 commission

            IShareTransaction target = new Sell(date, ticker, price, shares, commission);

            const OrderType expected = type;
            OrderType actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void SellPriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 100.00m;      // sold at $100.00 per share
            const double shares = 5;            // sold 5 shares
            const decimal commission = 7.95m;   // sold with $7.95 commission

            IShareTransaction target = new Sell(date, ticker, price, shares, commission);

            const decimal expected = -100.00m;
            decimal actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        public void SellSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 100.00m;      // sold at $100.00 per share
            const double shares = 5;            // sold 5 shares
            const decimal commission = 7.95m;   // sold with $7.95 commission

            IShareTransaction target = new Sell(date, ticker, price, shares, commission);

            const double expected = shares;
            double actual = target.Shares;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        public void SellCommissionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 100.00m;      // sold at $100.00 per share
            const double shares = 5;            // sold 5 shares
            const decimal commission = 7.95m;   // $7.95 trading commission

            IShareTransaction target = new Sell(date, ticker, price, shares, commission);

            const decimal expected = commission;
            decimal actual = target.Commission;
            Assert.AreEqual(expected, actual);
        }
    }
}
