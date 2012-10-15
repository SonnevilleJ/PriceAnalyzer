using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.TechnicalAnalysis;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public class RsiAverageGainsTest : ParentIndicatorTestBase
    {
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

        #region Overrides of CommonIndicatorTests

        /// <summary>
        /// The default lookback period to use when creating test instances.
        /// </summary>
        protected override int GetDefaultLookback()
        {
            return 14;
        }

        /// <summary>
        /// The cumulative lookback period for this indicator and all sub-indicators.
        /// </summary>
        /// <returns></returns>
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
            return new RsiAverageGains(timeSeries, lookback);
        }

        protected override decimal[] Get11ExpectedValues(int lookback)
        {
            if(lookback == 14) return _expected14;
            throw new ArgumentOutOfRangeException("lookback", lookback,
                                                  String.Format("Cannot return expected values for lookback period of length {0}", lookback));
        }

        #endregion
    }
}