using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class DividendReceiptTest
    {
        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReceiptWithNegativeSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const decimal price = 2.0m;         // dividend of $2.00 per share
            const double shares = -5;           // received -5 shares - error

            new DividendReceipt(date, ticker, price, shares);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReceiptWithNegativePriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const decimal price = -2.0m;        // bought at $-2.00 per share - error
            const double shares = 5;            // received 5 shares

            new DividendReceipt(date, ticker, price, shares);
        }

        [TestMethod()]
        public void SerializeDividendReceiptTransactionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 17);
            const decimal price = 2.00m;     // $2.00 per share
            const double shares = 5;            // received 5 shares

            IShareTransaction target = new DividendReceipt(date, ticker, price, shares);

            TestUtilities.VerifySerialization(target);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod()]
        public void DividendReceiptTickerTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 2.00m;        // received $2.00 per share
            const double shares = 5;            // received 5 shares

            IShareTransaction target = new DividendReceipt(date, ticker, price, shares);

            const string expected = ticker;
            string actual = target.Ticker;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [TestMethod()]
        public void DividendReceiptSettlementDateTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 2.00m;        // received $2.00 per share
            const double shares = 5;            // received 5 shares

            IShareTransaction target = new DividendReceipt(date, ticker, price, shares);

            DateTime expected = date;
            DateTime actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for OrderType
        ///</summary>
        [TestMethod()]
        public void DividendReceiptOrderTypeTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.DividendReceipt;
            const decimal price = 2.00m;        // received $2.00 per share
            const double shares = 5;            // received 5 shares

            IShareTransaction target = new DividendReceipt(date, ticker, price, shares);

            const OrderType expected = type;
            OrderType actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void DividendReceiptPriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 2.00m;        // received $2.00 per share
            const double shares = 5;            // received 5 shares

            IShareTransaction target = new DividendReceipt(date, ticker, price, shares);

            const decimal expected = price;
            decimal actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        public void DividendReceiptSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 2.00m;        // received $2.00 per share
            const double shares = 5;            // received 5 shares

            IShareTransaction target = new DividendReceipt(date, ticker, price, shares);

            const double expected = shares;
            double actual = target.Shares;
            Assert.AreEqual(expected, actual);
        }
    }
}
