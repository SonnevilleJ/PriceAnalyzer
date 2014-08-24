using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class TransactionEqualityTests
    {
        private readonly ITransactionFactory _transactionFactory;

        public TransactionEqualityTests()
        {
            _transactionFactory = new TransactionFactory();
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

            var list1 = new List<ITransaction> {t1, t2};
            var list2 = new List<ITransaction> {t3, t4};

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [Test]
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

            var list1 = new List<ITransaction> { t1, t2 };
            var list2 = new List<ITransaction> { t3, t4 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [Test]
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

            var list1 = new List<ITransaction> { t1, t2 };
            var list2 = new List<ITransaction> { t3, t4, t5 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [Test]
        public void EnumerableIsEquivalentWithMissingTransaction()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<ITransaction> { t1, t2 };
            var list2 = new List<ITransaction> { t3 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [Test]
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

            var list1 = new List<ITransaction> { t1, t2 };
            var list2 = new List<ITransaction> { t3, t4 };

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [Test]
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

            var list1 = new List<ITransaction> { t1, t2 };
            var list2 = new List<ITransaction> { t3, t4 };

            CollectionAssert.AreEqual(list1, list2);
        }

        [Test]
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

            var list1 = new List<ITransaction> { t1, t2 };
            var list2 = new List<ITransaction> { t3, t4 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [Test]
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

            var list1 = new List<ITransaction> { t1, t2 };
            var list2 = new List<ITransaction> { t3, t4, t5 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [Test]
        public void EnumerableIsEqualWithMissingTransaction()
        {
            const string ticker = "DE";
            var settlementDate = new DateTime(2012, 6, 19);
            const decimal shares = 5.0m;
            const decimal price = 100.00m;

            var t1 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t2 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);
            var t3 = _transactionFactory.ConstructBuy(ticker, settlementDate, shares, price);

            var list1 = new List<ITransaction> { t1, t2 };
            var list2 = new List<ITransaction> { t3 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [Test]
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

            var list1 = new List<ITransaction> { t1, t2 };
            var list2 = new List<ITransaction> { t3, t4 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [Test]
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

        [Test]
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
