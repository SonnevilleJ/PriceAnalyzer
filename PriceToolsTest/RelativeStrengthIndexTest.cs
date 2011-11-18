using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.SamplePriceData;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    /// Summary description for RelativeStrengthIndexTest
    /// </summary>
    [TestClass]
    public class RelativeStrengthIndexTest
    {
        //
        // The algorithms in the RelativeStrengthIndicator class are based on an Excel calculator from the following article:
        // http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:relative_strength_index_rsi
        // See the cs-rsi.xls file in the Resources folder.
        //

        [TestInitialize]
        public void TestInitialize()
        {
            Settings.SetDefaultSettings();
            Settings.CanConnectToInternet = false;
        }

        [TestMethod]
        public void CalculatesCorrectly14Periods()
        {
            var expected = new[]
                               {
                                   81.42,
                                   75.70,
                                   79.16,
                                   79.08,
                                   65.17,
                                   70.54,
                                   74.86,
                                   76.86,
                                   73.46,
                                   71.94
                               };
            var priceSeries = new YahooPriceHistoryCsvFile(new ResourceStream(TestData.DE_1_1_2011_to_3_15_2011_Daily_Yahoo)).PriceSeries;
            var target = new RelativeStrengthIndex(priceSeries);

            target.CalculateAll();

            for (var i = 14; i < 24; i++)
            {
                var index = priceSeries.Head.GetFollowingOpen();
                Assert.AreEqual(target[index], expected[i - 14]);
            }
        }
    }
}
