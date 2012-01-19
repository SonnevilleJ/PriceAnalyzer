﻿using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
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
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);

            const decimal expectedValue = amount;
            var actualValue = target.GetCashBalance(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }
        
        /// <summary>
        ///A test for Withdraw
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WithdrawBeforeDepositTest()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            target.Withdraw(dateTime, amount);
        }
        
        /// <summary>
        ///A test for Withdraw
        ///</summary>
        [TestMethod]
        public void WithdrawAfterDepositTest()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            target.Deposit(dateTime, amount);
            target.Withdraw(dateTime, amount);

            const decimal expectedValue = 0.00m;
            var actualValue = target.GetCashBalance(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void RoundingCountTest1()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2011, 9, 16);
            const decimal amount = 500.00m;
            const int iterations = 10000;     // $0.05 transactions

            for (var i = 0; i < iterations; i++)
            {
                // workaround for bug with Entity Framework
                // EF will call GetHashCode, which returns a random number (I think).
                // Occasionally, two Transaction objects will return the same result for GetHashCode.
                // This results in EF getting confused and thinking the new object is already in the collection
                // and EF will not add the new object to the collection, thus changing the final count.
                var settlementDate = dateTime.AddTicks(i);

                var dividend = TransactionFactory.ConstructDeposit(settlementDate, amount / iterations);
                target.Deposit(dividend);
            }
            Assert.AreEqual(iterations, target.Transactions.Count);
        }

        [TestMethod]
        public void RoundingCountTest2()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2011, 9, 16);
            const decimal amount = 500.00m;
            const int iterations = 10000;     // $0.05 transactions

            for (var i = 0; i < iterations; i++)
            {
                // workaround for bug with Entity Framework
                // EF will call GetHashCode, which returns a random number (I think).
                // Occasionally, two Transaction objects will return the same result for GetHashCode.
                // This results in EF getting confused and thinking the new object is already in the collection
                // and EF will not add the new object to the collection, thus changing the final count.
                var settlementDate = dateTime.AddTicks(i);

                var dividend = TransactionFactory.ConstructDeposit(settlementDate, amount / iterations);
                target.Deposit(dividend);
            }

            for (var j = 0; j < iterations; j++)
            {
                // see comment above about bug with Entity Framework
                var settlementDate = dateTime.AddTicks(j);

                var withdrawal = TransactionFactory.ConstructWithdrawal(settlementDate, amount / iterations);
                target.Withdraw(withdrawal);
            }
            Assert.AreEqual(iterations * 2, target.Transactions.Count);
        }

        [TestMethod]
        public void RoundingBalanceTest()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2011, 9, 16);
            const decimal amount = 500.00m;
            const int iterations = 10000;     // $0.05 transactions

            for (var i = 0; i < iterations; i++)
            {
                // workaround for bug with Entity Framework
                // EF will call GetHashCode, which returns a random number (I think).
                // Occasionally, two Transaction objects will return the same result for GetHashCode.
                // This results in EF getting confused and thinking the new object is already in the collection
                // and EF will not add the new object to the collection, thus changing the final count.
                var settlementDate = dateTime.AddTicks(i);

                var dividend = TransactionFactory.ConstructDeposit(settlementDate, amount / iterations);
                target.Deposit(dividend);
            }

            for (var j = 0; j < iterations; j++)
            {
                // see comment above about bug with Entity Framework
                var settlementDate = dateTime.AddTicks(j);

                var withdrawal = TransactionFactory.ConstructWithdrawal(settlementDate, amount / iterations);
                target.Withdraw(withdrawal);
            }

            const decimal expected = 0.00m;
            var actual = target.GetCashBalance(dateTime);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanWithdrawDividendsTest()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            var dividendReceipt = TransactionFactory.ConstructDividendReceipt(dateTime, amount);
            target.Deposit(dividendReceipt);
            target.Withdraw(dateTime, amount);

            const decimal expectedValue = 0.00m;
            var actualValue = target.GetCashBalance(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod]
        public void DepositWithdrawalTest()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);
            target.Withdraw(dateTime, amount);

            const int expectedTransactions = 2;
            var actualTransactions = target.Transactions.Count;
            Assert.AreEqual(expectedTransactions, actualTransactions);
        }

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod]
        public void DepositIsValidTest()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            var deposit = TransactionFactory.ConstructDeposit(dateTime, amount);

            Assert.IsTrue(target.TransactionIsValid(deposit));
        }

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod]
        public void WithdrawalIsValidBeforeDepositTest()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            var withdrawal = TransactionFactory.ConstructWithdrawal(dateTime, amount);

            Assert.IsFalse(target.TransactionIsValid(withdrawal));
        }

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod]
        public void WithdrawalIsValidAfterDepositTest()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            var deposit = TransactionFactory.ConstructDeposit(dateTime, amount);
            var withdrawal = TransactionFactory.ConstructWithdrawal(dateTime, amount);
            target.Deposit(deposit);
            
            Assert.IsTrue(target.TransactionIsValid(withdrawal));
        }

        [TestMethod]
        public void UnknownIsNotValid()
        {
            ICashAccount target = new CashAccount();

            var fakeTransaction = new UnknownOrderType {OrderType = (OrderType) (-1)};

            Assert.IsFalse(target.TransactionIsValid(fakeTransaction));
        }

        private class UnknownOrderType : CashTransaction
        {
            /// <summary>
            ///   Gets the DateTime that the Transaction occurred.
            /// </summary>
            public DateTime SettlementDate { get; set; }

            /// <summary>
            ///   Gets the <see cref = "PriceTools.OrderType" /> of this Transaction.
            /// </summary>
            public OrderType OrderType { get; set; }

            /// <summary>
            ///   Gets the amount of cash in this CashTransaction.
            /// </summary>
            public decimal Amount { get; set; }
        }

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod]
        public void ModifyingTransactionsListDoesNotAffectCashAccountTest()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);
            target.Withdraw(dateTime, amount);

            var deposit = TransactionFactory.ConstructDeposit(dateTime, amount);
            target.Transactions.Add(deposit);

            const int expectedTransactions = 2;
            var actualTransactions = target.Transactions.Count;
            Assert.AreEqual(expectedTransactions, actualTransactions);
        }

        [TestMethod]
        public void ParallelAddTest()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2011, 9, 16);
            const decimal amount = 50000.00m;
            const int iterations = 100000;     // $0.05 transactions

            Parallel.For(0, iterations, i => target.Deposit(TransactionFactory.ConstructDeposit(dateTime.AddTicks(i), amount / iterations)));

            Assert.AreEqual(iterations, target.Transactions.Count);
        }
    }
}
