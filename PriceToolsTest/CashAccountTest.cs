using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for ICashAccountTest and is intended
    ///to contain all ICashAccountTest Unit Tests
    ///</summary>
    [TestClass]
    public class CashAccountTest
    {
        /// <summary>
        ///A test for Deposit
        ///</summary>
        [TestMethod]
        public void DepositTest()
        {
            ICashAccount target = new CashAccount();
            DateTime dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);

            const decimal expectedValue = amount;
            decimal actualValue = target.GetCashBalance(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }
        
        /// <summary>
        ///A test for Withdraw
        ///</summary>
        [TestMethod]
        public void WithdrawTest()
        {
            ICashAccount target = new CashAccount();
            DateTime dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);
            target.Withdraw(dateTime, amount);

            const decimal expectedValue = 0.00m;
            decimal actualValue = target.GetCashBalance(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod]
        public void TransactionsTest()
        {
            ICashAccount target = new CashAccount();
            DateTime dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);
            target.Withdraw(dateTime, amount);

            const int expectedTransactions = 2;
            int actualTransactions = target.Transactions.Count;
            Assert.AreEqual(expectedTransactions, actualTransactions);
        }

        [TestMethod]
        public void SerializeCashAccountTest()
        {
            DateTime date = new DateTime(2011, 1, 16);
            const decimal amount = 10000m;
            ICashAccount target = new CashAccount();
            target.Deposit(date, amount);
            target.Withdraw(date, amount);

            ICashAccount expected = target;
            ICashAccount actual = ((ICashAccount) TestUtilities.Serialize(target));
            Assert.AreEqual(expected, actual);
        }
    }
}
