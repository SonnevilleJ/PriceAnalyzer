using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
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
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(new TimeSpan(2, 4, 30, 0), target.TimeSpan);
        }

        [TestMethod]
        public void OpenTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(q1.Price, target.Open);
        }

        [TestMethod]
        public void HighTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(q3.Price, target.High);
        }

        [TestMethod]
        public void LowTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(q2.Price, target.Low);
        }

        [TestMethod]
        public void CloseTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(q3.Price, target.Close);
        }

        [TestMethod]
        public void LastTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(target.Close, target.Last);
        }

        [TestMethod]
        public void VolumeTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(q1.Volume + q2.Volume + q3.Volume, target.Volume);
        }

        [TestMethod]
        public void TimeSpanTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();
            IPriceQuote q4 = CreateQuote4();

            QuotedPricePeriod target = new QuotedPricePeriod();

            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(new TimeSpan(2, 4, 30, 0), target.TimeSpan);

            target.AddPriceQuote(q4);
            Assert.AreEqual(new TimeSpan(2, 6, 30, 0), target.TimeSpan);
        }

        [TestMethod]
        public void OpenTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();
            IPriceQuote q4 = CreateQuote4();

            QuotedPricePeriod target = new QuotedPricePeriod();

            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(q1.Price, target.Open);

            target.AddPriceQuote(q4);
            Assert.AreEqual(q1.Price, target.Open);
        }

        [TestMethod]
        public void HighTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();
            IPriceQuote q4 = CreateQuote4();

            QuotedPricePeriod target = new QuotedPricePeriod();

            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(q3.Price, target.High);

            target.AddPriceQuote(q4);
            Assert.AreEqual(q3.Price, target.High);
        }

        [TestMethod]
        public void LowTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();
            IPriceQuote q4 = CreateQuote4();

            QuotedPricePeriod target = new QuotedPricePeriod();

            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(q2.Price, target.Low);

            target.AddPriceQuote(q4);
            Assert.AreEqual(q2.Price, target.Low);
        }

        [TestMethod]
        public void CloseTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();
            IPriceQuote q4 = CreateQuote4();

            QuotedPricePeriod target = new QuotedPricePeriod();

            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(q3.Price, target.Close);

            target.AddPriceQuote(q4);
            Assert.AreEqual(q4.Price, target.Close);
        }

        [TestMethod]
        public void VolumeTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();
            IPriceQuote q4 = CreateQuote4();

            QuotedPricePeriod target = new QuotedPricePeriod();

            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(q1.Volume + q2.Volume + q3.Volume, target.Volume);

            target.AddPriceQuote(q4);
            Assert.AreEqual(q1.Volume + q2.Volume + q3.Volume + q4.Volume, target.Volume);
        }

        [TestMethod]
        public void OpenTestAfterAddingPriorPriceQuote()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            
            target.AddPriceQuote(q2, q3);
            Assert.AreEqual(q2.Price, target.Open);

            target.AddPriceQuote(q1);
            Assert.AreEqual(q1.Price, target.Open);
        }

        [TestMethod]
        public void SerializeQuotedPricePeriodTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            target.AddPriceQuote(q1, q2, q3);

            QuotedPricePeriod actual = ((QuotedPricePeriod)TestUtilities.Serialize(target));
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void EntityQuotedPricePeriodTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();


            QuotedPricePeriod target = new QuotedPricePeriod();
            target.AddPriceQuote(q1, q2, q3);

            TestUtilities.VerifyPricePeriodEntity(target);
        }

        [TestMethod]
        public void QuotedPricePeriodIndexerTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            target.AddPriceQuote(q1, q2, q3);

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
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(3, target.PriceQuotes.Count);
        }

        [TestMethod]
        public void NewPriceDataAvailableEventTest()
        {
            IPriceQuote q1 = CreateQuote1();
            IPriceQuote q2 = CreateQuote2();
            IPriceQuote q3 = CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            int count = 0;
            target.NewPriceDataAvailable += delegate { count++; };
            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(1, count);

            target = new QuotedPricePeriod();
            count = 0;
            target.NewPriceDataAvailable += delegate { count++; };
            target.AddPriceQuote(q1);
            target.AddPriceQuote(q2);
            target.AddPriceQuote(q3);

            Assert.AreEqual(3, count);
        }

        private static PriceQuote CreateQuote1()
        {
            return new PriceQuote
                       {
                           SettlementDate = DateTime.Parse("2/28/2011 9:30 AM"),
                           Price = 10,
                           Volume = 50
                       };
        }

        private static PriceQuote CreateQuote2()
        {
            return new PriceQuote
                       {
                           SettlementDate = DateTime.Parse("3/1/2011 10:00 AM"),
                           Price = 9,
                           Volume = 60
                       };
        }

        private static PriceQuote CreateQuote3()
        {
            return new PriceQuote
                       {
                           SettlementDate = DateTime.Parse("3/2/2011 2:00 PM"),
                           Price = 14,
                           Volume = 50
                       };
        }

        private static PriceQuote CreateQuote4()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("3/2/2011 4:00 PM"),
                Price = 11,
                Volume = 30
            };
        }
    }
}
