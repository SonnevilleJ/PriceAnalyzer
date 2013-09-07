using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Implementation;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class TransactionEqualityTests
    {
        private readonly ITransactionFactory _transactionFactory;

        public TransactionEqualityTests()
        {
            _transactionFactory = new TransactionFactory();
        }

        [TestMethod]
        public void TransactionEqualsWithSameData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;
            
            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void TransactionEqualsWithDifferentData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares * 2, price);

            Assert.IsFalse(t1.Equals(t2));
        }

        [TestMethod]
        public void TransactionGetHashCodeWithSameData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        }

        [TestMethod]
        public void TransactionGetHashCodeWithDifferentData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares * 2, price);

            Assert.AreNotEqual(t1.GetHashCode(), t2.GetHashCode());
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithSameData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t4 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> {t1, t2};
            var list2 = new List<Transaction> {t3, t4};

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithDifferentData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares + 1, price);
            var t4 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithExtraTransaction()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t4 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t5 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4, t5 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithMissingTransaction()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentOrderCheck()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares * 2, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t4 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares * 2, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4 };

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEqualWithSameData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t4 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4 };

            CollectionAssert.AreEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEqualWithDifferentData()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares + 1, price);
            var t4 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEqualWithExtraTransaction()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t4 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t5 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4, t5 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEqualWithMissingTransaction()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEqualOrderCheck()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares * 2, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t4 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares * 2, price);

            var list1 = new List<Transaction> { t1, t2 };
            var list2 = new List<Transaction> { t3, t4 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void GetHashCodeSame()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeDifferent()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares + 1, price);

            Assert.AreNotEqual(t1.GetHashCode(), t2.GetHashCode());
        }
    }
}
