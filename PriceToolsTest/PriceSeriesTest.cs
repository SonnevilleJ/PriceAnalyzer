using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sonneville.PriceTools.Extensions;
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

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod]
        public void OrderedCloseTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p3);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p1);

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
        ///A test for Open
        ///</summary>
        [TestMethod]
        public void OrderedOpenTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p3);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p1);

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

            var pricePeriods = priceSeries.GetPricePeriods(Resolution.Weeks);

            Assert.AreEqual(26, pricePeriods.Count);
        }

        [TestMethod]
        public void GetWeeklyPeriodsFromDailyPeriodsTestPeriodData()
        {
            var seriesHead = new DateTime(2011, 1, 1);
            var seriesTail = new DateTime(2011, 6, 30, 23, 59, 59);
            var priceSeries = new YahooPriceHistoryCsvFile(TestData.DE_1_1_2011_to_6_30_2011, seriesHead, seriesTail).PriceSeries;

            var dailyPeriods = priceSeries.GetPricePeriods(Resolution.Days);
            var weeklyPeriods = priceSeries.GetPricePeriods(Resolution.Weeks);

            var weekHead = seriesHead.GetCurrentOrFollowingTradingDay();
            var weekTail = seriesHead.GetFollowingWeekClose();

            var dtfi = DateTimeFormatInfo.CurrentInfo;
            if (dtfi == null) Assert.Inconclusive();
            var calendar = dtfi.Calendar;
            var calendarWeekRule = dtfi.CalendarWeekRule;
            var firstDayOfWeek = dtfi.FirstDayOfWeek;

            do
            {
                var periodsInWeek = dailyPeriods.Where(period => period.Head >= weekHead && period.Tail <= weekTail);
                var weeklyPeriod = weeklyPeriods.Where(period => period.Head >= weekHead && period.Tail <= weekTail).First();

                var head = periodsInWeek.Min(p => p.Head);
                Assert.IsTrue(DatesShareWeek(head, weeklyPeriod.Head));
                var tail = periodsInWeek.Max(p => p.Tail);
                Assert.IsTrue(DatesShareWeek(tail, weeklyPeriod.Tail));
                Assert.AreEqual(periodsInWeek.First().Open, weeklyPeriod.Open);
                Assert.AreEqual(periodsInWeek.Max(p => p.High), weeklyPeriod.High);
                Assert.AreEqual(periodsInWeek.Min(p => p.Low), weeklyPeriod.Low);
                Assert.AreEqual(periodsInWeek.Last().Close, weeklyPeriod.Close);

                weekHead = weekTail.GetFollowingWeeklyOpen();
                weekTail = weekHead.GetFollowingWeekClose();
            } while (calendar.GetWeekOfYear(weekTail, calendarWeekRule, firstDayOfWeek) <=
                     calendar.GetWeekOfYear(seriesTail, calendarWeekRule, firstDayOfWeek));
        }

        [TestMethod]
        public void GetMonthlyPeriodsFromDailyPeriodsTestPeriodData()
        {
            var seriesHead = new DateTime(2011, 1, 1);
            var seriesTail = new DateTime(2011, 6, 30, 23, 59, 59);
            var priceSeries = new YahooPriceHistoryCsvFile(TestData.DE_1_1_2011_to_6_30_2011, seriesHead, seriesTail).PriceSeries;

            var dailyPeriods = priceSeries.GetPricePeriods(Resolution.Days);
            var monthlyPeriods = priceSeries.GetPricePeriods(Resolution.Months);

            var monthHead = seriesHead.GetCurrentOrFollowingTradingDay();
            var monthTail = seriesHead.GetFollowingMonthlyClose();

            var dtfi = DateTimeFormatInfo.CurrentInfo;
            if (dtfi == null) Assert.Inconclusive();
            var calendar = dtfi.Calendar;

            do
            {
                var periodsInMonth = dailyPeriods.Where(period => period.Head >= monthHead && period.Tail <= monthTail);
                var monthlyPeriod = monthlyPeriods.Where(period => period.Head >= monthHead && period.Tail <= monthTail).First();

                var head = periodsInMonth.Min(p => p.Head);
                Assert.IsTrue(DatesShareMonth(head, monthlyPeriod.Head));
                var tail = periodsInMonth.Max(p => p.Tail);
                Assert.IsTrue(DatesShareMonth(tail, monthlyPeriod.Tail));
                Assert.AreEqual(periodsInMonth.First().Open, monthlyPeriod.Open);
                Assert.AreEqual(periodsInMonth.Max(p => p.High), monthlyPeriod.High);
                Assert.AreEqual(periodsInMonth.Min(p => p.Low), monthlyPeriod.Low);
                Assert.AreEqual(periodsInMonth.Last().Close, monthlyPeriod.Close);

                monthHead = monthTail.GetFollowingMonthlyOpen();
                monthTail = monthHead.GetFollowingMonthlyClose();
            } while (calendar.GetMonth(monthTail) <= calendar.GetMonth(seriesTail));
        }

        private static bool DatesShareWeek(DateTime date1, DateTime date2)
        {
            // this method is only used to test if two dates are in the same week of price data.
            // Implementing support for market holidays should remove the need for this method.

            var periodStart = date1.GetMostRecentWeeklyOpen();
            var periodEnd = date1.GetFollowingWeeklyClose();
            return date2 >= periodStart && date2 <= periodEnd;
        }

        private static bool DatesShareMonth(DateTime date1, DateTime date2)
        {
            var periodStart = date1.GetMostRecentMonthlyOpen();
            var periodEnd = date1.GetFollowingMonthlyClose();
            return date2 >= periodStart && date2 <= periodEnd;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetDailyPeriodsFromWeeklyPeriodsTest()
        {
            IPriceSeries priceSeries = new YahooPriceHistoryCsvFile(TestData.DE_Apr_June2011_Weekly_Google).PriceSeries;
            priceSeries.GetPricePeriods(Resolution.Days);
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

        private static List<KeyValuePair<DateTime, decimal>> GetNewHighs()
        {
            return new List<KeyValuePair<DateTime, decimal>>
                       {
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 1, 13), 89.97m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 1, 18), 91.63m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 1, 21), 90.64m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 2, 2), 94.24m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 2, 7), 94.61m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 2, 9), 94.74m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 2, 14), 95.9m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 2, 16), 97.36m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 2, 25), 92m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 3, 3), 93.16m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 3, 14), 88.13m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 3, 21), 92.46m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 3, 28), 94.98m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 4, 1), 99.8m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 4, 15), 94.49m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 4, 26), 97.78m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 5, 2), 98.3m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 5, 6), 94.17m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 5, 10), 94.61m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 5, 19), 87.73m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 6, 9), 82.79m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 6, 14), 82.66m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 6, 22), 83m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 6, 29), 82.99m)
                       };
        }

        private static List<KeyValuePair<DateTime, decimal>> GetNewLows()
        {
            return new List<KeyValuePair<DateTime, decimal>>
                       {
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 1, 4), 81.8m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 1, 20), 86.89m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 1, 25), 88.24m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 1, 31), 88.38m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 2, 3), 92.37m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 2, 8), 93.01m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 2, 15), 93.09m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 2, 23), 86.23m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 3, 1), 88.33m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 3, 8), 88.9m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 3, 11), 84.59m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 3, 15), 84.27m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 3, 23), 90.12m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 3, 29), 91.75m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 4, 14), 92.61m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 4, 18), 90.65m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 4, 27), 94.65m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 5, 5), 91m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 5, 18), 84.65m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 5, 23), 82.25m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 6, 1), 82.73m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 6, 8), 79.61m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 6, 13), 79.77m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 6, 16), 78.23m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 6, 20), 77.81m),
                           new KeyValuePair<DateTime, decimal>(new DateTime(2011, 6, 23), 78.8m)
                       };
        }

        [TestMethod]
        public void ReactionMHighsTest()
        {
            var newHighs = GetNewHighs();
            var target = new GooglePriceHistoryCsvFile(new ResourceStream(TestData.DE_1_1_2011_to_6_30_2011)).PriceSeries;

            IEnumerable<KeyValuePair<DateTime, decimal>> actualHighs = target.ReactionHighs;

            Assert.AreEqual(newHighs.Count, actualHighs.Count());
            foreach (var dateTime in newHighs)
            {
                Assert.IsTrue(actualHighs.Contains(dateTime));
            }
        }

        [TestMethod]
        public void ReactionLowsTest()
        {
            var newLows = GetNewLows();
            var target = new GooglePriceHistoryCsvFile(new ResourceStream(TestData.DE_1_1_2011_to_6_30_2011)).PriceSeries;

            IEnumerable<KeyValuePair<DateTime, decimal>> actualLows = target.ReactionLows;

            Assert.AreEqual(newLows.Count, actualLows.Count());
            foreach (var dateTime in newLows)
            {
                Assert.IsTrue(actualLows.Contains(dateTime));
            }
        }
    }
}