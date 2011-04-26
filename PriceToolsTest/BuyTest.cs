using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{


    /// <summary>
    ///This is a test class for Buy and is intended
    ///to contain all Buy Unit Tests
    ///</summary>
    [TestClass]
    public class BuyTest
    {
        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BuyWithNegativeSharesTest()
        {
            const double shares = -5;           // bought -5 shares - error

            new Buy
                {
                    Shares = shares,
                };
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod]
        public void BuyPriceNegativeTest()
        {
            const decimal price = -100.00m;     // bought at $100.00 per share

            IShareTransaction target = new Buy
            {
                Price = price,
            };

            const decimal expected = 100.00m;
            decimal actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BuyWithNegativeCommissionTest()
        {
            const decimal commission = -7.95m;  // bought with $-7.95 commission - error

            new Buy
                {
                    Commission = commission
                };
        }

        [TestMethod]
        public void SerializeBuyTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 1);
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            IShareTransaction expected = new Buy
                                           {
                                               SettlementDate = date,
                                               Ticker = ticker,
                                               Price = price,
                                               Shares = shares,
                                               Commission = commission
                                           };

            IShareTransaction actual = (IShareTransaction) TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod]
        public void BuyTickerTest()
        {
            const string ticker = "DE";

            IShareTransaction target = new Buy
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
        [TestMethod]
        public void BuySettlementDateTest()
        {
            DateTime date = new DateTime(2000, 1, 1);

            IShareTransaction target = new Buy
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
        [TestMethod]
        public void BuyOrderTypeTest()
        {
            IShareTransaction target = new Buy();

            const OrderType expected = OrderType.Buy;
            OrderType actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod]
        public void BuyPricePositiveTest()
        {
            const decimal price = 100.00m;      // bought at $100.00 per share

            IShareTransaction target = new Buy
                                           {
                                               Price = price,
                                           };

            const decimal expected = 100.00m;
            decimal actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod]
        public void BuySharesTest()
        {
            const double shares = 5;            // bought 5 shares

            IShareTransaction target = new Buy
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
        [TestMethod]
        public void BuyCommissionTest()
        {
            const decimal commission = 7.95m;   // $7.95 trading commission

            IShareTransaction target = new Buy
                                           {
                                               Commission = commission
                                           };

            const decimal expected = commission;
            decimal actual = target.Commission;
            Assert.AreEqual(expected, actual);
        }
    }
}
