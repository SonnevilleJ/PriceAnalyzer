using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.TechnicalAnalysis;

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
        public void ResolutionDaysMatchesPriceSeries()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(20, date, 1);

            var target = GetTestInstance(priceSeries);

            var expected = priceSeries.Resolution;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void QueryBeforeHeadThrowsException()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);

            var target = GetTestInstance(priceSeries);

            // CreateTestPriceSeries above does NOT create a full period for the resolution (TickedPricePeriodImpl)
            var tail = priceSeries.PricePeriods.ToArray()[target.Lookback - 1].Tail;
            var result = target[tail.AddTicks(-1)];
        }

        [TestMethod]
        public void QueryAtFirstPeriodTailDoesNotThrowException()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);

            var target = GetTestInstance(priceSeries);

            // CreateTestPriceSeries above does NOT create a full period for the resolution (TickedPricePeriodImpl)
            var tail = priceSeries.PricePeriods.ToArray()[target.Lookback - 1].Tail;
            var result = target[tail];
        }

        [TestMethod]
        public void CalculateAllCompletesWithoutThrowing()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);

            var target = GetTestInstance(priceSeries);

            target.CalculateAll();
        }

        protected static IPriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price)
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
    }
}