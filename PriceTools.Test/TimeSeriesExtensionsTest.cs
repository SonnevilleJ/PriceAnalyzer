using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.SampleData;

namespace Sonneville.PriceTools.Test
{
    /// <summary>
    /// This test class contains unit tests for the TimeSeriesUtility class.
    /// </summary>
    [TestClass]
    public class TimeSeriesExtensionsTest
    {
        private IPriceSeriesFactory _priceSeriesFactory;
        private ITimeSeriesUtility _timeSeriesUtility;

        [TestInitialize]
        public void Initialize()
        {
            _priceSeriesFactory = new PriceSeriesFactory();
            _timeSeriesUtility = new TimeSeriesUtility();
        }

        [TestMethod]
        public void ResizePricePeriodsWeeklyResolutionCount()
        {
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries("DE");
            priceSeries.AddPriceData(SamplePriceDatas.Deere.PriceHistory.PricePeriods);

            var pricePeriods = _timeSeriesUtility.ResizePricePeriods(priceSeries, Resolution.Weeks);

            Assert.AreEqual(26, pricePeriods.Count());
        }

        [TestMethod]
        public void ResizePricePeriodsWeeklyResolutionData()
        {
            var seriesHead = new DateTime(2011, 1, 1);
            var seriesTail = new DateTime(2011, 6, 30, 23, 59, 59);
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries("DE");
            priceSeries.AddPriceData(SamplePriceDatas.Deere.PriceHistory.PricePeriods);

            var dailyPeriods = _timeSeriesUtility.ResizePricePeriods(priceSeries, Resolution.Days).ToArray();
            var weeklyPeriods = _timeSeriesUtility.ResizePricePeriods(priceSeries, Resolution.Weeks).ToArray();

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
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries("DE");
            priceSeries.AddPriceData(SamplePriceDatas.Deere.PriceHistory.PricePeriods);

            var dailyPeriods = _timeSeriesUtility.ResizePricePeriods(priceSeries, Resolution.Days).ToArray();
            var monthlyPeriods = _timeSeriesUtility.ResizePricePeriods(priceSeries, Resolution.Months).ToArray();

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
            var target = SamplePriceDatas.Deere.PriceSeries;
            var resolution = target.Resolution;

            CollectionAssert.AreEquivalent(_timeSeriesUtility.ResizePricePeriods(target, resolution).Cast<ITimePeriod<decimal>>().ToList(), _timeSeriesUtility.ResizeTimePeriods(target, resolution).ToList());
        }

        [TestMethod]
        public void ResizePricePeriodsEqualsResizeTimePeriodsAllParameters()
        {
            var target = SamplePriceDatas.Deere.PriceSeries;
            var resolution = target.Resolution;
            var head = target.Head;
            var tail = target.Tail;

            CollectionAssert.AreEquivalent(_timeSeriesUtility.ResizePricePeriods(target, resolution, head, tail).Cast<ITimePeriod<decimal>>().ToList(), _timeSeriesUtility.ResizeTimePeriods(target, resolution, head, tail).ToList());
        }

        [TestMethod]
        public void GetPreviousPeriodsFromHeadReturnsEmpty()
        {
            var target = SamplePriceDatas.Deere.PriceSeries;

            var previousPeriods = _timeSeriesUtility.GetPreviousTimePeriods(target, 1, target.Head);
            Assert.AreEqual(0, previousPeriods.Count());
        }

        [TestMethod]
        public void GetPreviousPeriodsFromTailReturnsAllMinusOne()
        {
            var target = SamplePriceDatas.Deere.PriceSeries;

            var pricePeriods = target.PricePeriods.ToArray();
            var expected = pricePeriods.Take(pricePeriods.Count() - 1).ToArray();
            var actual = _timeSeriesUtility.GetPreviousTimePeriods(target, pricePeriods.Count(), target.Tail).ToArray();
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void GetPreviousPeriodsFromAfterTailReturnsAllPeriods()
        {
            var target = SamplePriceDatas.Deere.PriceSeries;

            var allPeriods = target.PricePeriods.ToArray();
            var previousPeriods = _timeSeriesUtility.GetPreviousTimePeriods(target, allPeriods.Count(), target.Tail.AddTicks(1)).ToArray();
            CollectionAssert.AreEquivalent(allPeriods, previousPeriods);
        }

        [TestMethod]
        public void GetPreviousPeriodsReturnsCorrectCount()
        {
            var target = SamplePriceDatas.Deere.PriceSeries;

            const int requestedCount = 5;
            var previousPeriods = _timeSeriesUtility.GetPreviousTimePeriods(target, requestedCount, target.Tail.AddTicks(1));
            Assert.AreEqual(requestedCount, previousPeriods.Count());
        }

        [TestMethod]
        public void GetPreviousPeriodsReturnsCorrectCountWhenMaxExceedsPeriods()
        {
            var target = SamplePriceDatas.Deere.PriceSeries;

            var totalPeriodCount = target.PricePeriods.Count();
            var requestedCount = totalPeriodCount + 1;
            var previousPeriods = _timeSeriesUtility.GetPreviousTimePeriods(target, requestedCount, target.Tail.AddTicks(1));
            Assert.AreEqual(totalPeriodCount, previousPeriods.Count());
        }

        [TestMethod]
        public void GetPreviousPeriodsReturnsCorrectPeriods()
        {
            var target = SamplePriceDatas.Deere.PriceSeries;

            const int requestedCount = 3;
            var previousPeriods = _timeSeriesUtility.GetPreviousTimePeriods(target, requestedCount, target.Tail.AddTicks(1)).ToArray();
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
            var target = SamplePriceDatas.Deere.PriceSeries;
            var periodCount = target.PricePeriods.Count();
            var origin = target.PricePeriods.ToArray()[periodCount - 1].Head;

            var expected = target.PricePeriods.ToArray()[periodCount - 2];
            var actual = _timeSeriesUtility.GetPreviousTimePeriod(target, origin);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPreviousPeriodFromTail()
        {
            var target = SamplePriceDatas.Deere.PriceSeries;
            var periodCount = target.PricePeriods.Count();
            var origin = target.PricePeriods.ToArray()[periodCount - 1].Tail;

            var expected = target.PricePeriods.ToArray()[periodCount - 2];
            var actual = _timeSeriesUtility.GetPreviousTimePeriod(target, origin);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPreviousPeriodFromSaturday()
        {
            var target = SamplePriceDatas.Deere.PriceSeries;
            var origin = new DateTime(2011, 6, 25);
            var pricePeriods = _timeSeriesUtility.GetPreviousPricePeriods(target, 2, origin).ToArray();

            var expected = pricePeriods[pricePeriods.Count() - 1];
            var actual = _timeSeriesUtility.GetPreviousTimePeriod(target, origin);
            Assert.AreEqual(expected, actual);
        }
    }
}
