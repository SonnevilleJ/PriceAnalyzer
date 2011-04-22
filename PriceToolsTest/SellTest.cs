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
            const double shares = -5;           // sold 5 shares

            new Sell
                {
                    Shares = shares,
                };
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void SellPricePositiveTest()
        {
            const decimal price = 100.00m;      // sold at $100.00 per share

            IShareTransaction target = new Sell
            {
                Price = price,
            };

            const decimal expected = -100.00m;
            decimal actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellWithNegativeCommissionTest()
        {
            const decimal commission = -7.95m;  // sold with $7.95 commission

            new Sell
                {
                    Commission = commission
                };
        }

        [TestMethod()]
        public void SerializeSellTransactionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 1);
            const decimal price = 100.00m;   // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            IShareTransaction expected = new Sell
                                           {
                                               SettlementDate = date,
                                               Ticker = ticker,
                                               Price = price,
                                               Shares = shares,
                                               Commission = commission
                                           };

            IShareTransaction actual = (IShareTransaction)TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod()]
        public void SellTickerTest()
        {
            const string ticker = "DE";

            IShareTransaction target = new Sell
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
        public void SellSettlementDateTest()
        {
            DateTime date = new DateTime(2000, 1, 1);

            IShareTransaction target = new Sell
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
        public void SellOrderTypeTest()
        {
            IShareTransaction target = new Sell();

            const OrderType expected = OrderType.Sell;
            OrderType actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void SellPriceNegativeTest()
        {
            const decimal price = -100.00m;      // sold at $100.00 per share

            IShareTransaction target = new Sell
                                           {
                                               Price = price,
                                           };

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
            const double shares = 5;            // sold 5 shares

            IShareTransaction target = new Sell
                                           {
                                               Shares = shares,
                                           };

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
            const decimal commission = 7.95m;   // $7.95 trading commission

            IShareTransaction target = new Sell
                                           {
                                               Commission = commission
                                           };

            const decimal expected = commission;
            decimal actual = target.Commission;
            Assert.AreEqual(expected, actual);
        }
    }
}
