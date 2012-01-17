using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class TransactionFactoryTest
    {
        [TestMethod]
        public void ConstructDepositTest()
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var target = TransactionFactory.ConstructDeposit(date, amount);

            Assert.AreEqual(OrderType.Deposit, target.OrderType);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(Math.Abs(amount), Math.Abs(target.Amount));
        }

        [TestMethod]
        public void ConstructWithdrawalTest()
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var target = TransactionFactory.ConstructWithdrawal(date, amount);

            Assert.AreEqual(OrderType.Withdrawal, target.OrderType);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(Math.Abs(amount), Math.Abs(target.Amount));
        }

        [TestMethod]
        public void ConstructDividendReceiptTest()
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var target = TransactionFactory.ConstructDividendReceipt(date, amount);

            Assert.AreEqual(OrderType.DividendReceipt, target.OrderType);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(Math.Abs(amount), Math.Abs(target.Amount));
        }

        [TestMethod]
        public void ConstructCashTransactionDepositTest()
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var expected = TransactionFactory.ConstructDeposit(date, amount);
            var actual = TransactionFactory.ConstructCashTransaction(OrderType.Deposit, date, amount);
            TestUtilities.AssertSameState(expected, actual);
        }

        [TestMethod]
        public void ConstructCashTransactionWithdrawalTest()
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var expected = TransactionFactory.ConstructWithdrawal(date, amount);
            var actual = TransactionFactory.ConstructCashTransaction(OrderType.Withdrawal, date, amount);
            TestUtilities.AssertSameState(expected, actual);
        }

        [TestMethod]
        public void ConstructCashTransactionDividendReceiptTest()
        {
            var date = new DateTime(2012, 1, 17);
            const decimal amount = 10000.00m;

            var expected = TransactionFactory.ConstructDividendReceipt(date, amount);
            var actual = TransactionFactory.ConstructCashTransaction(OrderType.DividendReceipt, date, amount);
            TestUtilities.AssertSameState(expected, actual);
        }
    }
}
