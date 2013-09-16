using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools.Extensions
{
    [TestClass]
    public class TransactionExtensionsTest
    {
        [TestMethod]
        public void BuyOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.Buy, true);
        }
        
        [TestMethod]
        public void SellOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.Sell, false);
        }
        
        [TestMethod]
        public void SellShortOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.SellShort, true);
        }
        
        [TestMethod]
        public void BuyToCoverOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.BuyToCover, false);
        }
        
        [TestMethod]
        public void DividendReinvestmentOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.DividendReinvestment, true);
        }
        
        [TestMethod]
        public void DividendReceiptOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.DividendReceipt, false);
        }
        
        [TestMethod]
        public void DepositOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.Deposit, false);
        }
        
        [TestMethod]
        public void WithdrawalOpeningTransaction()
        {
            OpeningTransactionTest(OrderType.Withdrawal, false);
        }
        
        [TestMethod]
        public void BuyClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.Buy, false);
        }
        
        [TestMethod]
        public void SellClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.Sell, true);
        }
        
        [TestMethod]
        public void SellShortClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.SellShort, false);
        }
        
        [TestMethod]
        public void BuyToCoverClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.BuyToCover, true);
        }
        
        [TestMethod]
        public void DividendReinvestmentClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.DividendReinvestment, false);
        }
        
        [TestMethod]
        public void DividendReceiptClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.DividendReceipt, false);
        }
        
        [TestMethod]
        public void DepositClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.Deposit, false);
        }
        
        [TestMethod]
        public void WithdrawalClosingTransactionTest()
        {
            ClosingTransactionTest(OrderType.Withdrawal, false);
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