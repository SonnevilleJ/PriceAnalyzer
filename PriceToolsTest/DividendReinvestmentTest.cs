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
            const double shares = -5;           // reinvested -5 shares - error

            new DividendReinvestment
                {
                    Shares = shares,
                };
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void DividendReinvestmentPriceNegativeTest()
        {
            const decimal price = -2.00m;       // reinvested $-2.00 per share - error

            IShareTransaction target = new DividendReinvestment
            {
                Price = price,
            };

            const decimal expected = 2.00m;
            decimal actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SerializeDividendReinvestmentTransactionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 1);
            const decimal price = 2.00m;        // $2.00 per share
            const double shares = 5;            // received 5 shares

            IShareTransaction expected = new DividendReinvestment
                                           {
                                               SettlementDate = date,
                                               Ticker = ticker,
                                               Price = price,
                                               Shares = shares,
                                           };

            IShareTransaction actual = (IShareTransaction)TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod()]
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
        [TestMethod()]
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
        [TestMethod()]
        public void DividendReinvestmentTickerTest()
        {
            const string ticker = "DE";

            IShareTransaction target = new DividendReinvestment
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
        public void DividendReinvestmentSettlementDateTest()
        {
            DateTime date = new DateTime(2000, 1, 1);

            IShareTransaction target = new DividendReinvestment
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
        public void DividendReinvestmentOrderTypeTest()
        {
            IShareTransaction target = new DividendReinvestment();

            const OrderType expected = OrderType.DividendReinvestment;
            OrderType actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void DividendReinvestmentPricePositiveTest()
        {
            const decimal price = 2.00m;        // reinvested $2.00 per share

            IShareTransaction target = new DividendReinvestment
                                           {
                                               Price = price,
                                           };

            const decimal expected = 2.00m;
            decimal actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        public void DividendReinvestmentSharesTest()
        {
            const double shares = 5;            // reinvested 5 shares

            IShareTransaction target = new DividendReinvestment
                                           {
                                               Shares = shares,
                                           };

            const double expected = shares;
            double actual = target.Shares;
            Assert.AreEqual(expected, actual);
        }
    }
}
