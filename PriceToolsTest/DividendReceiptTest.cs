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
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public void DividendReceiptAmountNegativeTest()
        {
            const decimal price = -2.0m;        // bought at $-2.00 per share - error

            new DividendReceipt
                {
                    Amount = price,
                };
        }

        [TestMethod]
        public void SerializeDividendReceiptTransactionTest()
        {
            DateTime date = new DateTime(2001, 1, 17);
            const decimal price = 2.00m;        // $2.00 per share
            const double shares = 5;            // received 5 shares

            DividendReceipt expected = new DividendReceipt
                                           {
                                               SettlementDate = date,
                                               Amount = price * (decimal)shares
                                           };

            ICashTransaction actual = (ICashTransaction)TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [TestMethod]
        public void DividendReceiptSettlementDateTest()
        {
            DateTime date = new DateTime(2000, 1, 1);

            DividendReceipt target = new DividendReceipt
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
        public void DividendReceiptOrderTypeTest()
        {
            DividendReceipt target = new DividendReceipt();

            const OrderType expected = OrderType.DividendReceipt;
            OrderType actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public void DividendReceiptAmountPositiveTest()
        {
            const decimal price = 2.00m;        // received $2.00 per share

            DividendReceipt target = new DividendReceipt
                                           {
                                               Amount = price,
                                           };

            const decimal expected = price;
            decimal actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }
    }
}
