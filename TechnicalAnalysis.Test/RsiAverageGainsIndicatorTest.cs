using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.TechnicalAnalysis;
using Sonneville.PriceTools.Test.PriceData;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public class RsiAverageGainsIndicatorTest : CommonIndicatorTests
    {
        #region Overrides of CommonIndicatorTests

        /// <summary>
        /// The default lookback period to use when creating test instances.
        /// </summary>
        protected override int GetDefaultLookback()
        {
            return 14;
        }

        protected override int GetCumulativeLookback()
        {
            return GetDefaultLookback() + 1;
        }

        /// <summary>
        /// Gets an instance of the <see cref="Indicator"/> to test, using a specific lookback period.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to transform.</param>
        /// <param name="lookback">The lookback period the <see cref="Indicator"/> should use.</param>
        /// <returns></returns>
        protected override Indicator GetTestInstance(ITimeSeries timeSeries, int lookback)
        {
            return new RsiGainsIndicator(timeSeries);
        }

        #endregion

        private readonly decimal[] _expected14 =
            {
                0.6042857142857142857142857143m,
                0.5611224489795918367346938776m,
                0.6353279883381924198250728864m,
                0.5899474177426072469804248231m,
                0.5478083164752781579103944786m,
                0.6508220081556154323453663016m,
                0.7514775790016429014635544229m,
                0.7785148947872398370733005356m,
                0.7229066880167227058537790688m,
                0.671270496015528226864223421m,
                0.6897511748715619249453503195m
            };

        [TestMethod]
        public void MeasuredTimeSeriesMeasuredTimeSeriesIsPriceSeries()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            var target = GetTestInstance(priceSeries, 14);

            Assert.AreEqual(priceSeries, ((Indicator)target.MeasuredTimeSeries).MeasuredTimeSeries);
        }

        [TestMethod]
        public void CalculateFirstPeriodCorrectly()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            var target = GetTestInstance(priceSeries, 14);

            var expected = _expected14[0];
            var actual = target[target.Head];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNext10PeriodsCorrectly()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            var target = GetTestInstance(priceSeries, 14);
            target.CalculateAll();

            for (var i = 1; i < _expected14.Length; i++)
            {
                var expected = _expected14[i];
                var actual = target.TimePeriods.ToArray()[i].Value();
                Assert.AreEqual(expected, actual);
            }
        }
    }
}