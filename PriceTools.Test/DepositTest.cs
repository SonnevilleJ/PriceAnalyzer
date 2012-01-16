using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class DepositTest
    {
        /// <summary>
        ///A test for Date
        ///</summary>
        [TestMethod]
        public void DepositSettlementDateTest()
        {
            var date = new DateTime(2011, 1, 9);

            var target = new Deposit
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
        public void DepositOrderTypeTest()
        {
            var target = new Deposit();

            const OrderType expected = OrderType.Deposit;
            var actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public void DepositAmountPositiveTest()
        {
            const decimal amount = 1000.00m;

            var target = new Deposit
                             {
                                 Amount = amount
                             };

            const decimal expected = 1000.00m;
            var actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public void DepositAmountNegativeTest()
        {
            const decimal amount = -1000.00m;

            var target = new Deposit
                             {
                                 Amount = amount
                             };

            const decimal expected = 1000.00m;
            var actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }
    }
}
