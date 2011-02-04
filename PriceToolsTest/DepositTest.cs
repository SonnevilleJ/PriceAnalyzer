using System;
using System.Data;
using System.Data.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class DepositTest
    {
        [TestMethod]
        public void DepositEntityTest()
        {
            using (var ctx = new Container())
            {
                DateTime dateTime = new DateTime(2011, 1, 9);
                const decimal amount = 1000.00m;

                Deposit target = new Deposit(dateTime, amount);

                Assert.AreEqual(EntityState.Detached, target.EntityState);

                ctx.AddObject("Transactions", target);

                Assert.AreEqual(EntityState.Added, target.EntityState);

                ctx.SaveChanges();

                Assert.AreEqual(EntityState.Unchanged, target.EntityState);
            }
        }

        /// <summary>
        ///A test for Date
        ///</summary>
        [TestMethod()]
        public void DepositDateTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000.00m;
            ICashTransaction target = new Deposit(dateTime, amount);

            DateTime expected = dateTime;
            DateTime actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for OrderType
        ///</summary>
        [TestMethod()]
        public void DepositOrderTypeTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000.00m;
            ICashTransaction target = new Deposit(dateTime, amount);

            const OrderType expected = OrderType.Deposit;
            OrderType actual = target.OrderType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod()]
        public void DepositAmountPositiveTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000.00m;
            ICashTransaction target = new Deposit(dateTime, amount);

            const decimal expected = amount;
            decimal actual = target.Amount;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DepositAmountNegativeTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = -1000.00m;

            new Deposit(dateTime, amount);
        }

        [TestMethod()]
        public void SerializeDepositTransactionTest()
        {
            DateTime date = new DateTime(2001, 1, 1);
            const decimal amount = 100.00m;   // $100.00

            ICashTransaction target = new Deposit(date, amount);

            TestUtilities.VerifySerialization(target);
        }
    }
}