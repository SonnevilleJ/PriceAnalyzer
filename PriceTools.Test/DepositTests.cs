using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class DepositTests : CashTransactionTestsBase
    {
        private const OrderType TransactionType = OrderType.Deposit;

        [Test]
        public override void SerializeTest()
        {
            CashTransactionSerializeTest(TransactionType);
        }

        [Test]
        public override void AmountInvalidTest()
        {
            CashTransactionAmountInvalidTest(TransactionType);
        }

        [Test]
        public override void SettlementDateTest()
        {
            CashTransactionSettlementDateTest(TransactionType);
        }

        [Test]
        public override void AmountValidTest()
        {
            CashTransactionAmountValidTest(TransactionType);
        }
    }
}