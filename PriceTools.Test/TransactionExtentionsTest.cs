using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class TransactionExtentionsTest
    {
        [TestMethod]
        public void TransactionIsEqualWithSameData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;
            
            var t1 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            Assert.IsTrue(t1.IsEqual(t2));
        }

        [TestMethod]
        public void TransactionIsEqualWithDifferentData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares * 2, price);

            Assert.IsFalse(t1.IsEqual(t2));
        }
    }
}
