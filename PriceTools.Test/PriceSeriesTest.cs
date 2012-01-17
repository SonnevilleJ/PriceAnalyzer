﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.SamplePriceData;
using Sonneville.PriceTools.Yahoo;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Test
{
    /// <summary>
    ///This is a test class for PriceSeriesTest and is intended
    ///to contain all PriceSeriesTest Unit Tests
    ///</summary>
    [TestClass]
    public class PriceSeriesTest
    {
        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod]
        public void CloseTest()
        {
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p3.Close;
            var actual = target.Close;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod]
        public void OrderedCloseTest()
        {
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p3);
            target.AddPriceData(p2);
            target.AddPriceData(p1);

            var expected = p3.Close;
            var actual = target.Close;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HasValue1Test()
        {
            var target = PriceSeriesFactory.CreatePriceSeries("test");
            Assert.IsFalse(target.HasValueInRange(DateTime.Now));
        }

        [TestMethod]
        public void HasValue2Test()
        {
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            Assert.IsTrue(target.HasValueInRange(p1.Head));
        }

        [TestMethod]
        public void HasValue3Test()
        {
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            Assert.IsTrue(target.HasValueInRange(p3.Tail));
        }

        /// <summary>
        ///A test for Head
        ///</summary>
        [TestMethod]
        public void HeadTest()
        {
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p1.Head;
            var actual = target.Head;
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
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

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
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

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
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            decimal? expected = p3.Close;
            decimal? actual = target[target.Tail];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueBeforeHeadTest()
        {
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            const decimal expected = 0.00m;
            var actual = target[p1.Head.Subtract(new TimeSpan(1))];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAfterTailTest()
        {
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

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
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

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
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

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
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p3);
            target.AddPriceData(p2);
            target.AddPriceData(p1);

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
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

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
            var priceSeries = new YahooPriceHistoryCsvFile("DE", CsvPriceHistory.DE_1_1_2011_to_6_30_2011, head, tail).PriceSeries;

            var pricePeriods = priceSeries.GetPricePeriods(Resolution.Weeks);

            Assert.AreEqual(26, pricePeriods.Count);
        }

        [TestMethod]
        public void GetWeeklyPeriodsFromDailyPeriodsTestPeriodData()
        {
            var seriesHead = new DateTime(2011, 1, 1);
            var seriesTail = new DateTime(2011, 6, 30, 23, 59, 59);
            var priceSeries = new YahooPriceHistoryCsvFile("DE", CsvPriceHistory.DE_1_1_2011_to_6_30_2011, seriesHead, seriesTail).PriceSeries;

            var dailyPeriods = priceSeries.GetPricePeriods(Resolution.Days);
            var weeklyPeriods = priceSeries.GetPricePeriods(Resolution.Weeks);

            var weekHead = seriesHead.GetFollowingOpen();
            var weekTail = seriesHead.GetFollowingWeeklyClose();

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
                weekTail = weekHead.GetFollowingWeeklyClose();
            } while (calendar.GetWeekOfYear(weekTail, calendarWeekRule, firstDayOfWeek) <=
                     calendar.GetWeekOfYear(seriesTail, calendarWeekRule, firstDayOfWeek));
        }

        [TestMethod]
        public void GetMonthlyPeriodsFromDailyPeriodsTestPeriodData()
        {
            var seriesHead = new DateTime(2011, 1, 1);
            var seriesTail = new DateTime(2011, 6, 30, 23, 59, 59);
            var priceSeries = new YahooPriceHistoryCsvFile("DE", CsvPriceHistory.DE_1_1_2011_to_6_30_2011, seriesHead, seriesTail).PriceSeries;

            var dailyPeriods = priceSeries.GetPricePeriods(Resolution.Days);
            var monthlyPeriods = priceSeries.GetPricePeriods(Resolution.Months);

            var monthHead = seriesHead.GetFollowingOpen();
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
            var priceSeries = SamplePriceSeries.DE_Apr_June_2011_Weekly_Google;
            priceSeries.GetPricePeriods(Resolution.Days);
        }

        /// <summary>
        ///A test for Tail
        ///</summary>
        [TestMethod]
        public void TailTest()
        {
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p3.Tail;
            var actual = target.Tail;
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
            var target = PriceSeriesFactory.CreatePriceSeries(ticker);

            const string expected = ticker;
            var actual = target.Ticker;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Volume
        ///</summary>
        [TestMethod]
        public void VolumeTest()
        {
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p1.Volume + p3.Volume; // p2 has no volume
            var actual = target.Volume;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIndexerGetsMostRecentPriceBeforeNextPeriod()
        {
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p2.Close;
            var actual = target[p3.Head.Subtract(new TimeSpan(1))];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestDownloadPriceDataHead()
        {
            var dateTime = new DateTime(2011, 4, 1);
            var target = PriceSeriesFactory.CreatePriceSeries("DE");

            var provider = new YahooPriceDataProvider();
            target.RetrievePriceData(provider, dateTime);

            Assert.IsNotNull(target[dateTime.AddHours(12)]);    // add 12 hours because no price is available at midnight.
        }

        [TestMethod]
        public void TestDownloadPriceDataHeadTail()
        {
            var head = new DateTime(2011, 4, 1);
            var tail = head.AddMonths(1);
            var target = PriceSeriesFactory.CreatePriceSeries("DE");

            var provider = new YahooPriceDataProvider();
            target.RetrievePriceData(provider, head, tail);

            Assert.IsNotNull(target[tail]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDownloadPriceDataProvidersResolutionIsChecked()
        {
            var provider = new WeeklyProvider();
            var head = new DateTime(2011, 4, 1);
            var tail = head.AddMonths(1);
            var target = PriceSeriesFactory.CreatePriceSeries("DE");

            target.RetrievePriceData(provider, head, tail);
        }

        private static IEnumerable<ReactionMove> GetReactionHighs()
        {
            return new List<ReactionMove>
                       {
                           new ReactionMove
                               {DateTime = new DateTime(2011, 1, 13), HighLow = HighLow.High, Reaction = 89.97m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 1, 18), HighLow = HighLow.High, Reaction = 91.63m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 1, 21), HighLow = HighLow.High, Reaction = 90.64m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 2, 2), HighLow = HighLow.High, Reaction = 94.24m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 2, 7), HighLow = HighLow.High, Reaction = 94.61m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 2, 9), HighLow = HighLow.High, Reaction = 94.74m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 2, 14), HighLow = HighLow.High, Reaction = 95.9m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 2, 16), HighLow = HighLow.High, Reaction = 97.36m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 2, 25), HighLow = HighLow.High, Reaction = 92m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 3, 3), HighLow = HighLow.High, Reaction = 93.16m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 3, 14), HighLow = HighLow.High, Reaction = 88.13m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 3, 21), HighLow = HighLow.High, Reaction = 92.46m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 3, 28), HighLow = HighLow.High, Reaction = 94.98m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 4, 1), HighLow = HighLow.High, Reaction = 99.8m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 4, 15), HighLow = HighLow.High, Reaction = 94.49m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 4, 26), HighLow = HighLow.High, Reaction = 97.78m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 5, 2), HighLow = HighLow.High, Reaction = 98.3m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 5, 6), HighLow = HighLow.High, Reaction = 94.17m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 5, 10), HighLow = HighLow.High, Reaction = 94.61m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 5, 19), HighLow = HighLow.High, Reaction = 87.73m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 6, 9), HighLow = HighLow.High, Reaction = 82.79m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 6, 14), HighLow = HighLow.High, Reaction = 82.66m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 6, 22), HighLow = HighLow.High, Reaction = 83m},
                       };
        }

        private static IEnumerable<ReactionMove> GetReactionLows()
        {
            return new List<ReactionMove>
                       {
                           new ReactionMove
                               {DateTime = new DateTime(2011, 1, 4), HighLow = HighLow.Low, Reaction = 81.8m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 1, 20), HighLow = HighLow.Low, Reaction = 86.89m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 1, 25), HighLow = HighLow.Low, Reaction = 88.24m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 1, 31), HighLow = HighLow.Low, Reaction = 88.38m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 2, 3), HighLow = HighLow.Low, Reaction = 92.37m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 2, 8), HighLow = HighLow.Low, Reaction = 93.01m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 2, 15), HighLow = HighLow.Low, Reaction = 93.09m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 2, 23), HighLow = HighLow.Low, Reaction = 86.23m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 3, 1), HighLow = HighLow.Low, Reaction = 88.33m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 3, 8), HighLow = HighLow.Low, Reaction = 88.9m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 3, 11), HighLow = HighLow.Low, Reaction = 84.59m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 3, 15), HighLow = HighLow.Low, Reaction = 84.27m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 3, 23), HighLow = HighLow.Low, Reaction = 90.12m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 3, 29), HighLow = HighLow.Low, Reaction = 91.75m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 4, 14), HighLow = HighLow.Low, Reaction = 92.61m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 4, 18), HighLow = HighLow.Low, Reaction = 90.65m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 4, 27), HighLow = HighLow.Low, Reaction = 94.65m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 5, 5), HighLow = HighLow.Low, Reaction = 91m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 5, 18), HighLow = HighLow.Low, Reaction = 84.65m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 5, 23), HighLow = HighLow.Low, Reaction = 82.25m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 6, 1), HighLow = HighLow.Low, Reaction = 82.73m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 6, 8), HighLow = HighLow.Low, Reaction = 79.61m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 6, 13), HighLow = HighLow.Low, Reaction = 79.77m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 6, 16), HighLow = HighLow.Low, Reaction = 78.23m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 6, 20), HighLow = HighLow.Low, Reaction = 77.81m},
                           new ReactionMove
                               {DateTime = new DateTime(2011, 6, 23), HighLow = HighLow.Low, Reaction = 78.8m}
                       };
        }

        private static IEnumerable<ReactionMove> GetReactionMoves()
        {
            var reactionMoves = GetReactionHighs().ToList();
            reactionMoves.AddRange(GetReactionLows());
            reactionMoves.OrderBy(rm => rm.DateTime);

            return reactionMoves;
        }

        [TestMethod]
        public void ReactionMovesCountTest()
        {
            var reactionMoves = GetReactionMoves();
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var actualMoves = target.ReactionMoves;

            Assert.AreEqual(reactionMoves.Count(), actualMoves.Count());
        }

        [TestMethod]
        public void ReactionMovesTest()
        {
            var reactionMoves = GetReactionMoves();
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var actualMoves = target.ReactionMoves;

            foreach (var reactionMove in reactionMoves)
            {
                Assert.IsTrue(actualMoves.Contains(reactionMove));
            }
        }

        [TestMethod]
        public void ReactionHighsCountTest()
        {
            var newHighs = GetReactionHighs();
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var actualHighs = target.ReactionHighs;

            Assert.AreEqual(newHighs.Count(), actualHighs.Count());
        }

        [TestMethod]
        public void ReactionHighsTest()
        {
            var newHighs = GetReactionHighs();
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var actualHighs = target.ReactionHighs;

            foreach (var reactionMove in newHighs)
            {
                Assert.IsTrue(actualHighs.Contains(reactionMove));
            }
        }

        [TestMethod]
        public void ReactionLowsCountTest()
        {
            var newLows = GetReactionLows();
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var actualLows = target.ReactionLows;

            Assert.AreEqual(newLows.Count(), actualLows.Count());
        }

        [TestMethod]
        public void ReactionLowsTest()
        {
            var newLows = GetReactionLows();
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var actualLows = target.ReactionLows;

            foreach (var reactionMove in newLows)
            {
                Assert.IsTrue(actualLows.Contains(reactionMove));
            }
        }

        [TestMethod]
        public void ValuesCountTest()
        {
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            IDictionary<DateTime, decimal> expected = new Dictionary<DateTime, decimal> {{p1.Head, p1.Close}, {p2.Head, p2.Close}, {p3.Head, p3.Close}};

            var actual = target.Values;
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void ValuesMatchTest()
        {
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();

            var target = PriceSeriesFactory.CreatePriceSeries("test");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            IDictionary<DateTime, decimal> expected = new Dictionary<DateTime, decimal> {{p1.Head, p1.Close}, {p2.Head, p2.Close}, {p3.Head, p3.Close}};

            var actual = target.Values;
            foreach (var key in expected.Keys)
            {
                Assert.IsTrue(actual.ContainsKey(key));
            }
        }

        [TestMethod]
        public void DefaultResolutionTest()
        {
            var target = PriceSeriesFactory.CreatePriceSeries("DE");

            const Resolution expected = Resolution.Days;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddPricePeriodEventTest()
        {
            var target = PriceSeriesFactory.CreatePriceSeries("DE");
            var head = new DateTime(2011, 12, 28);
            var tail = head.GetFollowingClose();
            const decimal close = 5.00m;
            var period = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            var raised = false;
            EventHandler<NewPriceDataAvailableEventArgs> handler = (sender, e) => { raised = true; };

            try
            {
                target.NewPriceDataAvailable += handler;

                target.AddPriceData(period);

                Assert.IsTrue(raised);
            }
            finally
            {
                target.NewPriceDataAvailable -= handler;
            }
        }

        [TestMethod]
        public void AddPricePeriodEventArgsTest()
        {
            var target = PriceSeriesFactory.CreatePriceSeries("DE");
            var head = new DateTime(2011, 12, 28);
            var tail = head.GetFollowingClose();
            const decimal close = 5.00m;
            var period = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            NewPriceDataAvailableEventArgs args = null;
            EventHandler<NewPriceDataAvailableEventArgs> handler = (sender, e) => { args = e; };

            try
            {
                target.NewPriceDataAvailable += handler;

                target.AddPriceData(period);

                var argsHead = args.Head;
                var argsTail = args.Tail;

                Assert.AreEqual(head, argsHead);
                Assert.AreEqual(tail, argsTail);
            }
            finally
            {
                target.NewPriceDataAvailable -= handler;
            }
        }

        [TestMethod]
        public void AddPricePeriodsEventArgsTest()
        {
            var target = PriceSeriesFactory.CreatePriceSeries("DE");
            var p1 = TestUtilities.CreatePeriod1();
            var p2 = TestUtilities.CreatePeriod2();
            var p3 = TestUtilities.CreatePeriod3();
            var pricePeriods = new List<IPricePeriod> {p1, p2, p3};
            var head = p1.Head;
            var tail = p3.Tail;

            NewPriceDataAvailableEventArgs args = null;
            EventHandler<NewPriceDataAvailableEventArgs> handler = (sender, e) => { args = e; };

            try
            {
                target.NewPriceDataAvailable += handler;

                target.AddPriceData(pricePeriods);

                var argsHead = args.Head;
                var argsTail = args.Tail;

                Assert.AreEqual(head, argsHead);
                Assert.AreEqual(tail, argsTail);
            }
            finally
            {
                target.NewPriceDataAvailable -= handler;
            }
        }

        [TestMethod]
        public void AddPricePeriodAddsToPricePeriodsTest()
        {
            var target = PriceSeriesFactory.CreatePriceSeries("DE");
            var head = new DateTime(2011, 12, 28);
            var tail = head.GetFollowingClose();
            const decimal close = 5.00m;
            var period = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            target.AddPriceData(period);

            Assert.IsTrue(target.PricePeriods.Contains(period));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPricePeriodOverlapTestInner()
        {
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var head = new DateTime(2011, 2, 28);
            var tail = head.GetFollowingClose();
            const decimal close = 5.00m;
            var period = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            target.AddPriceData(period);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPricePeriodOverlapHeadTest()
        {
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var tail = target.Head.GetFollowingClose();
            var head = tail.AddDays(-1).GetMostRecentOpen();
            const decimal close = 5.00m;
            var period = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            target.AddPriceData(period);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPricePeriodOverlapTailTest()
        {
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var head = target.Tail.GetMostRecentOpen();
            var tail = head.AddDays(1).GetFollowingClose();
            const decimal close = 5.00m;
            var period = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            target.AddPriceData(period);
        }
    }
}