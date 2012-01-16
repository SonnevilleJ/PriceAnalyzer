using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class BuyToCoverTest
    {
        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BuyToCoverWithNegativeSharesTest()
        {
            const double shares = -5;          // bought 5 shares

            new BuyToCover
                {
                    Shares = shares,
                };
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod]
        public void BuyToCoverPricePositiveTest()
        {
            const decimal price = 100.00m;      // bought at $100.00 per share

            IShareTransaction target = new BuyToCover
            {
                Price = price,
            };

            const decimal expected = -100.00m;
            var actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BuyToCoverWithNegativeCommissionTest()
        {
            const decimal commission = -7.95m;  // bought with $7.95 commission

            new BuyToCover
                {
                    Commission = commission
                };
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod]
        public void BuyToCoverTickerTest()
        {
            const string ticker = "DE";

            IShareTransaction target = new BuyToCover
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
        public void BuyToCoverSettlementDateTest()
        {
            var date = new DateTime(2000, 1, 1);

            IShareTransaction target = new BuyToCover
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
        public void BuyToCoverOrderTypeTest()
        {
            IShareTransaction target = new BuyToCover();

            const OrderType expected = OrderType.BuyToCover;
            var actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod]
        public void BuyToCoverPriceNegativeTest()
        {
            const decimal price = -100.00m;      // bought at $100.00 per share

            IShareTransaction target = new BuyToCover
                                           {
                                               Price = price,
                                           };

            const decimal expected = -100.00m;
            var actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod]
        public void BuyToCoverSharesTest()
        {
            const double shares = 5;            // bought 5 shares

            IShareTransaction target = new BuyToCover
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
        public void BuyToCoverCommissionTest()
        {
            const decimal commission = 7.95m;   // $7.95 trading commission

            IShareTransaction target = new BuyToCover
                                           {
                                               Commission = commission
                                           };

            const decimal expected = commission;
            var actual = target.Commission;
            Assert.AreEqual(expected, actual);
        }
    }
}
