using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.Test.Extensions
{
    [TestFixture]
    public class TransactionExtensionsTest
    {
        [Test]
        public void BuyOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.Buy, true);
        }
        
        [Test]
        public void SellOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.Sell, false);
        }
        
        [Test]
        public void SellShortOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.SellShort, true);
        }
        
        [Test]
        public void BuyToCoverOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.BuyToCover, false);
        }
        
        [Test]
        public void DividendReinvestmentOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.DividendReinvestment, true);
        }
        
        [Test]
        public void DividendReceiptOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.DividendReceipt, true);
        }
        
        [Test]
        public void DepositOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.Deposit, true);
        }
        
        [Test]
        public void WithdrawalOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.Withdrawal, false);
        }
        
        [Test]
        public void BuyClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.Buy, false);
        }
        
        [Test]
        public void SellClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.Sell, true);
        }
        
        [Test]
        public void SellShortClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.SellShort, false);
        }
        
        [Test]
        public void BuyToCoverClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.BuyToCover, true);
        }
        
        [Test]
        public void DividendReinvestmentClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.DividendReinvestment, false);
        }
        
        [Test]
        public void DividendReceiptClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.DividendReceipt, false);
        }
        
        [Test]
        public void DepositClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.Deposit, false);
        }
        
        [Test]
        public void WithdrawalClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.Withdrawal, true);
        }
        
        private static void OpeningTransactionTest(OrderType orderType, bool expected)
        {
            var transaction = new TransactionFactory().ConstructTransaction(orderType, DateTime.Today, "DE", 5, 5, 0);

            Assert.AreEqual(expected, transaction.IsOpeningTransaction());
        }
        
        private static void ClosingTransactionTest(OrderType orderType, bool expected)
        {
            var transaction = new TransactionFactory().ConstructTransaction(orderType, DateTime.Today, "DE", 5, 5, 0);

            Assert.AreEqual(expected, transaction.IsClosingTransaction());
        }
    }
}