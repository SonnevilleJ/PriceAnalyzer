using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools.Extensions
{
    [TestClass]
    public class TransactionExtensionsTest
    {
        [TestMethod]
        public void Buy()
        {
            InnerTest(OrderType.Buy, true);
        }
        
        [TestMethod]
        public void Sell()
        {
            InnerTest(OrderType.Sell, false);
        }
        
        [TestMethod]
        public void SellShort()
        {
            InnerTest(OrderType.SellShort, true);
        }
        
        [TestMethod]
        public void BuyToCover()
        {
            InnerTest(OrderType.BuyToCover, false);
        }
        
        [TestMethod]
        public void DividendReinvestment()
        {
            InnerTest(OrderType.DividendReinvestment, true);
        }
        
        [TestMethod]
        public void DividendReceipt()
        {
            InnerTest(OrderType.DividendReceipt, false);
        }
        
        [TestMethod]
        public void Deposit()
        {
            InnerTest(OrderType.Deposit, false);
        }
        
        [TestMethod]
        public void Withdrawal()
        {
            InnerTest(OrderType.Withdrawal, false);
        }
        
        private static void InnerTest(OrderType orderType, bool expected)
        {
            var transaction = new TransactionFactory().ConstructTransaction(orderType, DateTime.Today, "DE", 5, 5, 0);

            Assert.AreEqual(expected, transaction.IsOpeningTransaction());
        }
    }
}