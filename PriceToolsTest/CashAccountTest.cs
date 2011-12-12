using System.Threading.Tasks;
using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        public void WithdrawTest()
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

                var dividend = new Deposit { Amount = amount / iterations, SettlementDate = settlementDate };
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

                var dividend = new Deposit { Amount = amount / iterations, SettlementDate = settlementDate };
                target.Deposit(dividend);
            }

            for (var j = 0; j < iterations; j++)
            {
                // see comment above about bug with Entity Framework
                var settlementDate = dateTime.AddTicks(j);

                var withdrawal = new Withdrawal { Amount = amount / iterations, SettlementDate = settlementDate };
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

                var dividend = new Deposit { Amount = amount / iterations, SettlementDate = settlementDate };
                target.Deposit(dividend);
            }

            for (var j = 0; j < iterations; j++)
            {
                // see comment above about bug with Entity Framework
                var settlementDate = dateTime.AddTicks(j);

                var withdrawal = new Withdrawal { Amount = amount / iterations, SettlementDate = settlementDate };
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
            var dividendReceipt = new DividendReceipt {Amount = amount, SettlementDate = dateTime};
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
        public void TransactionsTest()
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
        public void ModifyingTransactionsListDoesNotAffectCashAccountTest()
        {
            ICashAccount target = new CashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;
            target.Deposit(dateTime, amount);
            target.Withdraw(dateTime, amount);
            
            var deposit = new Deposit {SettlementDate = dateTime, Amount = amount};
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

            Parallel.For(0, iterations, i => target.Deposit(new Deposit {Amount = amount/iterations, SettlementDate = dateTime.AddTicks(i)}));

            Assert.AreEqual(iterations, target.Transactions.Count);
        }
    }
}
