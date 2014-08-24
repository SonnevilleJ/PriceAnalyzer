using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class DividendReinvestmentTests : ShareTransactionTestsBase
    {
        private const OrderType TransactionType = OrderType.DividendReinvestment;

        [Test]
        public override void SerializeTest()
        {
            ShareTransactionSerializeTest(TransactionType);
        }

        [Test]
        public override void SettlementDateTest()
        {
            ShareTransactionSettlementDateTest(TransactionType);
        }

        [Test]
        public override void TickerTest()
        {
            ShareTransactionTickerTest(TransactionType);
        }

        [Test]
        public override void PriceValidTest()
        {
            ShareTransactionPriceValidTest(TransactionType);
        }

        [Test]
        public override void PriceInvalidTest()
        {
            ShareTransactionPriceInvalidTest(TransactionType);
        }

        [Test]
        public override void SharesValidTest()
        {
            ShareTransactionSharesValidTest(TransactionType);
        }

        [Test]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void SharesInvalidTest()
        {
            ShareTransactionSharesInvalidTest(TransactionType);
        }

        [Test]
        public override void CommissionValidTest()
        {
            ShareTransactionCommissionValidTest(TransactionType);
        }

        [Test]
        public override void CommissionInvalidTest()
        {
            // For DividendReinvestment types, the only valid commission is zero.
        }

        [Test]
        public override void TotalValueTest()
        {
            ShareTransactionTotalValueTest(TransactionType);
        }
    }
}