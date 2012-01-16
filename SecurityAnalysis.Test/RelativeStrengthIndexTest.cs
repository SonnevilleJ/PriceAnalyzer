using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.SamplePriceData;

namespace Sonneville.PriceTools.SecurityAnalysis.Test
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

        [TestMethod]
        public void Calculates14PeriodSingleCorrect()
        {
            var priceSeries = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var target = new RelativeStrengthIndex(priceSeries);

            const decimal expected = 81.42m;
            var actual = target[target.Head];
            Assert.AreEqual(expected, Math.Round(actual, 2));
        }

        [TestMethod]
        public void Calculates14PeriodAllCorrect()
        {
            var results = new[]
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
            var priceSeries = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var target = new RelativeStrengthIndex(priceSeries);

            target.CalculateAll();

            var index = target.Head;
            var lookback = target.Lookback;
            for (var i = lookback; i < results.Length + lookback; i++, index = index.GetFollowingOpen())
            {
                var expected = results[i - lookback];
                var actual = target[index];
                Assert.AreEqual(expected, Math.Round(actual, 2));
            }
        }

        [TestMethod]
        public void Calculates10PeriodSingleCorrect()
        {
            var priceSeries = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var target = new RelativeStrengthIndex(priceSeries, 10);

            const decimal expected = 93.01m;
            var actual = target[target.Head];
            Assert.AreEqual(expected, Math.Round(actual, 2));
        }

        [TestMethod]
        public void Calculates10PeriodAllCorrect()
        {
            var results = new[]
                              {
                                  93.01m,
                                  79.93m,
                                  78.50m,
                                  78.28m,
                                  80.92m,
                                  72.76m,
                                  78.13m,
                                  78.02m,
                                  59.36m,
                                  67.63m
                              };
            var priceSeries = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var target = new RelativeStrengthIndex(priceSeries, 10);

            target.CalculateAll();

            var index = target.Head;
            var lookback = target.Lookback;
            for (var i = lookback; i < results.Length + lookback; i++, index = index.GetFollowingOpen())
            {
                var expected = results[i - lookback];
                var actual = target[index];
                Assert.AreEqual(expected, Math.Round(actual, 2));
            }
        }
    }
}
