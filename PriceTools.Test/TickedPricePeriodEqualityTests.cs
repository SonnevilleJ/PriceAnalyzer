﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Implementation;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class TickedPricePeriodEqualityTests
    {
        private readonly IPricePeriodFactory _pricePeriodFactory;
        private readonly IPriceTickFactory _priceTickFactory;

        public TickedPricePeriodEqualityTests()
        {
            _pricePeriodFactory = new PricePeriodFactory();
            _priceTickFactory = new PriceTickFactory();
        }

        [TestMethod]
        public void PeriodEqualsWithDifferentData()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price + 1));

            Assert.IsFalse(period1.Equals(period2));
        }

        [TestMethod]
        public void PeriodEqualsWithSameData()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));

            Assert.IsTrue(period1.Equals(period2));
        }

        [TestMethod]
        public void PeriodGetHashCodeWithDifferentData()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price + 1));

            Assert.AreNotEqual(period1.GetHashCode(), period2.GetHashCode());
        }

        [TestMethod]
        public void PeriodGetHashCodeWithSameData()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));

            Assert.AreEqual(period1.GetHashCode(), period2.GetHashCode());
        }

        [TestMethod]
        public void PeriodEqualsWithDifferentImplementations()
        {
            var head = new DateTime(2012, 6, 16);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, close);
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(new List<PriceTick>
                                                                            {
                                                                                _priceTickFactory.ConstructPriceTick(head, close),
                                                                                _priceTickFactory.ConstructPriceTick(tail, close)
                                                                            });

            Assert.IsFalse(period1.Equals(period2));
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithDifferentData()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period3 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price + 1));
            var period4 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));

            var list1 = new List<ITickedPricePeriod> { period1, period2 };
            var list2 = new List<ITickedPricePeriod> { period3, period4 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithSameData()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period3 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period4 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));

            var list1 = new List<ITickedPricePeriod> { period1, period2 };
            var list2 = new List<ITickedPricePeriod> { period3, period4 };

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithExtraPeriod()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period3 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period4 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period5 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));

            var list1 = new List<ITickedPricePeriod> { period1, period2 };
            var list2 = new List<ITickedPricePeriod> { period3, period4, period5 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithMissingPeriod()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period3 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));

            var list1 = new List<ITickedPricePeriod> { period1, period2 };
            var list2 = new List<ITickedPricePeriod> { period3 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentOrderCheck()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price + 1));
            var period3 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price + 1));
            var period4 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));

            var list1 = new List<ITickedPricePeriod> { period1, period2 };
            var list2 = new List<ITickedPricePeriod> { period3, period4 };

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithDifferentData()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period3 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price + 1));
            var period4 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));

            var list1 = new List<ITickedPricePeriod> { period1, period2 };
            var list2 = new List<ITickedPricePeriod> { period3, period4 };

            Assert.IsFalse(list1.Equals(list2));
        }

        [TestMethod]
        public void EnumerableEqualsWithSameData()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period3 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period4 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));

            var list1 = new List<ITickedPricePeriod> { period1, period2 };
            var list2 = new List<ITickedPricePeriod> { period3, period4 };

            CollectionAssert.AreEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithExtraPeriod()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period3 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period4 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period5 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));

            var list1 = new List<ITickedPricePeriod> { period1, period2 };
            var list2 = new List<ITickedPricePeriod> { period3, period4, period5 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithMissingPeriod()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period3 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));

            var list1 = new List<ITickedPricePeriod> { period1, period2 };
            var list2 = new List<ITickedPricePeriod> { period3 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsOrderCheck()
        {
            var settlementDate = new DateTime(2012, 6, 16);
            const decimal price = 100.00m;

            var period1 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));
            var period2 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price + 1));
            var period3 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price + 1));
            var period4 = _pricePeriodFactory.ConstructTickedPricePeriod(_priceTickFactory.ConstructPriceTick(settlementDate, price));

            var list1 = new List<ITickedPricePeriod> { period1, period2 };
            var list2 = new List<ITickedPricePeriod> { period3, period4 };

            CollectionAssert.AreNotEqual(list1, list2);
        }
    }
}
