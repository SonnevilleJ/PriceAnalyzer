using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.TechnicalAnalysis;
using Sonneville.PriceTools.Test.PriceData;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public abstract class CommonIndicatorTests<T> where T : ITimeSeriesIndicator
    {
        protected static readonly IPricePeriodFactory PricePeriodFactory;
        protected static readonly IPriceSeriesFactory PriceSeriesFactory;
        private static readonly IPriceTickFactory PriceTickFactory;

        static CommonIndicatorTests()
        {
            PricePeriodFactory = new PricePeriodFactory();
            PriceSeriesFactory = new PriceSeriesFactory();
            PriceTickFactory = new PriceTickFactory();
        }

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
        /// Gets an instance of the <see cref="TimeSeriesIndicator"/> to test, using a default lookback period.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to transform.</param>
        /// <returns></returns>
        protected T GetTestObjectInstance(ITimeSeries timeSeries)
        {
            return GetTestObjectInstance(timeSeries, GetDefaultLookback());
        }

        /// <summary>
        /// Gets an instance of the <see cref="TimeSeriesIndicator"/> to test, using a specific lookback period.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to transform.</param>
        /// <param name="lookback">The lookback period the <see cref="TimeSeriesIndicator"/> should use.</param>
        /// <returns></returns>
        protected abstract T GetTestObjectInstance(ITimeSeries timeSeries, int lookback);

        [TestMethod]
        public void ResolutionMatchesPriceSeries()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback(), date, 1);

            var target = GetTestObjectInstance(priceSeries);

            var expected = priceSeries.Resolution;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadIsCorrectForNoWeekendData()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            var target = GetTestObjectInstance(priceSeries);

            // must use MeasuredTimeSeries instead of priceSeries - target may be an indicator of an indicator, thus elongating the Lookback
            var expected = priceSeries.TimePeriods.ElementAt(GetCumulativeLookback() - 1).Head;
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadIsCorrectForWeekendData()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback() * 2, date, 1, Resolution.Days, true);

            var target = GetTestObjectInstance(priceSeries);

            // must use MeasuredTimeSeries instead of priceSeries - target may be an indicator of an indicator, thus elongating the Lookback
            var expected = priceSeries.TimePeriods.ElementAt(GetCumulativeLookback() - 1).Head;
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadDoesNotThrowException()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback() * 2, date, 1);

            var target = GetTestObjectInstance(priceSeries);

            var dateTime = target.Head;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void QueryBeforeHeadThrowsException()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback(), date, 1);

            var target = GetTestObjectInstance(priceSeries);

            var result = target[target.Head.AddTicks(-1)];
        }

        [TestMethod]
        public void QueryAtHeadDoesNotThrowException()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback() * 2, date, 1);

            var target = GetTestObjectInstance(priceSeries);

            var result = target[target.Head];
        }

        [TestMethod]
        public void PeriodsMatchMeasuredTimeSeries()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var target = GetTestObjectInstance(priceSeries);

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

            var target = GetTestObjectInstance(priceSeries);

            Assert.IsTrue(target.TimePeriods.Any());
        }

        [TestMethod]
        public void CalculateAllCompletesWithoutThrowing()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback() * 2, date, 1);

            var target = GetTestObjectInstance(priceSeries);

            target.CalculateAll();
        }

        protected static IPriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price, Resolution resolution = Resolution.Days, bool weekendData = false)
        {
            var series = PriceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
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

        [TestMethod]
        public void CalculateFirstPeriodCorrectly()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            var target = GetTestObjectInstance(priceSeries);

            var expected = GetExpectedValues(target.Lookback)[0];
            var actual = target[target.Head];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNext10PeriodsCorrectly()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            var target = GetTestObjectInstance(priceSeries);

            var lookback = target.Lookback;
            for (var i = 1; i < 10; i++)
            {
                var expected = GetExpectedValues(lookback)[i];
                var actual = target.TimePeriods.ToArray()[i].Value();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void SetLookbackClearsAllCachedValues()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var target = GetTestObjectInstance(priceSeries);

            target.CalculateAll();
            var expected = target.TimePeriods.ToArray();

            target.Lookback = target.Lookback + 1;

            target.CalculateAll();
            var actual = target.TimePeriods.ToArray();

            CollectionAssert.AreNotEqual(expected, actual);
        }

        /// <summary>
        /// Gets a list of expected values for a given lookback period.
        /// </summary>
        /// <param name="lookback"></param>
        /// <returns></returns>
        protected abstract decimal[] GetExpectedValues(int lookback);
    }
}