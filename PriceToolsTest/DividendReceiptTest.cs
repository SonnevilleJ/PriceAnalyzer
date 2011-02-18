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
            const double shares = -5;           // received -5 shares - error

            new DividendReceipt
                {
                    Shares = shares,
                };
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void DividendReceiptWithNegativePriceTest()
        {
            const decimal price = -2.0m;        // bought at $-2.00 per share - error

            new DividendReceipt
                {
                    Price = price,
                };
        }

        [TestMethod()]
        public void EntityDividendReceiptTransactionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 17);
            const decimal price = 2.00m;     // $2.00 per share
            const double shares = 5;            // received 5 shares

            IShareTransaction target = new DividendReceipt
                                           {
                                               SettlementDate = date,
                                               Ticker = ticker,
                                               Price = price,
                                               Shares = shares,
                                           };

            TestUtilities.VerifyTransactionEntity(target);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentCommissionNegativeTest()
        {
            const decimal commission = -7.95m;  // commission of $7.95 - error

            new DividendReceipt
            {
                Commission = commission
            };
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentCommissionPositiveTest()
        {
            const decimal commission = 7.95m;   // commission of $7.95 - error

            new DividendReceipt
            {
                Commission = commission
            };
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod()]
        public void DividendReceiptTickerTest()
        {
            const string ticker = "DE";

            IShareTransaction target = new DividendReceipt
                                           {
                                               Ticker = ticker,
                                           };

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
            DateTime date = new DateTime(2000, 1, 1);

            IShareTransaction target = new DividendReceipt
                                           {
                                               SettlementDate = date,
                                           };

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
            IShareTransaction target = new DividendReceipt();

            const OrderType expected = OrderType.DividendReceipt;
            OrderType actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void DividendReceiptPriceTest()
        {
            const decimal price = 2.00m;        // received $2.00 per share

            IShareTransaction target = new DividendReceipt
                                           {
                                               Price = price,
                                           };

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
            const double shares = 5;            // received 5 shares

            IShareTransaction target = new DividendReceipt
                                           {
                                               Shares = shares,
                                           };

            const double expected = shares;
            double actual = target.Shares;
            Assert.AreEqual(expected, actual);
        }
    }
}
