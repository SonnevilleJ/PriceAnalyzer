using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    /// <summary>
    ///This is a test class for ICashAccountTest and is intended
    ///to contain all ICashAccountTest Unit Tests
    ///</summary>
    [TestClass]
    public class CashAccountTest
    {
        private readonly ICashAccountFactory _cashAccountFactory;
        private readonly ITransactionFactory _transactionFactory;

        public CashAccountTest()
        {
            _cashAccountFactory = new CashAccountFactory();
            _transactionFactory = new TransactionFactory();
        }

        /// <summary>
        ///A test for Deposit
        ///</summary>
        [TestMethod]
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
        
        /// <summary>
        ///A test for Withdraw
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WithdrawBeforeDepositTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
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
            var target = _cashAccountFactory.ConstructCashAccount();
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
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2011, 9, 16);
            const decimal amount = 500.00m;
            const int iterations = 10000;     // $0.05 transactions

            for (var i = 0; i < iterations; i++)
            {
                var dividend = _transactionFactory.ConstructDeposit(dateTime, amount / iterations);
                target.Deposit(dividend);
            }
            Assert.AreEqual(iterations, target.Transactions.Count);
        }

        [TestMethod]
        public void RoundingCountTest2()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2011, 9, 16);
            const decimal amount = 500.00m;
            const int iterations = 10000;     // $0.05 transactions

            for (var i = 0; i < iterations; i++)
            {
                var dividend = _transactionFactory.ConstructDeposit(dateTime, amount / iterations);
                target.Deposit(dividend);
            }

            for (var j = 0; j < iterations; j++)
            {
                var withdrawal = _transactionFactory.ConstructWithdrawal(dateTime, amount / iterations);
                target.Withdraw(withdrawal);
            }
            Assert.AreEqual(iterations * 2, target.Transactions.Count);
        }

        [TestMethod]
        public void RoundingBalanceTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2011, 9, 16);
            const decimal amount = 500.00m;
            const int iterations = 10000;     // $0.05 transactions

            for (var i = 0; i < iterations; i++)
            {
                var dividend = _transactionFactory.ConstructDeposit(dateTime, amount / iterations);
                target.Deposit(dividend);
            }

            for (var j = 0; j < iterations; j++)
            {
                var withdrawal = _transactionFactory.ConstructWithdrawal(dateTime, amount / iterations);
                target.Withdraw(withdrawal);
            }

            const decimal expected = 0.00m;
            var actual = target.GetCashBalance(dateTime);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
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

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod]
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

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod]
        public void DepositIsValidTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            var deposit = _transactionFactory.ConstructDeposit(dateTime, amount);

            Assert.IsTrue(target.TransactionIsValid(deposit));
        }

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod]
        public void WithdrawalIsValidBeforeDepositTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            var withdrawal = _transactionFactory.ConstructWithdrawal(dateTime, amount);

            Assert.IsFalse(target.TransactionIsValid(withdrawal));
        }

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod]
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

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod]
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

        [TestMethod]
        public void ParallelAddTest()
        {
            var target = _cashAccountFactory.ConstructCashAccount();
            var dateTime = new DateTime(2011, 9, 16);
            const decimal amount = 50000.00m;
            const int iterations = 100000;     // $0.05 transactions

            Parallel.For(0, iterations, i => target.Deposit(_transactionFactory.ConstructDeposit(dateTime.AddTicks(i), amount / iterations)));

            Assert.AreEqual(iterations, target.Transactions.Count);
        }
    }
}
