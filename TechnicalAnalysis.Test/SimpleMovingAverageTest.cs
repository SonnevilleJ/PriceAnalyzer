using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.TechnicalAnalysis.Test
{
    [TestClass]
    public class SimpleMovingAverageTest : CommonIndicatorTests<SimpleMovingAverage>
    {
        private decimal[] _expected4
        {
            get
            {
                return new[]
                           {
                               83.775m,
                               83.9625m,
                               84.3525m,
                               84.7075m,
                               85.5075m,
                               86.79m,
                               88.025m,
                               89.26m,
                               89.7475m,
                               89.695m
                           };
            }
        }

        /// <summary>
        /// The default lookback period to use when creating test instances.
        /// </summary>
        protected override int GetDefaultLookback()
        {
            return 4;
        }

        /// <summary>
        /// Gets an instance of the <see cref="TimeSeriesIndicator"/> to test, using a specific lookback period.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to transform.</param>
        /// <param name="lookback">The lookback period the <see cref="TimeSeriesIndicator"/> should use.</param>
        /// <returns></returns>
        protected override SimpleMovingAverage GetTestObjectInstance(ITimeSeries timeSeries, int lookback)
        {
            return new SimpleMovingAverage(timeSeries, lookback);
        }

        protected override decimal[] GetExpectedValues(int lookback)
        {
            switch (lookback)
            {
                case 4:
                    return _expected4;
                default:
                    Assert.Inconclusive("Expected values for lookback period of {0} are unknown.", lookback);
// ReSharper disable once HeuristicUnreachableCode
                    return null;
            }
        }

        [TestMethod]
        public void IndexerTest()
        {
            var date = new DateTime(2011, 3, 1);
            var priceSeries = CreateTestPriceSeries(10, date, 1);

            var target = GetTestObjectInstance(priceSeries);

            var testDate = date.SeekPeriods(target.Lookback, priceSeries.Resolution);
            var expected = priceSeries[testDate];
            var actual = target[testDate];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FlatPeriodReturnsSameAverage()
        {
            var date = new DateTime(2011, 3, 1);
            const int price = 2;

            var series = CreateTestPriceSeries(10, date, price);

            const int lookback = 2;
            var ma = new SimpleMovingAverage(series, lookback);

            const decimal expected = price;
            for (var i = lookback; i < series.PricePeriods.Count(); i++)
            {
                var actual = ma[date.SeekPeriods(i, series.Resolution)];
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
