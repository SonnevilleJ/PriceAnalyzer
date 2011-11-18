using System;
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
        public void Calculates14PeriodSingleCorrect()
        {
            var priceSeries = new YahooPriceHistoryCsvFile(new ResourceStream(TestData.DE_1_1_2011_to_3_15_2011_Daily_Yahoo)).PriceSeries;
            var target = new RelativeStrengthIndex(priceSeries);

            const decimal expected = 81.42m;
            var actual = target[target.Head];
            Assert.AreEqual(expected, Math.Round(actual, 2));
        }

        [TestMethod]
        public void Calculates14PeriodAllCorrect()
        {
            var expected = new[]
                               {
                                   81.42m,
                                   75.70m,
                                   79.16m,
                                   79.08m,
                                   65.17m,
                                   70.54m,
                                   74.86m,
                                   76.86m,
                                   73.46m,
                                   71.94m
                               };
            var priceSeries = new YahooPriceHistoryCsvFile(new ResourceStream(TestData.DE_1_1_2011_to_3_15_2011_Daily_Yahoo)).PriceSeries;
            var target = new RelativeStrengthIndex(priceSeries);

            target.CalculateAll();

            var index = target.Head;
            var lookback = target.Lookback;
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(Math.Round(target[index], 2), expected[i - lookback]);
                index = index.GetFollowingOpen();
            }
        }
    }
}
