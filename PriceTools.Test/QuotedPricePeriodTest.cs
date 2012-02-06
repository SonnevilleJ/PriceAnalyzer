using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Test
{
    /// <summary>
    /// Summary description for QuotedPricePeriodTest
    /// </summary>
    [TestClass]
    public class QuotedPricePeriodTest
    {
        [TestMethod]
        public void TimeSpanTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            target.AddPriceTicks(q1, q2, q3);

            Assert.AreEqual(new TimeSpan(2, 4, 30, 0), target.TimeSpan());
        }

        [TestMethod]
        public void OpenTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            target.AddPriceTicks(q1, q2, q3);

            Assert.AreEqual(q1.Price, target.Open);
        }

        [TestMethod]
        public void HighTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            target.AddPriceTicks(q1, q2, q3);

            Assert.AreEqual(q3.Price, target.High);
        }

        [TestMethod]
        public void LowTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            target.AddPriceTicks(q1, q2, q3);

            Assert.AreEqual(q2.Price, target.Low);
        }

        [TestMethod]
        public void CloseTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            target.AddPriceTicks(q1, q2, q3);

            Assert.AreEqual(q3.Price, target.Close);
        }

        [TestMethod]
        public void VolumeTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            target.AddPriceTicks(q1, q2, q3);

            Assert.AreEqual(q1.Volume + q2.Volume + q3.Volume, target.Volume);
        }

        [TestMethod]
        public void TimeSpanTestAfterAddingSubsequentPriceTickTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();
            var q4 = TestUtilities.CreateQuote4();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();

            target.AddPriceTicks(q1, q2, q3);
            Assert.AreEqual(new TimeSpan(2, 4, 30, 0), target.TimeSpan());

            target.AddPriceTicks(q4);
            Assert.AreEqual(new TimeSpan(2, 6, 30, 0), target.TimeSpan());
        }

        [TestMethod]
        public void OpenTestAfterAddingSubsequentPriceTickTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();
            var q4 = TestUtilities.CreateQuote4();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();

            target.AddPriceTicks(q1, q2, q3);
            Assert.AreEqual(q1.Price, target.Open);

            target.AddPriceTicks(q4);
            Assert.AreEqual(q1.Price, target.Open);
        }

        [TestMethod]
        public void HighTestAfterAddingSubsequentPriceTickTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();
            var q4 = TestUtilities.CreateQuote4();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();

            target.AddPriceTicks(q1, q2, q3);
            Assert.AreEqual(q3.Price, target.High);

            target.AddPriceTicks(q4);
            Assert.AreEqual(q3.Price, target.High);
        }

        [TestMethod]
        public void LowTestAfterAddingSubsequentPriceTickTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();
            var q4 = TestUtilities.CreateQuote4();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();

            target.AddPriceTicks(q1, q2, q3);
            Assert.AreEqual(q2.Price, target.Low);

            target.AddPriceTicks(q4);
            Assert.AreEqual(q2.Price, target.Low);
        }

        [TestMethod]
        public void CloseTestAfterAddingSubsequentPriceTickTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();
            var q4 = TestUtilities.CreateQuote4();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();

            target.AddPriceTicks(q1, q2, q3);
            Assert.AreEqual(q3.Price, target.Close);

            target.AddPriceTicks(q4);
            Assert.AreEqual(q4.Price, target.Close);
        }

        [TestMethod]
        public void VolumeTestAfterAddingSubsequentPriceTickTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();
            var q4 = TestUtilities.CreateQuote4();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();

            target.AddPriceTicks(q1, q2, q3);
            Assert.AreEqual(q1.Volume + q2.Volume + q3.Volume, target.Volume);

            target.AddPriceTicks(q4);
            Assert.AreEqual(q1.Volume + q2.Volume + q3.Volume + q4.Volume, target.Volume);
        }

        [TestMethod]
        public void OpenTestAfterAddingPriorPriceTick()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            
            target.AddPriceTicks(q2, q3);
            Assert.AreEqual(q2.Price, target.Open);

            target.AddPriceTicks(q1);
            Assert.AreEqual(q1.Price, target.Open);
        }

        [TestMethod]
        public void QuotedPricePeriodIndexerTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            target.AddPriceTicks(q1, q2, q3);

            Assert.AreEqual(q2.Price, target[q2.SettlementDate]);
            Assert.AreEqual(q1.Price, target[q2.SettlementDate - new TimeSpan(1)]);
            Assert.AreEqual(q2.Price, target[q2.SettlementDate + new TimeSpan(1)]);
        }

        /// <summary>
        ///A test for PriceTicks
        ///</summary>
        [TestMethod]
        public void PriceTicksTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            target.AddPriceTicks(q1, q2, q3);

            Assert.AreEqual(3, target.PriceTicks.Count);
        }

        /// <summary>
        ///A test for PriceTicks
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PriceTicksChangedTest()
        {
            var q1 = TestUtilities.CreateTick1();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            target.PriceTicks.Add(q1);

            Assert.AreEqual(0, target.PriceTicks.Count);
        }

        [TestMethod]
        public void NewPriceDataAvailableEventTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            var count = 0;
            target.NewPriceDataAvailable += delegate { count++; };
            target.AddPriceTicks(q1, q2, q3);
            Assert.AreEqual(1, count);

            target = PricePeriodFactory.ConstructTickedPricePeriod();
            count = 0;
            target.NewPriceDataAvailable += delegate { count++; };
            target.AddPriceTicks(q1);
            target.AddPriceTicks(q2);
            target.AddPriceTicks(q3);

            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void ValuesCountTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            target.AddPriceTicks(q1, q2, q3);

            IDictionary<DateTime, decimal> expected = new Dictionary<DateTime, decimal> { { q1.SettlementDate, q1.Price }, { q2.SettlementDate, q2.Price }, { q3.SettlementDate, q3.Price } };

            var actual = target.Values;
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void ValuesMatchTest()
        {
            var q1 = TestUtilities.CreateTick1();
            var q2 = TestUtilities.CreateTick2();
            var q3 = TestUtilities.CreateTick3();

            var target = PricePeriodFactory.ConstructTickedPricePeriod();
            target.AddPriceTicks(q1, q2, q3);

            IDictionary<DateTime, decimal> expected = new Dictionary<DateTime, decimal> {{q1.SettlementDate, q1.Price}, {q2.SettlementDate, q2.Price}, {q3.SettlementDate, q3.Price}};

            var actual = target.Values;
            foreach (var key in expected.Keys)
            {
                Assert.IsTrue(actual.ContainsKey(key));
            }
        }
    }
}
