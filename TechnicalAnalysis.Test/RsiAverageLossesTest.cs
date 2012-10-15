using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.TechnicalAnalysis;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public class RsiAverageLossesTest : ParentIndicatorTestBase
    {
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
            return new RsiAverageLosses(timeSeries, lookback);
        }

        #endregion

        private readonly decimal[] _expected14 =
            {
                -0.1378571428571428571428571429m,
                -0.180153061224489795918367347m,
                -0.1672849854227405247813411079m,
                -0.1560503436068304872969596002m,
                -0.2927610333491997382043196288m,
                -0.2718495309671140426182967982m,
                -0.252431707326605896716989884m,
                -0.2344008710889911898086334637m,
                -0.2612293802969203905365882163m,
                -0.2618558531328546483554033437m,
                -0.2431518636233650306157316763m
            };

        /// <summary>
        /// Gets a list of expected values for a given lookback period.
        /// </summary>
        /// <param name="lookback"></param>
        /// <returns></returns>
        protected override decimal[] Get11ExpectedValues(int lookback)
        {
            if (lookback == 14) return _expected14;
            throw new ArgumentOutOfRangeException("lookback", lookback,
                                                  String.Format("Cannot return expected values for lookback period of length {0}", lookback));
        }
    }
}