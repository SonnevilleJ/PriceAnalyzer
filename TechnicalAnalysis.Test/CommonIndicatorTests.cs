using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.TechnicalAnalysis;
using Sonneville.PriceTools.Test.PriceData;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public abstract class CommonIndicatorTests
    {
        /// <summary>
        /// The default lookback period to use when creating test instances.
        /// </summary>
        protected abstract int GetDefaultLookback();

        /// <summary>
        /// The cumulative lookback period for this indicator and all sub-indicators.
        /// </summary>
        /// <returns></returns>
        protected virtual int GetCumulativeLookback()
        {
            return GetDefaultLookback();
        }

        /// <summary>
        /// Gets an instance of the <see cref="Indicator"/> to test, using a default lookback period.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to transform.</param>
        /// <returns></returns>
        protected Indicator GetTestInstance(ITimeSeries timeSeries)
        {
            return GetTestInstance(timeSeries, GetDefaultLookback());
        }

        /// <summary>
        /// Gets an instance of the <see cref="Indicator"/> to test, using a specific lookback period.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to transform.</param>
        /// <param name="lookback">The lookback period the <see cref="Indicator"/> should use.</param>
        /// <returns></returns>
        protected abstract Indicator GetTestInstance(ITimeSeries timeSeries, int lookback);

        [TestMethod]
        public void ResolutionMatchesPriceSeries()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback(), date, 1);

            var target = GetTestInstance(priceSeries);

            var expected = priceSeries.Resolution;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadIsCorrectForNoWeekendData()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback(), date, 1, Resolution.Days, false);

            var target = GetTestInstance(priceSeries);

            // must use MeasuredTimeSeries instead of priceSeries - target may be an indicator of an indicator, thus elongating the Lookback
            var expected = target.MeasuredTimeSeries.Head.SeekTradingPeriods(target.Lookback - 1, target.Resolution);
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadIsCorrectForWeekendData()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback() * 2, date, 1, Resolution.Days, true);

            var target = GetTestInstance(priceSeries);

            // must use MeasuredTimeSeries instead of priceSeries - target may be an indicator of an indicator, thus elongating the Lookback
            var expected = target.MeasuredTimeSeries.Head.SeekPeriods(target.Lookback - 1, target.Resolution);
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadDoesNotThrowException()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback() * 2, date, 1);

            var target = GetTestInstance(priceSeries);

            var dateTime = target.Head;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void QueryBeforeHeadThrowsException()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback(), date, 1);

            var target = GetTestInstance(priceSeries);

            var result = target[target.Head.AddTicks(-1)];
        }

        [TestMethod]
        public void QueryAtHeadDoesNotThrowException()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback() * 2, date, 1);

            var target = GetTestInstance(priceSeries);

            var result = target[target.Head];
        }

        [TestMethod]
        public void PeriodsMatchMeasuredTimeSeries()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var target = GetTestInstance(priceSeries);

            var psPeriods = priceSeries.PricePeriods.Where(p => p.Head >= target.Head);
            var tPeriods = target.TimePeriods.ToArray();

            foreach (var period in psPeriods)
            {
                Assert.AreEqual(1, tPeriods.Count(p => p.Head == period.Head));
            }
        }

        [TestMethod]
        public void TimePeriodsCalculatesAll()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback() * 2, date, 1);

            var target = GetTestInstance(priceSeries);

            Assert.IsTrue(target.TimePeriods.Any());
        }

        [TestMethod]
        public void CalculateAllCompletesWithoutThrowing()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback() * 2, date, 1);

            var target = GetTestInstance(priceSeries);

            target.CalculateAll();
        }

        protected static IPriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price, Resolution resolution = Resolution.Days, bool weekendData = false)
        {
            var series = PriceSeriesFactory.CreatePriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            for (var i = 0; i < count; i++)
            {
                var period = PricePeriodFactory.ConstructTickedPricePeriod();
                var weekdayDateShifter = new Func<DateTime, int, Resolution, DateTime>((origin, periods, res) => origin.SeekTradingPeriods(periods, res));
                var weekendDateShifter = new Func<DateTime, int, Resolution, DateTime>((origin, periods, res) => origin.SeekPeriods(periods, res));

                var dateShifter = weekendData ? weekendDateShifter : weekdayDateShifter;
                period.AddPriceTicks(PriceTickFactory.ConstructPriceTick(dateShifter(startDate, i, resolution), price));
                series.AddPriceData(period);
            }
            return series;
        }
    }
}