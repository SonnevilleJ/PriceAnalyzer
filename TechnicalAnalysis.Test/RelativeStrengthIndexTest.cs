using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.TechnicalAnalysis;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// Summary description for RelativeStrengthIndexTest
    /// </summary>
    [TestClass]
    public class RelativeStrengthIndexTest : ParentIndicatorTestBase
    {
        //
        // The algorithms in the RelativeStrengthIndex class are based on an Excel calculator from the following article:
        // http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:relative_strength_index_rsi
        // See the cs-rsi.xls file in the Resources folder.
        //

        private readonly decimal[] _expected14 =
            {
                81.42444658325312800769971126m,
                75.69688209787321907908321288m,
                79.157453107334371608766919273m,
                79.081660603840080954001029371m,
                65.171102966063162898061236594m,
                70.536694864828394482539826322m,
                74.855127772563689999935239674m,
                76.858799222438717932028815194m,
                73.455969280289921181866567785m,
                71.937792414517368012939476741m,
                73.935998320291967941371047445m
            };

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
        protected override Indicator GetTestObjectInstance(ITimeSeries timeSeries, int lookback)
        {
            return new RelativeStrengthIndex(timeSeries, lookback);
        }

        #region Overrides of ParentIndicatorTestBase

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

        #endregion
    }
}
