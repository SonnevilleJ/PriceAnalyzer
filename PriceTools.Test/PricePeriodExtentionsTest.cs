﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PricePeriodExtentionsTest
    {
        [TestMethod]
        public void PeriodEqualsWithDifferentData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head.AddDays(1), tail.AddDays(1), close);

            Assert.IsFalse(period1.Equals(period2));
        }

        [TestMethod]
        public void PeriodEqualsWithSameData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            Assert.IsTrue(period1.Equals(period2));
        }
        
        [TestMethod]
        public void PeriodEqualsWithDifferentImplementations()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.ConstructTickedPricePeriod(new List<PriceTick>
                                                                            {
                                                                                PriceTickFactory.ConstructPriceTick(head, close),
                                                                                PriceTickFactory.ConstructPriceTick(tail, close)
                                                                            });

            Assert.IsFalse(period1.Equals(period2));
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithDifferentData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period3 = PricePeriodFactory.CreateStaticPricePeriod(head, tail.AddDays(1), close);
            var period4 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            var list1 = new List<PricePeriod> { period1, period2 };
            var list2 = new List<PricePeriod> { period3, period4 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithSameData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period3 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period4 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            var list1 = new List<PricePeriod> {period1, period2};
            var list2 = new List<PricePeriod> {period3, period4};

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithExtraPeriod()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period3 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period4 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period5 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            var list1 = new List<PricePeriod> {period1, period2};
            var list2 = new List<PricePeriod> {period3, period4, period5};

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithMissingPeriod()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period3 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            var list1 = new List<PricePeriod> { period1, period2 };
            var list2 = new List<PricePeriod> {period3};

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentOrderCheck()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail.AddDays(1), close);
            var period3 = PricePeriodFactory.CreateStaticPricePeriod(head, tail.AddDays(1), close);
            var period4 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            var list1 = new List<PricePeriod> { period1, period2 };
            var list2 = new List<PricePeriod> { period3, period4 };

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithDifferentData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period3 = PricePeriodFactory.CreateStaticPricePeriod(head, tail.AddDays(1), close);
            var period4 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            var list1 = new List<PricePeriod> { period1, period2 };
            var list2 = new List<PricePeriod> { period3, period4 };

            Assert.IsFalse(list1.Equals(list2));
        }

        [TestMethod]
        public void EnumerableEqualsWithSameData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period3 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period4 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            var list1 = new List<PricePeriod> { period1, period2 };
            var list2 = new List<PricePeriod> { period3, period4 };

            CollectionAssert.AreEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithExtraPeriod()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period3 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period4 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period5 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            var list1 = new List<PricePeriod> { period1, period2 };
            var list2 = new List<PricePeriod> { period3, period4, period5 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithMissingPeriod()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period3 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            var list1 = new List<PricePeriod> { period1, period2 };
            var list2 = new List<PricePeriod> { period3 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsOrderCheck()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail.AddDays(1), close);
            var period3 = PricePeriodFactory.CreateStaticPricePeriod(head, tail.AddDays(1), close);
            var period4 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            var list1 = new List<PricePeriod> { period1, period2 };
            var list2 = new List<PricePeriod> { period3, period4 };

            CollectionAssert.AreNotEqual(list1, list2);
        }
    }
}