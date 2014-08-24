using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class SellShortTests : ShareTransactionTestsBase
    {
        private const OrderType TransactionType = OrderType.SellShort;

        [Test]
        public override void SerializeTest()
        {
            ShareTransactionSerializeTest(TransactionType);
        }

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [Test]
        public override void SettlementDateTest()
        {
            ShareTransactionSettlementDateTest(TransactionType);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [Test]
        public override void TickerTest()
        {
            ShareTransactionTickerTest(TransactionType);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [Test]
        public override void PriceValidTest()
        {
            ShareTransactionPriceValidTest(TransactionType);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [Test]
        public override void PriceInvalidTest()
        {
            ShareTransactionPriceInvalidTest(TransactionType);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [Test]
        public override void SharesValidTest()
        {
            ShareTransactionSharesValidTest(TransactionType);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [Test]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void SharesInvalidTest()
        {
            ShareTransactionSharesInvalidTest(TransactionType);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [Test]
        public override void CommissionValidTest()
        {
            ShareTransactionCommissionValidTest(TransactionType);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [Test]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CommissionInvalidTest()
        {
            ShareTransactionCommissionInvalidTest(TransactionType);
        }

        /// <summary>
        /// A test for TotalValue
        /// </summary>
        [Test]
        public override void TotalValueTest()
        {
            ShareTransactionTotalValueTest(TransactionType);
        }
    }
}