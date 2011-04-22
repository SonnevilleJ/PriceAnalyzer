using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for PriceSeriesTest and is intended
    ///to contain all PriceSeriesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PriceSeriesTest
    {
        [ClassCleanup]
        public static void ClassCleanup()
        {
            Settings.SetDefaultSettings();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Settings.SetDefaultSettings();
        }

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod()]
        public void CloseTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            decimal expected = p3.Close;
            decimal actual = target.Close;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HasValue1Test()
        {
            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            Assert.IsFalse(target.HasValue(DateTime.Now));
        }

        [TestMethod]
        public void HasValue2Test()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            Assert.IsTrue(target.HasValue(p1.Head));
        }

        [TestMethod]
        public void HasValue3Test()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            Assert.IsTrue(target.HasValue(p3.Tail));
        }

        /// <summary>
        ///A test for Head
        ///</summary>
        [TestMethod()]
        public void HeadTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            DateTime expected = p1.Head;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HeadEmptyTest()
        {
            var test = PriceSeriesFactory.CreatePriceSeries("test").Head;
        }

        /// <summary>
        ///A test for High
        ///</summary>
        [TestMethod()]
        public void HighTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            decimal? expected = p2.High;
            decimal? actual = target.High;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void IndexerValueAtHeadTest()
        {
            Settings.CanConnectToInternet = false;

            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            decimal? expected = target.Open;
            decimal? actual = target[p1.Head];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void IndexerValueAtTailTest()
        {
            Settings.CanConnectToInternet = false;

            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            decimal? expected = p3.Close;
            decimal? actual = target[target.Tail];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void IndexerValueBeforeHeadTest()
        {
            Settings.CanConnectToInternet = false;

            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);
            
            Assert.IsNull(target[p1.Head.Subtract(new TimeSpan(1))]);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void IndexerValueAfterTailTest()
        {
            Settings.CanConnectToInternet = false;

            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            Assert.IsNull(target[p3.Tail.Add(new TimeSpan(1))]);
        }

        /// <summary>
        ///A test for Low
        ///</summary>
        [TestMethod()]
        public void LowTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            decimal? expected = p3.Low;
            decimal? actual = target.Low;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Open
        ///</summary>
        [TestMethod()]
        public void OpenTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            decimal? expected = p1.Open;
            decimal? actual = target.Open;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PricePeriods
        ///</summary>
        [TestMethod()]
        public void PricePeriodsTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            Assert.AreEqual(3, target.PricePeriods.Count);
            Assert.IsTrue(target.PricePeriods.Contains(p1));
            Assert.IsTrue(target.PricePeriods.Contains(p2));
            Assert.IsTrue(target.PricePeriods.Contains(p3));
        }

        /// <summary>
        ///A test for Tail
        ///</summary>
        [TestMethod()]
        public void TailTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            DateTime expected = p3.Tail;
            DateTime actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TailEmptyTest()
        {
            var test = PriceSeriesFactory.CreatePriceSeries("test").Tail;
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod()]
        public void TickerTest()
        {
            const string ticker = "test";
            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries(ticker);

            const string expected = ticker;
            string actual = target.Ticker;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Volume
        ///</summary>
        [TestMethod()]
        public void VolumeTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            long? expected = p1.Volume + p3.Volume; // p2 has no volume
            long? actual = target.Volume;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializePriceSeriesTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.PricePeriods.Add(p1);
            target.PricePeriods.Add(p2);
            target.PricePeriods.Add(p3);

            PriceSeries actual = ((PriceSeries)TestUtilities.Serialize(target));
            Assert.AreEqual(target, actual);
        }
    }
}
