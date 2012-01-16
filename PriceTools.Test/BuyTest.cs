using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
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
            var actual = target.Price;
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
            var actual = target.Ticker;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [TestMethod]
        public void BuySettlementDateTest()
        {
            var date = new DateTime(2000, 1, 1);

            IShareTransaction target = new Buy
                                           {
                                               SettlementDate = date,
                                           };

            var expected = date;
            var actual = target.SettlementDate;
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
            var actual = target.OrderType;
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
            var actual = target.Price;
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
            var actual = target.Shares;
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
            var actual = target.Commission;
            Assert.AreEqual(expected, actual);
        }
    }
}
