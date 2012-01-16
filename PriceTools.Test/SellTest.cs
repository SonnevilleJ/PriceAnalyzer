using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class SellTest
    {
        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod]
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
        [TestMethod]
        public void SellPricePositiveTest()
        {
            const decimal price = 100.00m;      // sold at $100.00 per share

            IShareTransaction target = new Sell
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
        public void SellWithNegativeCommissionTest()
        {
            const decimal commission = -7.95m;  // sold with $7.95 commission

            new Sell
                {
                    Commission = commission
                };
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod]
        public void SellTickerTest()
        {
            const string ticker = "DE";

            IShareTransaction target = new Sell
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
        public void SellSettlementDateTest()
        {
            var date = new DateTime(2000, 1, 1);

            IShareTransaction target = new Sell
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
        public void SellOrderTypeTest()
        {
            IShareTransaction target = new Sell();

            const OrderType expected = OrderType.Sell;
            var actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod]
        public void SellPriceNegativeTest()
        {
            const decimal price = -100.00m;      // sold at $100.00 per share

            IShareTransaction target = new Sell
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
        public void SellSharesTest()
        {
            const double shares = 5;            // sold 5 shares

            IShareTransaction target = new Sell
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
        public void SellCommissionTest()
        {
            const decimal commission = 7.95m;   // $7.95 trading commission

            IShareTransaction target = new Sell
                                           {
                                               Commission = commission
                                           };

            const decimal expected = commission;
            var actual = target.Commission;
            Assert.AreEqual(expected, actual);
        }
    }
}
