using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
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

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [TestMethod]
        public void DividendReceiptSettlementDateTest()
        {
            var date = new DateTime(2000, 1, 1);

            var target = new DividendReceipt
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
        public void DividendReceiptOrderTypeTest()
        {
            var target = new DividendReceipt();

            const OrderType expected = OrderType.DividendReceipt;
            var actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public void DividendReceiptAmountPositiveTest()
        {
            const decimal price = 2.00m;        // received $2.00 per share

            var target = new DividendReceipt
                             {
                                 Amount = price,
                             };

            const decimal expected = price;
            var actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }
    }
}
