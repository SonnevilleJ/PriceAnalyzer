using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class PricePeriodEqualityTests
    {
        [Test]
        public void PeriodEqualsWithDifferentData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head.AddDays(1), tail.AddDays(1), close);

            Assert.IsFalse(period1.Equals(period2));
        }

        [Test]
        public void PeriodEqualsWithSameData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head, tail, close);

            Assert.IsTrue(period1.Equals(period2));
        }

        [Test]
        public void PeriodGetHashCodeWithDifferentData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head.AddDays(1), tail.AddDays(1), close);

            Assert.AreNotEqual(period1.GetHashCode(), period2.GetHashCode());
        }

        [Test]
        public void PeriodGetHashCodeWithSameData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head, tail, close);

            Assert.AreEqual(period1.GetHashCode(), period2.GetHashCode());
        }

        [Test]
        public void EnumerableIsEquivalentWithDifferentData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head, tail, close);
            var period3 = new PricePeriod(head, tail.AddDays(1), close);
            var period4 = new PricePeriod(head, tail, close);

            var list1 = new List<IPricePeriod> { period1, period2 };
            var list2 = new List<IPricePeriod> { period3, period4 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [Test]
        public void EnumerableIsEquivalentWithSameData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head, tail, close);
            var period3 = new PricePeriod(head, tail, close);
            var period4 = new PricePeriod(head, tail, close);

            var list1 = new List<IPricePeriod> {period1, period2};
            var list2 = new List<IPricePeriod> {period3, period4};

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [Test]
        public void EnumerableIsEquivalentWithExtraPeriod()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head, tail, close);
            var period3 = new PricePeriod(head, tail, close);
            var period4 = new PricePeriod(head, tail, close);
            var period5 = new PricePeriod(head, tail, close);

            var list1 = new List<IPricePeriod> {period1, period2};
            var list2 = new List<IPricePeriod> {period3, period4, period5};

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [Test]
        public void EnumerableIsEquivalentWithMissingPeriod()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head, tail, close);
            var period3 = new PricePeriod(head, tail, close);

            var list1 = new List<IPricePeriod> { period1, period2 };
            var list2 = new List<IPricePeriod> {period3};

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [Test]
        public void EnumerableIsEquivalentOrderCheck()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head, tail.AddDays(1), close);
            var period3 = new PricePeriod(head, tail.AddDays(1), close);
            var period4 = new PricePeriod(head, tail, close);

            var list1 = new List<IPricePeriod> { period1, period2 };
            var list2 = new List<IPricePeriod> { period3, period4 };

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [Test]
        public void EnumerableEqualsWithDifferentData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head, tail, close);
            var period3 = new PricePeriod(head, tail.AddDays(1), close);
            var period4 = new PricePeriod(head, tail, close);

            var list1 = new List<IPricePeriod> { period1, period2 };
            var list2 = new List<IPricePeriod> { period3, period4 };

            Assert.IsFalse(list1.Equals(list2));
        }

        [Test]
        public void EnumerableEqualsWithSameData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head, tail, close);
            var period3 = new PricePeriod(head, tail, close);
            var period4 = new PricePeriod(head, tail, close);

            var list1 = new List<IPricePeriod> { period1, period2 };
            var list2 = new List<IPricePeriod> { period3, period4 };

            CollectionAssert.AreEqual(list1, list2);
        }

        [Test]
        public void EnumerableEqualsWithExtraPeriod()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head, tail, close);
            var period3 = new PricePeriod(head, tail, close);
            var period4 = new PricePeriod(head, tail, close);
            var period5 = new PricePeriod(head, tail, close);

            var list1 = new List<IPricePeriod> { period1, period2 };
            var list2 = new List<IPricePeriod> { period3, period4, period5 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [Test]
        public void EnumerableEqualsWithMissingPeriod()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head, tail, close);
            var period3 = new PricePeriod(head, tail, close);

            var list1 = new List<IPricePeriod> { period1, period2 };
            var list2 = new List<IPricePeriod> { period3 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [Test]
        public void EnumerableEqualsOrderCheck()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = new PricePeriod(head, tail, close);
            var period2 = new PricePeriod(head, tail.AddDays(1), close);
            var period3 = new PricePeriod(head, tail.AddDays(1), close);
            var period4 = new PricePeriod(head, tail, close);

            var list1 = new List<IPricePeriod> { period1, period2 };
            var list2 = new List<IPricePeriod> { period3, period4 };

            CollectionAssert.AreNotEqual(list1, list2);
        }
    }
}
