using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class DividendReinvestmentTest
    {
        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentWithNegativeSharesTest()
        {
            const double shares = -5;           // reinvested -5 shares - error

            new DividendReinvestment
                {
                    Shares = shares,
                };
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod]
        public void DividendReinvestmentPriceNegativeTest()
        {
            const decimal price = -2.00m;       // reinvested $-2.00 per share - error

            IShareTransaction target = new DividendReinvestment
            {
                Price = price,
            };

            const decimal expected = 2.00m;
            var actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentCommissionNegativeTest()
        {
            const decimal commission = -7.95m;  // commission of $7.95 - error

            new DividendReinvestment
                {
                    Commission = commission
                };
        }
        
        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentCommissionPositiveTest()
        {
            const decimal commission = 7.95m;   // commission of $7.95 - error

            new DividendReinvestment
                {
                    Commission = commission
                };
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod]
        public void DividendReinvestmentTickerTest()
        {
            const string ticker = "DE";

            IShareTransaction target = new DividendReinvestment
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
        public void DividendReinvestmentSettlementDateTest()
        {
            var date = new DateTime(2000, 1, 1);

            IShareTransaction target = new DividendReinvestment
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
        public void DividendReinvestmentOrderTypeTest()
        {
            IShareTransaction target = new DividendReinvestment();

            const OrderType expected = OrderType.DividendReinvestment;
            var actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod]
        public void DividendReinvestmentPricePositiveTest()
        {
            const decimal price = 2.00m;        // reinvested $2.00 per share

            IShareTransaction target = new DividendReinvestment
                                           {
                                               Price = price,
                                           };

            const decimal expected = 2.00m;
            var actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod]
        public void DividendReinvestmentSharesTest()
        {
            const double shares = 5;            // reinvested 5 shares

            IShareTransaction target = new DividendReinvestment
                                           {
                                               Shares = shares,
                                           };

            const double expected = shares;
            var actual = target.Shares;
            Assert.AreEqual(expected, actual);
        }
    }
}
