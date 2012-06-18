using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PricePeriodExtentionsTest
    {
        [TestMethod]
        public void PeriodIsEqualWithDifferentData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head.AddDays(1), tail.AddDays(1), close);

            Assert.IsFalse(period1.IsEqual(period2));
        }

        [TestMethod]
        public void PeriodIsEqualWithSameData()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            Assert.IsTrue(period1.IsEqual(period2));
        }
        
        [TestMethod]
        public void PeriodIsEqualWithDifferentImplementations()
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

            Assert.IsFalse(period1.IsEqual(period2));
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

            Assert.IsFalse(list1.IsEquivalent(list2));
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

            Assert.IsTrue(list1.IsEquivalent(list2));
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

            Assert.IsFalse(list1.IsEquivalent(list2));
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

            Assert.IsFalse(list1.IsEquivalent(list2));
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

            Assert.IsTrue(list1.IsEquivalent(list2));
        }

        [TestMethod]
        public void EnumerableIsEqualWithDifferentData()
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

            Assert.IsFalse(list1.IsEqual(list2));
        }

        [TestMethod]
        public void EnumerableIsEqualWithSameData()
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

            Assert.IsTrue(list1.IsEqual(list2));
        }

        [TestMethod]
        public void EnumerableIsEqualWithExtraPeriod()
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

            Assert.IsFalse(list1.IsEqual(list2));
        }

        [TestMethod]
        public void EnumerableIsEqualWithMissingPeriod()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period3 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            var list1 = new List<PricePeriod> { period1, period2 };
            var list2 = new List<PricePeriod> { period3 };

            Assert.IsFalse(list1.IsEqual(list2));
        }

        [TestMethod]
        public void EnumerableIsEqualOrderCheck()
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

            Assert.IsFalse(list1.IsEqual(list2));
        }
    }
}
