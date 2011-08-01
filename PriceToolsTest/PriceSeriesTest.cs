using System.Globalization;
using System.Linq;
using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sonneville.PriceTools.Services;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for PriceSeriesTest and is intended
    ///to contain all PriceSeriesTest Unit Tests
    ///</summary>
    [TestClass]
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
            Settings.CanConnectToInternet = false;
        }

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod]
        public void CloseTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            decimal expected = p3.Close;
            decimal actual = target.Close;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HasValue1Test()
        {
            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            Assert.IsFalse(target.HasValueInRange(DateTime.Now));
        }

        [TestMethod]
        public void HasValue2Test()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            Assert.IsTrue(target.HasValueInRange(p1.Head));
        }

        [TestMethod]
        public void HasValue3Test()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            Assert.IsTrue(target.HasValueInRange(p3.Tail));
        }

        /// <summary>
        ///A test for Head
        ///</summary>
        [TestMethod]
        public void HeadTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

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
        [TestMethod]
        public void HighTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            decimal? expected = p2.High;
            decimal? actual = target.High;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAtHeadTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            decimal? expected = target.Open;
            decimal? actual = target[p1.Head];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAtTailTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            decimal? expected = p3.Close;
            decimal? actual = target[target.Tail];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IndexerValueBeforeHeadTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            var result = target[p1.Head.Subtract(new TimeSpan(1))];
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAfterTailTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            var expected = p3.Close;
            var actual = target[p3.Tail.Add(new TimeSpan(1))];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Low
        ///</summary>
        [TestMethod]
        public void LowTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            decimal? expected = p3.Low;
            decimal? actual = target.Low;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Open
        ///</summary>
        [TestMethod]
        public void OpenTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            decimal? expected = p1.Open;
            decimal? actual = target.Open;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PricePeriods
        ///</summary>
        [TestMethod]
        public void PricePeriodsTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            Assert.AreEqual(3, target.PricePeriods.Count);
            Assert.IsTrue(target.PricePeriods.Contains(p1));
            Assert.IsTrue(target.PricePeriods.Contains(p2));
            Assert.IsTrue(target.PricePeriods.Contains(p3));
        }

        [TestMethod]
        public void GetWeeklyPeriodsFromDailyPeriodsTestPeriodCount()
        {
            var head = new DateTime(2011, 1, 1);
            var tail = new DateTime(2011, 6, 30, 23, 59, 59);
            var priceSeries = new YahooPriceHistoryCsvFile(TestData.DE_1_1_2011_to_6_30_2011, head, tail).PriceSeries;
            
            var pricePeriods = priceSeries.GetPricePeriods(PriceSeriesResolution.Weeks);

            Assert.AreEqual(26, pricePeriods.Count);
        }

        [TestMethod]
        public void GetWeeklyPeriodsFromDailyPeriodsTestPeriodData()
        {
            var seriesHead = new DateTime(2011, 1, 1);
            var seriesTail = new DateTime(2011, 6, 30, 23, 59, 59);
            var priceSeries = new YahooPriceHistoryCsvFile(TestData.DE_1_1_2011_to_6_30_2011, seriesHead, seriesTail).PriceSeries;

            var dailyPeriods = priceSeries.GetPricePeriods(PriceSeriesResolution.Days);
            var weeklyPeriods = priceSeries.GetPricePeriods(PriceSeriesResolution.Weeks);

            var weekHead = seriesHead;
            var weekTail = GetNextFridayClose(seriesHead);
            if (DateTimeFormatInfo.CurrentInfo == null) Assert.Inconclusive();
            
            var calendar = DateTimeFormatInfo.CurrentInfo.Calendar;
            do
            {
                var periodsInWeek = dailyPeriods.Where(period => period.Head >= weekHead && period.Tail <= weekTail);
                var weeklyPeriod = weeklyPeriods.Where(period => period.Head >= weekHead && period.Tail <= weekTail).First();

                Assert.AreEqual(periodsInWeek.Min(p => p.Head), weeklyPeriod.Head);
                Assert.AreEqual(periodsInWeek.Max(p => p.Tail), weeklyPeriod.Tail);
                Assert.AreEqual(periodsInWeek.First().Open, weeklyPeriod.Open);
                Assert.AreEqual(periodsInWeek.Max(p => p.High), weeklyPeriod.High);
                Assert.AreEqual(periodsInWeek.Min(p => p.Low), weeklyPeriod.Low);
                Assert.AreEqual(periodsInWeek.Last().Close, weeklyPeriod.Close);

                weekHead = GetNextMondayOpen(weekTail);
                weekTail = GetNextFridayClose(weekHead);
            } while (calendar.GetWeekOfYear(weekTail, CalendarWeekRule.FirstDay, DayOfWeek.Sunday) <=
                     calendar.GetWeekOfYear(seriesTail, CalendarWeekRule.FirstDay, DayOfWeek.Sunday));
        }

        private static DateTime GetNextMondayOpen(DateTime dateTime)
        {
            do
            {
                dateTime = dateTime.AddDays(1);
            } while (dateTime.DayOfWeek != DayOfWeek.Monday);
            return dateTime.Date;
        }

        private static DateTime GetNextFridayClose(DateTime dateTime)
        {
            do
            {
                dateTime = dateTime.AddDays(1);
            } while (dateTime.DayOfWeek != DayOfWeek.Friday);
            return dateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetDailyPeriodsFromWeeklyPeriodsTest()
        {
            IPriceSeries priceSeries = new YahooPriceHistoryCsvFile(TestData.DE_Apr_June2011_Weekly_Google).PriceSeries;
            priceSeries.GetPricePeriods(PriceSeriesResolution.Days);
        }

        /// <summary>
        ///A test for Tail
        ///</summary>
        [TestMethod]
        public void TailTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

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
        [TestMethod]
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
        [TestMethod]
        public void VolumeTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            long? expected = p1.Volume + p3.Volume; // p2 has no volume
            long? actual = target.Volume;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIndexerWhenNotConnectedToInternet()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            var expected = p2.Close;
            var actual = target[p3.Head.Subtract(new TimeSpan(1))];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIndexerWhenConnectedToInternet()
        {
            Settings.CanConnectToInternet = true;

            DateTime dateTime = new DateTime(2011, 5, 2, 22, 52, 0);
            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("DE");

            const decimal expected = 97.39m;
            decimal? actual = target[dateTime];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestDownloadPriceHistory()
        {
            Settings.CanConnectToInternet = true;
            DateTime dateTime = new DateTime(2011, 4, 1);
            IPriceSeries target = PriceSeriesFactory.CreatePriceSeries("DE");
            
            target.DownloadPriceData(dateTime);

            Settings.CanConnectToInternet = false;
            Assert.IsNotNull(target[dateTime.AddHours(12)]);    // add 12 hours because no price is available at midnight.
        }
    }
}
