﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.SampleData;

namespace Sonneville.PriceTools.TechnicalAnalysis.Test
{
    [TestClass]
    public abstract class CommonIndicatorTests<T> where T : ITimeSeriesIndicator
    {
        private readonly IPricePeriodFactory _pricePeriodFactory;
        private readonly IPriceSeriesFactory _priceSeriesFactory;

        protected CommonIndicatorTests()
        {
            _pricePeriodFactory = new PricePeriodFactory();
            _priceSeriesFactory = new PriceSeriesFactory();
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
        protected T GetTestObjectInstance(ITimeSeries<ITimePeriod> timeSeries)
        {
            return GetTestObjectInstance(timeSeries, GetDefaultLookback());
        }

        /// <summary>
        /// Gets an instance of the <see cref="TimeSeriesIndicator"/> to test, using a specific lookback period.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to transform.</param>
        /// <param name="lookback">The lookback period the <see cref="TimeSeriesIndicator"/> should use.</param>
        /// <returns></returns>
        protected abstract T GetTestObjectInstance(ITimeSeries<ITimePeriod> timeSeries, int lookback);

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
            var priceSeries = SamplePriceDatas.Deere.PriceSeries;

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

            Assert.IsNotNull(target.Head);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void QueryBeforeHeadThrowsException()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback(), date, 1);

            var target = GetTestObjectInstance(priceSeries);

            var result = target[target.Head.AddTicks(-1)];

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void QueryAtHeadDoesNotThrowException()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(GetCumulativeLookback() * 2, date, 1);

            var target = GetTestObjectInstance(priceSeries);

            var result = target[target.Head];

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PeriodsMatchMeasuredTimeSeries()
        {
            var priceSeries = SamplePriceDatas.Deere.PriceSeries;
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

        protected IPriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price, Resolution resolution = Resolution.Days, bool weekendData = false)
        {
            var series = _priceSeriesFactory.ConstructPriceSeries("DE");
            for (var i = 0; i < count; i++)
            {
                var weekdayDateShifter = new Func<DateTime, int, Resolution, DateTime>((origin, periods, res) => origin.SeekTradingPeriods(periods, res));
                var weekendDateShifter = new Func<DateTime, int, Resolution, DateTime>((origin, periods, res) => origin.SeekPeriods(periods, res));

                var dateShifter = weekendData ? weekendDateShifter : weekdayDateShifter;
                var dateTime = dateShifter(startDate, i, resolution);
                var period = _pricePeriodFactory.ConstructStaticPricePeriod(dateTime, resolution, price);
                series.AddPriceData(period);
            }
            return series;
        }

        [TestMethod]
        public void CalculateFirstPeriodCorrectly()
        {
            var priceSeries = SamplePriceDatas.Deere.PriceSeries;

            var target = GetTestObjectInstance(priceSeries);

            var expected = GetExpectedValues(target.Lookback)[0];
            var actual = target[target.Head];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNext10PeriodsCorrectly()
        {
            var priceSeries = SamplePriceDatas.Deere.PriceSeries;

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
            var priceSeries = SamplePriceDatas.Deere.PriceSeries;
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