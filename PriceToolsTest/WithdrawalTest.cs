using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class WithdrawalTest
    {
        /// <summary>
        ///A test for Date
        ///</summary>
        [TestMethod()]
        public void WithdrawalDateTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000.00m;
            ICashTransaction target = new Withdrawal(dateTime, amount);

            DateTime expected = dateTime;
            DateTime actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for OrderType
        ///</summary>
        [TestMethod()]
        public void WithdrawalOrderTypeTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000.00m;
            ICashTransaction target = new Withdrawal(dateTime, amount);

            const OrderType expected = OrderType.Withdrawal;
            OrderType actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod()]
        public void WithdrawalAmountPositiveTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000.00m;
            ICashTransaction target = new Withdrawal(dateTime, amount);

            const decimal expected = amount;
            decimal actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WithdrawalAmountNegativeTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = -1000.00m;
            new Withdrawal(dateTime, amount);
        }

        [TestMethod()]
        public void SerializeWithdrawalTransactionTest()
        {
            DateTime date = new DateTime(2001, 1, 1);
            const decimal amount = 100.00m;   // $100.00

            ICashTransaction target = new Withdrawal(date, amount);

            TestUtilities.VerifySerialization(target);
        }
    }
}
