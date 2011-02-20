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
            DateTime date = new DateTime(2011, 1, 9);

            ICashTransaction target = new Withdrawal
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
        public void WithdrawalOrderTypeTest()
        {
            ICashTransaction target = new Withdrawal();

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
            const decimal amount = 1000.00m;

            ICashTransaction target = new Withdrawal
                                          {
                                              Amount = amount
                                          };

            const decimal expected = -1000.00m;
            decimal actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod()]
        public void WithdrawalAmountNegativeTest()
        {
            const decimal amount = -1000.00m;

            ICashTransaction target = new Withdrawal
            {
                Amount = amount
            };

            const decimal expected = -1000.00m;
            decimal actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SerializeWithdrawalTransactionTest()
        {
            DateTime date = new DateTime(2001, 1, 1);
            const decimal amount = 100.00m;   // $100.00

            ICashTransaction expected = new Withdrawal
                                          {
                                              SettlementDate = date,
                                              Amount = amount
                                          };

            ICashTransaction actual = (ICashTransaction)TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void EntityWithdrawalTransactionTest()
        {
            DateTime date = new DateTime(2001, 1, 1);
            const decimal amount = 100.00m;   // $100.00

            ICashTransaction target = new Withdrawal
                                          {
                                              SettlementDate = date,
                                              Amount = amount
                                          };

            TestUtilities.VerifyTransactionEntity(target);
        }
    }
}
