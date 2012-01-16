using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class WithdrawalTest
    {
        /// <summary>
        ///A test for Date
        ///</summary>
        [TestMethod]
        public void WithdrawalSettlementDateTest()
        {
            var date = new DateTime(2011, 1, 9);

            ICashTransaction target = new Withdrawal
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
        public void WithdrawalOrderTypeTest()
        {
            ICashTransaction target = new Withdrawal();

            const OrderType expected = OrderType.Withdrawal;
            var actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public void WithdrawalAmountPositiveTest()
        {
            const decimal amount = 1000.00m;

            ICashTransaction target = new Withdrawal
                                          {
                                              Amount = amount
                                          };

            const decimal expected = -1000.00m;
            var actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public void WithdrawalAmountNegativeTest()
        {
            const decimal amount = -1000.00m;

            ICashTransaction target = new Withdrawal
            {
                Amount = amount
            };

            const decimal expected = -1000.00m;
            var actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }
    }
}
