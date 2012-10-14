using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.Test.PriceData;
using Sonneville.PriceTools.Yahoo;
using Sonneville.Utilities;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    /// <summary>
    /// This test class contains unit tests for the TimeSeriesExtensions class.
    /// </summary>
    [TestClass]
    public class TimeSeriesExtensionsTest
    {
        [TestMethod]
        public void ResizePricePeriodsWeeklyResolutionCount()
        {
            var head = new DateTime(2011, 1, 1);
            var tail = new DateTime(2011, 6, 30).CurrentPeriodClose(Resolution.Days);
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(TickerManager.GetUniqueTicker());
            priceSeries.AddPriceData(new YahooPriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.DE_1_1_2011_to_6_30_2011), head, tail).PricePeriods);

            var pricePeriods = priceSeries.ResizePricePeriods(Resolution.Weeks);

            Assert.AreEqual(26, pricePeriods.Count());
        }

        [TestMethod]
        public void ResizePricePeriodsWeeklyResolutionData()
        {
            var seriesHead = new DateTime(2011, 1, 1);
            var seriesTail = new DateTime(2011, 6, 30, 23, 59, 59);
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(TickerManager.GetUniqueTicker());
            priceSeries.AddPriceData(new YahooPriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.DE_1_1_2011_to_6_30_2011), seriesHead, seriesTail).PricePeriods);

            var dailyPeriods = priceSeries.ResizePricePeriods(Resolution.Days).ToArray();
            var weeklyPeriods = priceSeries.ResizePricePeriods(Resolution.Weeks).ToArray();

            // don't use first period because data source may not have a complete period for the new resolution
            var weekHead = seriesHead.CurrentPeriodOpen(Resolution.Weeks).SeekPeriods(1, Resolution.Weeks);
            var weekTail = seriesHead.CurrentPeriodClose(Resolution.Weeks).SeekPeriods(1, Resolution.Weeks);

            var dtfi = DateTimeFormatInfo.CurrentInfo;
            if (dtfi == null) Assert.Inconclusive();
            var calendar = dtfi.Calendar;
            var calendarWeekRule = dtfi.CalendarWeekRule;
            var firstDayOfWeek = dtfi.FirstDayOfWeek;

            do
            {
                var periodsInWeek = dailyPeriods.Where(period => period.Head >= weekHead && period.Tail <= weekTail).ToArray();
                var weeklyPeriod = weeklyPeriods.First(period => period.Head >= weekHead && period.Tail <= weekTail);

                var head = periodsInWeek.Min(p => p.Head);
                Assert.IsTrue(DatesShareWeek(head, weeklyPeriod.Head));
                var tail = periodsInWeek.Max(p => p.Tail);
                Assert.IsTrue(DatesShareWeek(tail, weeklyPeriod.Tail));
                Assert.AreEqual(periodsInWeek.First().Open, weeklyPeriod.Open);
                Assert.AreEqual(periodsInWeek.Max(p => p.High), weeklyPeriod.High);
                Assert.AreEqual(periodsInWeek.Min(p => p.Low), weeklyPeriod.Low);
                Assert.AreEqual(periodsInWeek.Last().Close, weeklyPeriod.Close);

                weekHead = weekHead.NextPeriodOpen(Resolution.Weeks);
                weekTail = weekTail.NextPeriodClose(Resolution.Weeks);
            } while (calendar.GetWeekOfYear(weekTail, calendarWeekRule, firstDayOfWeek) <=
                     calendar.GetWeekOfYear(seriesTail, calendarWeekRule, firstDayOfWeek));
        }

        [TestMethod]
        public void ResizePricePeriodsMonthlyResolutionData()
        {
            var seriesHead = new DateTime(2011, 1, 1);
            var seriesTail = new DateTime(2011, 6, 30, 23, 59, 59);
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(TickerManager.GetUniqueTicker());
            priceSeries.AddPriceData(new YahooPriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.DE_1_1_2011_to_6_30_2011), seriesHead, seriesTail).PricePeriods);

            var dailyPeriods = priceSeries.ResizePricePeriods(Resolution.Days).ToArray();
            var monthlyPeriods = priceSeries.ResizePricePeriods(Resolution.Months).ToArray();

            // don't use first period because data source may not have a complete period for the new resolution
            var monthHead = seriesHead.CurrentPeriodOpen(Resolution.Months).SeekPeriods(1, Resolution.Months);
            var monthTail = seriesHead.CurrentPeriodClose(Resolution.Months).SeekPeriods(1, Resolution.Months);

            var dtfi = DateTimeFormatInfo.CurrentInfo;
            if (dtfi == null) Assert.Inconclusive();
            var calendar = dtfi.Calendar;

            do
            {
                var periodsInMonth = dailyPeriods.Where(period => period.Head >= monthHead && period.Tail <= monthTail).ToArray();
                var monthlyPeriod = monthlyPeriods.First(period => period.Head >= monthHead && period.Tail <= monthTail);

                var head = periodsInMonth.Min(p => p.Head);
                Assert.IsTrue(DatesShareMonth(head, monthlyPeriod.Head));
                var tail = periodsInMonth.Max(p => p.Tail);
                Assert.IsTrue(DatesShareMonth(tail, monthlyPeriod.Tail));
                Assert.AreEqual(periodsInMonth.First().Open, monthlyPeriod.Open);
                Assert.AreEqual(periodsInMonth.Max(p => p.High), monthlyPeriod.High);
                Assert.AreEqual(periodsInMonth.Min(p => p.Low), monthlyPeriod.Low);
                Assert.AreEqual(periodsInMonth.Last().Close, monthlyPeriod.Close);

                monthHead = monthHead.NextPeriodOpen(Resolution.Months);
                monthTail = monthTail.NextPeriodClose(Resolution.Months);
            } while (calendar.GetMonth(monthTail) <= calendar.GetMonth(seriesTail));
        }

        private static bool DatesShareWeek(DateTime date1, DateTime date2)
        {
            // this method is only used to test if two dates are in the same week of price data.
            // Implementing support for market holidays should remove the need for this method.

            var periodStart = date1.PreviousPeriodOpen(Resolution.Weeks);
            var periodEnd = date1.NextPeriodClose(Resolution.Weeks);
            return date2 >= periodStart && date2 <= periodEnd;
        }

        private static bool DatesShareMonth(DateTime date1, DateTime date2)
        {
            var periodStart = date1.PreviousPeriodOpen(Resolution.Months);
            var periodEnd = date1.NextPeriodClose(Resolution.Months);
            return date2 >= periodStart && date2 <= periodEnd;
        }

        [TestMethod]
        public void ResizePricePeriodsEqualsResizeTimePeriods()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var resolution = target.Resolution;

            CollectionAssert.AreEquivalent(target.ResizePricePeriods(resolution).Cast<ITimePeriod>().ToList(), target.ResizeTimePeriods(resolution).ToList());
        }

        [TestMethod]
        public void ResizePricePeriodsEqualsResizeTimePeriodsAllParameters()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var resolution = target.Resolution;
            var head = target.Head;
            var tail = target.Tail;

            CollectionAssert.AreEquivalent(target.ResizePricePeriods(resolution, head, tail).Cast<ITimePeriod>().ToList(), target.ResizeTimePeriods(resolution, head, tail).ToList());
        }

        [TestMethod]
        public void GetPreviousPeriodsFromHeadReturnsEmpty()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            var previousPeriods = target.GetPreviousTimePeriods(1, target.Head);
            Assert.AreEqual(0, previousPeriods.Count());
        }

        [TestMethod]
        public void GetPreviousPeriodsFromTailReturnsAllMinusOne()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            var pricePeriods = target.PricePeriods.ToArray();
            var expected = pricePeriods.Take(pricePeriods.Count() - 1).ToArray();
            var actual = target.GetPreviousTimePeriods(pricePeriods.Count(), target.Tail).ToArray();
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void GetPreviousPeriodsFromAfterTailReturnsAllPeriods()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            var allPeriods = target.PricePeriods.ToArray();
            var previousPeriods = target.GetPreviousTimePeriods(allPeriods.Count(), target.Tail.AddTicks(1)).ToArray();
            CollectionAssert.AreEquivalent(allPeriods, previousPeriods);
        }

        [TestMethod]
        public void GetPreviousPeriodsReturnsCorrectCount()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            const int requestedCount = 5;
            var previousPeriods = target.GetPreviousTimePeriods(requestedCount, target.Tail.AddTicks(1));
            Assert.AreEqual(requestedCount, previousPeriods.Count());
        }

        [TestMethod]
        public void GetPreviousPeriodsReturnsCorrectCountWhenMaxExceedsPeriods()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            var totalPeriodCount = target.PricePeriods.Count();
            var requestedCount = totalPeriodCount + 1;
            var previousPeriods = target.GetPreviousTimePeriods(requestedCount, target.Tail.AddTicks(1));
            Assert.AreEqual(totalPeriodCount, previousPeriods.Count());
        }

        [TestMethod]
        public void GetPreviousPeriodsReturnsCorrectPeriods()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            const int requestedCount = 3;
            var previousPeriods = target.GetPreviousTimePeriods(requestedCount, target.Tail.AddTicks(1)).ToArray();
            for (var i = 0; i < requestedCount; i++)
            {
                var count = target.PricePeriods.Count();
                var targetIndex = count - 1 - i;
                var period = target.PricePeriods.ToArray()[targetIndex];
                var periodsIndex = previousPeriods.Count() - 1 - i;
                Assert.AreEqual(period, previousPeriods[periodsIndex]);
            }
        }

        [TestMethod]
        public void GetPreviousPeriodFromHead()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var periodCount = target.PricePeriods.Count();
            var origin = target.PricePeriods.ToArray()[periodCount - 1].Head;

            var expected = target.PricePeriods.ToArray()[periodCount - 2];
            var actual = target.GetPreviousTimePeriod(origin);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPreviousPeriodFromTail()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var periodCount = target.PricePeriods.Count();
            var origin = target.PricePeriods.ToArray()[periodCount - 1].Tail;

            var expected = target.PricePeriods.ToArray()[periodCount - 2];
            var actual = target.GetPreviousTimePeriod(origin);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPreviousPeriodFromSaturday()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var origin = new DateTime(2011, 6, 25);
            var pricePeriods = target.GetPreviousPricePeriods(2, origin).ToArray();

            var expected = pricePeriods[pricePeriods.Count() - 1];
            var actual = target.GetPreviousTimePeriod(origin);
            Assert.AreEqual(expected, actual);
        }
    }
}
