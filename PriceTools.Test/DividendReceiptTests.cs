using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class DividendReceiptTests : CashTransactionTestsBase
    {
        private const OrderType TransactionType = OrderType.DividendReceipt;

        [Test]
        public override void SerializeTest()
        {
            CashTransactionSerializeTest(TransactionType);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [Test]
        public override void AmountInvalidTest()
        {
            CashTransactionAmountInvalidTest(TransactionType);
        }

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [Test]
        public override void SettlementDateTest()
        {
            CashTransactionSettlementDateTest(TransactionType);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [Test]
        public override void AmountValidTest()
        {
            CashTransactionAmountValidTest(TransactionType);
        }
    }
}