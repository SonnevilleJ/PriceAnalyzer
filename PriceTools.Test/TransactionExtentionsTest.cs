using System;
using System.Collections.Generic;
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

        [TestMethod]
        public void EnumerableIsEquivalentWithSameData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t4 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> {t1, t2};
            var list2 = new List<Transaction> {t3, t4};

            Assert.IsTrue(list1.IsEquivalent(list2));
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithDifferentData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares + 1, price);
            var t4 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4 };

            Assert.IsFalse(list1.IsEquivalent(list2));
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithExtraTransaction()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t4 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t5 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4, t5 };

            Assert.IsFalse(list1.IsEquivalent(list2));
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithMissingTransaction()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3 };

            Assert.IsFalse(list1.IsEquivalent(list2));
        }

        [TestMethod]
        public void EnumerableIsEquivalentOrderCheck()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares * 2, price);
            var t2 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t4 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares * 2, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4 };

            Assert.IsTrue(list1.IsEquivalent(list2));
        }

        [TestMethod]
        public void EnumerableIsEqualWithSameData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t4 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4 };

            Assert.IsTrue(list1.IsEqual(list2));
        }

        [TestMethod]
        public void EnumerableIsEqualWithDifferentData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares + 1, price);
            var t4 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4 };

            Assert.IsFalse(list1.IsEqual(list2));
        }

        [TestMethod]
        public void EnumerableIsEqualWithExtraTransaction()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t4 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t5 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4, t5 };

            Assert.IsFalse(list1.IsEqual(list2));
        }

        [TestMethod]
        public void EnumerableIsEqualWithMissingTransaction()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3 };

            Assert.IsFalse(list1.IsEqual(list2));
        }

        [TestMethod]
        public void EnumerableIsEqualOrderCheck()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares * 2, price);
            var t2 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t4 = TransactionFactory.ConstructBuy(ticker, settlementDate, shares * 2, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4 };

            Assert.IsFalse(list1.IsEqual(list2));
        }
    }
}
