using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PricePeriodExtentionsTest
    {
        [TestMethod]
        public void IsEqualDifferentData()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head.AddDays(1), tail.AddDays(1), close);

            Assert.IsFalse(period1.IsEqual(period2));
        }

        [TestMethod]
        public void IsEqualSameData()
        {
            var head = new DateTime(2011, 3, 13);
            var tail = head.AddDays(1);
            const decimal close = 100.00m;

            var period1 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);
            var period2 = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            Assert.IsTrue(period1.IsEqual(period2));
        }
        
        [TestMethod]
        public void IsEqualDifferentImplementations()
        {
            var head = new DateTime(2011, 3, 13);
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
    }
}
