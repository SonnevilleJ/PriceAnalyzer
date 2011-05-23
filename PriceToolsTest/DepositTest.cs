using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
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
            DateTime date = new DateTime(2011, 1, 9);

            ICashTransaction target = new Deposit
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
        public void DepositOrderTypeTest()
        {
            ICashTransaction target = new Deposit();

            const OrderType expected = OrderType.Deposit;
            OrderType actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public void DepositAmountPositiveTest()
        {
            const decimal amount = 1000.00m;

            ICashTransaction target = new Deposit
                                          {
                                              Amount = amount
                                          };

            const decimal expected = 1000.00m;
            decimal actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public void DepositAmountNegativeTest()
        {
            const decimal amount = -1000.00m;

            ICashTransaction target = new Deposit
            {
                Amount = amount
            };

            const decimal expected = 1000.00m;
            decimal actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }
    }
}
