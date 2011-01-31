using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class DividendReinvestmentTest
    {
        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentWithNegativeSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const decimal price = 2.00m;        // reinvested $2.00 per share
            const double shares = -5;           // reinvested -5 shares - error

            new DividendReinvestment(date, ticker, price, shares);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentWithNegativePriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const decimal price = -2.00m;       // reinvested $-2.00 per share - error
            const double shares = 5;            // received 5 shares

            new DividendReinvestment(date, ticker, price, shares);
        }

        [TestMethod()]
        public void SerializeDividendReinvestmentTransactionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 1);
            const decimal price = 2.00m;        // $2.00 per share
            const double shares = 5;            // received 5 shares

            IShareTransaction target = new DividendReinvestment(date, ticker, price, shares);

            TestUtilities.VerifySerialization(target);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod()]
        public void DividendReinvestmentTickerTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 2.00m;        // reinvested $2.00 per share
            const double shares = 5;            // reinvested 5 shares

            IShareTransaction target = new DividendReinvestment(date, ticker, price, shares);

            const string expected = ticker;
            string actual = target.Ticker;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [TestMethod()]
        public void DividendReinvestmentSettlementDateTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 2.00m;        // reinvested $2.00 per share
            const double shares = 5;            // reinvested 5 shares

            IShareTransaction target = new DividendReinvestment(date, ticker, price, shares);

            DateTime expected = date;
            DateTime actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for OrderType
        ///</summary>
        [TestMethod()]
        public void DividendReinvestmentOrderTypeTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.DividendReinvestment;
            const decimal price = 2.00m;        // reinvested $2.00 per share
            const double shares = 5;            // reinvested 5 shares

            IShareTransaction target = new DividendReinvestment(date, ticker, price, shares);

            const OrderType expected = type;
            OrderType actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void DividendReinvestmentPriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 2.00m;        // reinvested $2.00 per share
            const double shares = 5;            // reinvested 5 shares

            IShareTransaction target = new DividendReinvestment(date, ticker, price, shares);

            const decimal expected = price;
            decimal actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        public void DividendReinvestmentSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const decimal price = 2.00m;        // reinvested $2.00 per share
            const double shares = 5;            // reinvested 5 shares

            IShareTransaction target = new DividendReinvestment(date, ticker, price, shares);

            const double expected = shares;
            double actual = target.Shares;
            Assert.AreEqual(expected, actual);
        }
    }
}
