using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class MarginableCashAccountTest
    {
        private readonly ICashAccountFactory _cashAccountFactory;
        private readonly ITransactionFactory _transactionFactory;

        public MarginableCashAccountTest()
        {
            _cashAccountFactory = new CashAccountFactory();
            _transactionFactory = new TransactionFactory();
        }

        [Test]
        public void WithdrawBeforeDepositTest()
        {
            var target = _cashAccountFactory.ConstructMarginableCashAccount();
            var dateTime = new DateTime(2010, 1, 16);
            const decimal amount = 500.00m;

            target.Withdraw(dateTime, amount);

            Assert.AreEqual(-amount, target.GetCashBalance(dateTime));
        }

        [Test]
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

        [Test]
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
