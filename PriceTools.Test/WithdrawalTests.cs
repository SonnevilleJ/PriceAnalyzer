using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class WithdrawalTests : CashTransactionTestsBase
    {
        private const OrderType TransactionType = OrderType.Withdrawal;

        [TestMethod]
        public override void SerializeTest()
        {
            CashTransactionSerializeTest(TransactionType);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public override void AmountInvalidTest()
        {
            CashTransactionAmountInvalidTest(TransactionType);
        }

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [TestMethod]
        public override void SettlementDateTest()
        {
            CashTransactionSettlementDateTest(TransactionType);
        }

        /// <summary>
        ///A test for Amount
        ///</summary>
        [TestMethod]
        public override void AmountValidTest()
        {
            CashTransactionAmountValidTest(TransactionType);
        }
    }
}