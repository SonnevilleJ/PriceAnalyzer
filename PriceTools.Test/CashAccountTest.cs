﻿using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class CashAccountTest
    {
        private readonly ICashAccountFactory _cashAccountFactory;
        private readonly ITransactionFactory _transactionFactory;

        public CashAccountTest()
        {
            _cashAccountFactory = new CashAccountFactory();
            _transactionFactory = new TransactionFactory();
        }

        [Test]
        public void DepositTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);

            const decimal expectedValue = amount;
            var actualValue = target.GetCashBalance(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WithdrawBeforeDepositTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            target.Withdraw(dateTime, amount);
        }
        
        [Test]
        public void WithdrawAfterDepositTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            target.Deposit(dateTime, amount);
            target.Withdraw(dateTime, amount);

            const decimal expectedValue = 0.00m;
            var actualValue = target.GetCashBalance(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void RoundingCountTest1()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2011, 9, 16);
            const int iterations = 1000;
            const decimal amount = 50.00m / iterations; // $0.05 transactions

            for (var i = 0; i < iterations; i++)
            {
                var dividend = _transactionFactory.ConstructDeposit(dateTime, amount);
                target.Deposit(dividend);
            }
            Assert.AreEqual(iterations, target.Transactions.Count);
        }

        [Test]
        public void RoundingCountTest2()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2011, 9, 16);
            const int iterations = 1000;
            const decimal amount = 50.00m / iterations; // $0.05 transactions

            for (var i = 0; i < iterations; i++)
            {
                var dividend = _transactionFactory.ConstructDeposit(dateTime, amount);
                target.Deposit(dividend);
            }

            for (var j = 0; j < iterations; j++)
            {
                var withdrawal = _transactionFactory.ConstructWithdrawal(dateTime, amount);
                target.Withdraw(withdrawal);
            }
            Assert.AreEqual(iterations * 2, target.Transactions.Count);
        }

        [Test]
        public void RoundingBalanceTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2011, 9, 16);
            const int iterations = 1000;
            const decimal amount = 50.00m / iterations; // $0.05 transactions

            for (var i = 0; i < iterations; i++)
            {
                var dividend = _transactionFactory.ConstructDeposit(dateTime, amount);
                target.Deposit(dividend);
            }

            for (var j = 0; j < iterations; j++)
            {
                var withdrawal = _transactionFactory.ConstructWithdrawal(dateTime, amount);
                target.Withdraw(withdrawal);
            }

            const decimal expected = 0.00m;
            var actual = target.GetCashBalance(dateTime);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanWithdrawDividendsTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            var dividendReceipt = _transactionFactory.ConstructDividendReceipt(dateTime, amount);
            target.Deposit(dividendReceipt);
            target.Withdraw(dateTime, amount);

            const decimal expectedValue = 0.00m;
            var actualValue = target.GetCashBalance(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void DepositWithdrawalTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);
            target.Withdraw(dateTime, amount);

            const int expectedTransactions = 2;
            var actualTransactions = target.Transactions.Count;
            Assert.AreEqual(expectedTransactions, actualTransactions);
        }

        [Test]
        public void DepositIsValidTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            var deposit = _transactionFactory.ConstructDeposit(dateTime, amount);

            Assert.IsTrue(target.TransactionIsValid(deposit));
        }

        [Test]
        public void WithdrawalIsValidBeforeDepositTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            var withdrawal = _transactionFactory.ConstructWithdrawal(dateTime, amount);

            Assert.IsFalse(target.TransactionIsValid(withdrawal));
        }

        [Test]
        public void WithdrawalIsValidAfterDepositTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            var deposit = _transactionFactory.ConstructDeposit(dateTime, amount);
            var withdrawal = _transactionFactory.ConstructWithdrawal(dateTime, amount);
            target.Deposit(deposit);
            
            Assert.IsTrue(target.TransactionIsValid(withdrawal));
        }

        [Test]
        public void ModifyingTransactionsListDoesNotAffectCashAccountTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);
            target.Withdraw(dateTime, amount);

            var deposit = _transactionFactory.ConstructDeposit(dateTime, amount);
            target.Transactions.Add(deposit);

            const int expectedTransactions = 2;
            var actualTransactions = target.Transactions.Count;
            Assert.AreEqual(expectedTransactions, actualTransactions);
        }
    }
}
