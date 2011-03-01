using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    /// Summary description for PriceSeriesTest
    /// </summary>
    [TestClass]
    public class PriceSeriesTest
    {
        [TestMethod]
        public void TimeSpanTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            IPriceSeries target = new PriceSeries();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(new TimeSpan(2, 4, 30, 0), target.TimeSpan);
        }

        [TestMethod]
        public void OpenTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            IPriceSeries target = new PriceSeries();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(q1.Price, target.Open);
        }

        [TestMethod]
        public void HighTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            IPriceSeries target = new PriceSeries();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(q3.Price, target.High);
        }

        [TestMethod]
        public void LowTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            IPriceSeries target = new PriceSeries();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(q2.Price, target.Low);
        }

        [TestMethod]
        public void CloseTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            IPriceSeries target = new PriceSeries();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(q3.Price, target.Close);
        }

        [TestMethod]
        public void VolumeTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            IPriceSeries target = new PriceSeries();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(q1.Volume + q2.Volume + q3.Volume, target.Volume);
        }

        [TestMethod]
        public void TimeSpanTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();
            IPriceQuote q4 = GetQuote4();

            IPriceSeries target = new PriceSeries();

            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(new TimeSpan(2, 4, 30, 0), target.TimeSpan);

            target.AddPriceQuote(q4);
            Assert.AreEqual(new TimeSpan(2, 6, 30, 0), target.TimeSpan);
        }

        [TestMethod]
        public void OpenTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();
            IPriceQuote q4 = GetQuote4();

            IPriceSeries target = new PriceSeries();

            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(q1.Price, target.Open);

            target.AddPriceQuote(q4);
            Assert.AreEqual(q1.Price, target.Open);
        }

        [TestMethod]
        public void HighTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();
            IPriceQuote q4 = GetQuote4();

            IPriceSeries target = new PriceSeries();

            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(q3.Price, target.High);

            target.AddPriceQuote(q4);
            Assert.AreEqual(q3.Price, target.High);
        }

        [TestMethod]
        public void LowTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();
            IPriceQuote q4 = GetQuote4();

            IPriceSeries target = new PriceSeries();

            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(q2.Price, target.Low);

            target.AddPriceQuote(q4);
            Assert.AreEqual(q2.Price, target.Low);
        }

        [TestMethod]
        public void CloseTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();
            IPriceQuote q4 = GetQuote4();

            IPriceSeries target = new PriceSeries();

            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(q3.Price, target.Close);

            target.AddPriceQuote(q4);
            Assert.AreEqual(q4.Price, target.Close);
        }

        [TestMethod]
        public void VolumeTestAfterAddingSubsequentPriceQuoteTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();
            IPriceQuote q4 = GetQuote4();

            IPriceSeries target = new PriceSeries();

            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(q1.Volume + q2.Volume + q3.Volume, target.Volume);

            target.AddPriceQuote(q4);
            Assert.AreEqual(q1.Volume + q2.Volume + q3.Volume + q4.Volume, target.Volume);
        }

        [TestMethod]
        public void OpenTestAfterAddingPriorPriceQuote()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            IPriceSeries target = new PriceSeries();
            
            target.AddPriceQuote(q2, q3);
            Assert.AreEqual(q2.Price, target.Open);

            target.AddPriceQuote(q1);
            Assert.AreEqual(q1.Price, target.Open);
        }

        [TestMethod]
        public void OpenOverrideTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            const decimal open = 7.0m;
            PriceSeries target = new PriceSeries {Open = open};
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(open, target.Open);
        }

        [TestMethod]
        public void HighOverrideTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            const decimal high = 7.0m;
            PriceSeries target = new PriceSeries { High = high };
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(high, target.High);
        }

        [TestMethod]
        public void LowOverrideTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            const decimal low = 7.0m;
            PriceSeries target = new PriceSeries { Low = low };
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(low, target.Low);
        }

        [TestMethod]
        public void CloseOverrideTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            const decimal close = 7.0m;
            PriceSeries target = new PriceSeries { Close = close };
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(close, target.Close);
        }

        [TestMethod]
        public void HeadOverrideTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            DateTime head = new DateTime(2011, 2, 28);
            PriceSeries target = new PriceSeries { Head = head };
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(head, target.Head);
        }

        [TestMethod]
        public void TailOverrideTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            DateTime tail = new DateTime(2011, 2, 28);
            PriceSeries target = new PriceSeries { Tail = tail };
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(tail, target.Tail);
        }

        [TestMethod]
        public void VolumeOverrideTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            const long volume = 7000;
            PriceSeries target = new PriceSeries { Volume = volume };
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(volume, target.Volume);
        }

        [TestMethod]
        public void SerializePriceSeriesTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            IPriceSeries target = new PriceSeries();
            target.AddPriceQuote(q1, q2, q3);

            IPriceSeries actual = ((IPriceSeries)TestUtilities.Serialize(target));
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void EntityPriceSeriesTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();


            IPriceSeries target = new PriceSeries();
            target.AddPriceQuote(q1, q2, q3);

            TestUtilities.VerifyPriceSeriesEntity(target);
        }

        [TestMethod]
        public void PriceSeriesIndexerTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();


            IPriceSeries target = new PriceSeries();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(q2, target[q2.SettlementDate]);
            Assert.AreEqual(q1, target[q2.SettlementDate - new TimeSpan(1)]);
            Assert.AreEqual(q2, target[q2.SettlementDate + new TimeSpan(1)]);
        }

        /// <summary>
        ///A test for PriceQuotes
        ///</summary>
        [TestMethod]
        public void PriceQuotesTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            IPriceSeries target = new PriceSeries();
            target.AddPriceQuote(q1, q2, q3);

            Assert.AreEqual(3, target.PriceQuotes.Count);
        }

        [TestMethod]
        public void NewPriceDataAvailableEventTest()
        {
            IPriceQuote q1 = GetQuote1();
            IPriceQuote q2 = GetQuote2();
            IPriceQuote q3 = GetQuote3();

            IPriceSeries target = new PriceSeries();
            int count = 0;
            target.NewPriceDataAvailable += delegate { count++; };
            target.AddPriceQuote(q1, q2, q3);
            Assert.AreEqual(1, count);

            target = new PriceSeries();
            count = 0;
            target.NewPriceDataAvailable += delegate { count++; };
            target.AddPriceQuote(q1);
            target.AddPriceQuote(q2);
            target.AddPriceQuote(q3);

            Assert.AreEqual(3, count);
        }

        private static PriceQuote GetQuote1()
        {
            return new PriceQuote
                       {
                           SettlementDate = DateTime.Parse("2/28/2011 9:30 AM"),
                           Price = 10,
                           Volume = 50
                       };
        }

        private static PriceQuote GetQuote2()
        {
            return new PriceQuote
                       {
                           SettlementDate = DateTime.Parse("3/1/2011 10:00 AM"),
                           Price = 9,
                           Volume = 60
                       };
        }

        private static PriceQuote GetQuote3()
        {
            return new PriceQuote
                       {
                           SettlementDate = DateTime.Parse("3/2/2011 2:00 PM"),
                           Price = 14,
                           Volume = 50
                       };
        }

        private static PriceQuote GetQuote4()
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
