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
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            var target = new QuotedPricePeriod();
            target.AddPriceQuotes(q1, q2, q3);

            Assert.AreEqual(new TimeSpan(2, 4, 30, 0), target.TimeSpan());
        }

        [TestMethod]
        public void OpenTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            var target = new QuotedPricePeriod();
            target.AddPriceQuotes(q1, q2, q3);

            Assert.AreEqual(q1.Price, target.Open);
        }

        [TestMethod]
        public void HighTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            var target = new QuotedPricePeriod();
            target.AddPriceQuotes(q1, q2, q3);

            Assert.AreEqual(q3.Price, target.High);
        }

        [TestMethod]
        public void LowTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            var target = new QuotedPricePeriod();
            target.AddPriceQuotes(q1, q2, q3);

            Assert.AreEqual(q2.Price, target.Low);
        }

        [TestMethod]
        public void CloseTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            var target = new QuotedPricePeriod();
            target.AddPriceQuotes(q1, q2, q3);

            Assert.AreEqual(q3.Price, target.Close);
        }

        [TestMethod]
        public void VolumeTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            var target = new QuotedPricePeriod();
            target.AddPriceQuotes(q1, q2, q3);

            Assert.AreEqual(q1.Volume + q2.Volume + q3.Volume, target.Volume);
        }

        [TestMethod]
        public void TimeSpanTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();
            IPriceQuote q4 = TestUtilities.CreateQuote4();

            var target = new QuotedPricePeriod();

            target.AddPriceQuotes(q1, q2, q3);
            Assert.AreEqual(new TimeSpan(2, 4, 30, 0), target.TimeSpan());

            target.AddPriceQuotes(q4);
            Assert.AreEqual(new TimeSpan(2, 6, 30, 0), target.TimeSpan());
        }

        [TestMethod]
        public void OpenTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();
            IPriceQuote q4 = TestUtilities.CreateQuote4();

            var target = new QuotedPricePeriod();

            target.AddPriceQuotes(q1, q2, q3);
            Assert.AreEqual(q1.Price, target.Open);

            target.AddPriceQuotes(q4);
            Assert.AreEqual(q1.Price, target.Open);
        }

        [TestMethod]
        public void HighTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();
            IPriceQuote q4 = TestUtilities.CreateQuote4();

            var target = new QuotedPricePeriod();

            target.AddPriceQuotes(q1, q2, q3);
            Assert.AreEqual(q3.Price, target.High);

            target.AddPriceQuotes(q4);
            Assert.AreEqual(q3.Price, target.High);
        }

        [TestMethod]
        public void LowTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();
            IPriceQuote q4 = TestUtilities.CreateQuote4();

            var target = new QuotedPricePeriod();

            target.AddPriceQuotes(q1, q2, q3);
            Assert.AreEqual(q2.Price, target.Low);

            target.AddPriceQuotes(q4);
            Assert.AreEqual(q2.Price, target.Low);
        }

        [TestMethod]
        public void CloseTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();
            IPriceQuote q4 = TestUtilities.CreateQuote4();

            var target = new QuotedPricePeriod();

            target.AddPriceQuotes(q1, q2, q3);
            Assert.AreEqual(q3.Price, target.Close);

            target.AddPriceQuotes(q4);
            Assert.AreEqual(q4.Price, target.Close);
        }

        [TestMethod]
        public void VolumeTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();
            IPriceQuote q4 = TestUtilities.CreateQuote4();

            var target = new QuotedPricePeriod();

            target.AddPriceQuotes(q1, q2, q3);
            Assert.AreEqual(q1.Volume + q2.Volume + q3.Volume, target.Volume);

            target.AddPriceQuotes(q4);
            Assert.AreEqual(q1.Volume + q2.Volume + q3.Volume + q4.Volume, target.Volume);
        }

        [TestMethod]
        public void OpenTestAfterAddingPriorPriceQuote()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            var target = new QuotedPricePeriod();
            
            target.AddPriceQuotes(q2, q3);
            Assert.AreEqual(q2.Price, target.Open);

            target.AddPriceQuotes(q1);
            Assert.AreEqual(q1.Price, target.Open);
        }

        [TestMethod]
        public void QuotedPricePeriodIndexerTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            var target = new QuotedPricePeriod();
            target.AddPriceQuotes(q1, q2, q3);

            Assert.AreEqual(q2.Price, target[q2.SettlementDate]);
            Assert.AreEqual(q1.Price, target[q2.SettlementDate - new TimeSpan(1)]);
            Assert.AreEqual(q2.Price, target[q2.SettlementDate + new TimeSpan(1)]);
        }

        /// <summary>
        ///A test for PriceQuotes
        ///</summary>
        [TestMethod]
        public void PriceQuotesTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            var target = new QuotedPricePeriod();
            target.AddPriceQuotes(q1, q2, q3);

            Assert.AreEqual(3, target.PriceQuotes.Count);
        }

        [TestMethod]
        public void NewPriceDataAvailableEventTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            var target = new QuotedPricePeriod();
            var count = 0;
            target.NewPriceDataAvailable += delegate { count++; };
            target.AddPriceQuotes(q1, q2, q3);
            Assert.AreEqual(1, count);

            target = new QuotedPricePeriod();
            count = 0;
            target.NewPriceDataAvailable += delegate { count++; };
            target.AddPriceQuotes(q1);
            target.AddPriceQuotes(q2);
            target.AddPriceQuotes(q3);

            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void ValuesCountTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            var target = new QuotedPricePeriod();
            target.AddPriceQuotes(q1, q2, q3);

            IDictionary<DateTime, decimal> expected = new Dictionary<DateTime, decimal> { { q1.SettlementDate, q1.Price }, { q2.SettlementDate, q2.Price }, { q3.SettlementDate, q3.Price } };

            var actual = target.Values;
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void ValuesMatchTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            var target = new QuotedPricePeriod();
            target.AddPriceQuotes(q1, q2, q3);

            IDictionary<DateTime, decimal> expected = new Dictionary<DateTime, decimal> {{q1.SettlementDate, q1.Price}, {q2.SettlementDate, q2.Price}, {q3.SettlementDate, q3.Price}};

            var actual = target.Values;
            foreach (var key in expected.Keys)
            {
                Assert.IsTrue(actual.ContainsKey(key));
            }
        }
    }
}
