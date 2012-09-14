using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.TechnicalAnalysis;
using Sonneville.PriceTools.Test.PriceData;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public class CommonIndicatorTests
    {
        [TestMethod]
        public void ResolutionDaysByDefault()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(20, date, 1);

            const int lookback = 5;
            var target = new SimpleMovingAverage(priceSeries, lookback);

            const Resolution expected = Resolution.Days;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void QueryBeforeHeadThrowsException()
        {
            var series = CreateTestPriceSeries(4, new DateTime(2011, 1, 6), 2);
            var ma = new SimpleMovingAverage(series, 2);

            var result = ma[ma.Head.Subtract(new TimeSpan(1))];
        }

        [TestMethod]
        public void HeadTest()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);
            const int lookback = 4;

            var target = new SimpleMovingAverage(priceSeries, lookback);

            var expected = priceSeries.PricePeriods[target.Lookback - 1].Head;
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IndexerTest1()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);
            const int lookback = 4;

            var target = new SimpleMovingAverage(priceSeries, lookback);

            var testDate = date.AddDays(lookback);
            var expected = priceSeries[testDate];
            var actual = target[testDate];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IndexerTest2()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);
            const int lookback = 4;

            var target = new SimpleMovingAverage(priceSeries, lookback);

            var expected = priceSeries[lookback];
            var actual = target[0];
            Assert.AreEqual(expected, actual);
        }

        private static IPriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price)
        {
            var series = PriceSeriesFactory.CreatePriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            for (var i = 0; i < count; i++)
            {
                var period = PricePeriodFactory.ConstructTickedPricePeriod();
                period.AddPriceTicks(PriceTickFactory.ConstructPriceTick(startDate.AddDays(i), price));
                series.AddPriceData(period);
            }
            return series;
        }

        [TestMethod]
        public void TimePeriodsTest()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var target = new SimpleMovingAverage(priceSeries, 5);
            var iPricePeriods = typeof(SimpleMovingAverage).GetProperty("PricePeriods").GetMethod;
            var pricePeriodsParams = new object[] {};
            var iTimePeriods = typeof (SimpleMovingAverage).GetProperty("TimePeriods").GetMethod;
            var timePeriodsParams = new object[] {};

            TestPricePeriodsMatchesTimePeriods(target, iPricePeriods, pricePeriodsParams, iTimePeriods, timePeriodsParams);
        }

        [TestMethod]
        public void GetTimePeriods1Test()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var target = new SimpleMovingAverage(priceSeries, 5);
            var pricePeriodsParams = new object[] { };
            var timePeriodsParams = new object[] { };
            var pricePeriodTypes = SetPricePeriodTypes(pricePeriodsParams);
            var iPricePeriods = typeof(SimpleMovingAverage).GetMethod("GetPricePeriods", pricePeriodTypes);
            var timePeriodTypes = SetTimePeriodTypes(timePeriodsParams);
            var iTimePeriods = typeof(SimpleMovingAverage).GetMethod("GetTimePeriods", timePeriodTypes);

            TestPricePeriodsMatchesTimePeriods(target, iPricePeriods, pricePeriodsParams, iTimePeriods, timePeriodsParams);
        }

        [TestMethod]
        public void GetTimePeriods2Test()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var target = new SimpleMovingAverage(priceSeries, 5);
            var resolution = target.Resolution;
            var pricePeriodsParams = new object[] { resolution };
            var timePeriodsParams = new object[] { resolution };
            var pricePeriodTypes = SetPricePeriodTypes(pricePeriodsParams);
            var iPricePeriods = typeof(SimpleMovingAverage).GetMethod("GetPricePeriods", pricePeriodTypes);
            var timePeriodTypes = SetTimePeriodTypes(timePeriodsParams);
            var iTimePeriods = typeof(SimpleMovingAverage).GetMethod("GetTimePeriods", timePeriodTypes);

            TestPricePeriodsMatchesTimePeriods(target, iPricePeriods, pricePeriodsParams, iTimePeriods, timePeriodsParams);
        }

        [TestMethod]
        public void GetTimePeriods3Test()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var target = new SimpleMovingAverage(priceSeries, 5);
            var resolution = target.Resolution;
            var head = target.Head;
            var tail = target.Tail;
            var pricePeriodsParams = new object[] { resolution, head, tail };
            var timePeriodsParams = new object[] { resolution, head, tail };
            var pricePeriodTypes = SetPricePeriodTypes(pricePeriodsParams);
            var iPricePeriods = typeof (SimpleMovingAverage).GetMethod("GetPricePeriods", pricePeriodTypes);
            var timePeriodTypes = SetTimePeriodTypes(timePeriodsParams);
            var iTimePeriods = typeof(SimpleMovingAverage).GetMethod("GetTimePeriods", timePeriodTypes);

            TestPricePeriodsMatchesTimePeriods(target, iPricePeriods, pricePeriodsParams, iTimePeriods, timePeriodsParams);
        }

        private static Type[] SetTimePeriodTypes(object[] timePeriodsParams)
        {
            var timePeriodTypes = new Type[timePeriodsParams.Length];
            for (var i = 0; i < timePeriodsParams.Length; i++)
            {
                var param = timePeriodsParams[i].GetType();
                timePeriodTypes[i] = param;
            }
            return timePeriodTypes;
        }

        private static Type[] SetPricePeriodTypes(object[] pricePeriodsParams)
        {
            var pricePeriodTypes = new Type[pricePeriodsParams.Length];
            for (var i = 0; i < pricePeriodsParams.Length; i++)
            {
                var param = pricePeriodsParams[i].GetType();
                pricePeriodTypes[i] = param;
            }
            return pricePeriodTypes;
        }

        private static void TestPricePeriodsMatchesTimePeriods(IIndicator target, MethodInfo iPricePeriods, object[] pricePeriodsParams, MethodInfo iTimePeriods, object[] timePeriodsParams)
        {
            var checkExecption = false;
            Exception pricePeriodsException = null;
            Exception timePeriodsException = null;
            List<ITimePeriod> pricePeriods = null;
            List<ITimePeriod> timePeriods = null;

            try
            {
                pricePeriods = ((IList<IPricePeriod>)iPricePeriods.Invoke(target, pricePeriodsParams)).Cast<ITimePeriod>().ToList();
            }
            catch (Exception e)
            {
                checkExecption = true;
                pricePeriodsException = e;
            }

            try
            {
                timePeriods = ((IList<ITimePeriod>)iTimePeriods.Invoke(target, timePeriodsParams)).ToList();
            }
            catch (Exception e)
            {
                checkExecption = true;
                timePeriodsException = e;
            }

            if (checkExecption)
            {
                if (pricePeriodsException == null || timePeriodsException == null)
                {
                    Assert.Fail();
                }

                Assert.AreEqual(pricePeriodsException.GetType(), timePeriodsException.GetType());
            }
            else
            {
                CollectionAssert.AreEquivalent(pricePeriods, timePeriods);
            }
        }
    }
}