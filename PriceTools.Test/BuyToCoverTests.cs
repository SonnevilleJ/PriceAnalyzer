using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class BuyToCoverTests : ShareTransactionTestsBase
    {
        private const OrderType TransactionType = OrderType.BuyToCover;

        [TestMethod]
        public override void SerializeTest()
        {
            ShareTransactionSerializeTest(TransactionType);
        }

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [TestMethod]
        public override void SettlementDateTest()
        {
            ShareTransactionSettlementDateTest(TransactionType);
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod]
        public override void TickerTest()
        {
            ShareTransactionTickerTest(TransactionType);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod]
        public override void PriceValidTest()
        {
            ShareTransactionPriceValidTest(TransactionType);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod]
        public override void PriceInvalidTest()
        {
            ShareTransactionPriceInvalidTest(TransactionType);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod]
        public override void SharesValidTest()
        {
            ShareTransactionSharesValidTest(TransactionType);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void SharesInvalidTest()
        {
            ShareTransactionSharesInvalidTest(TransactionType);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod]
        public override void CommissionValidTest()
        {
            ShareTransactionCommissionValidTest(TransactionType);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public override void CommissionInvalidTest()
        {
            ShareTransactionCommissionInvalidTest(TransactionType);
        }

        /// <summary>
        /// A test for TotalValue
        /// </summary>
        [TestMethod]
        public override void TotalValueTest()
        {
            ShareTransactionTotalValueTest(TransactionType);
        }
    }
}