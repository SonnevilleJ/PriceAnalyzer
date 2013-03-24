using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class MarginableCashAccountTest
    {
        private readonly ICashAccountFactory _cashAccountFactory;
        private readonly ITransactionFactory _transactionFactory;

        public MarginableCashAccountTest()
        {
            _cashAccountFactory = new CashAccountFactory();
            _transactionFactory = new TransactionFactory();
        }

        /// <summary>
        ///A test for Withdraw
        ///</summary>
        [TestMethod]
        public void WithdrawBeforeDepositTest()
        {
            var target = _cashAccountFactory.ConstructMarginableCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            target.Withdraw(dateTime, amount);

            Assert.AreEqual(-amount, target.GetCashBalance(dateTime));
        }

        /// <summary>
        ///A test for Withdraw
        ///</summary>
        [TestMethod]
        public void WithdrawAfterDepositTest()
        {
            var target = _cashAccountFactory.ConstructMarginableCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            target.Deposit(dateTime, amount);
            target.Withdraw(dateTime, amount);

            const decimal expectedValue = 0.00m;
            var actualValue = target.GetCashBalance(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        ///A test for Transactions
        ///</summary>
        [TestMethod]
        public void WithdrawalIsValidBeforeDepositTest()
        {
            var target = _cashAccountFactory.ConstructMarginableCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            var withdrawal = _transactionFactory.ConstructWithdrawal(dateTime, amount);

            Assert.IsTrue(target.TransactionIsValid(withdrawal));
        }
    }
}
