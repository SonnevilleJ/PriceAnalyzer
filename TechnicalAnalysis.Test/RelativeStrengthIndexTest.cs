using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.TechnicalAnalysis;
using Sonneville.PriceTools.Test.PriceData;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
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
            var priceSeries = TestPriceSeries.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var target = new RelativeStrengthIndex(priceSeries);

            const decimal expected = 85.22m;
            var actual = target[target.Head];
            Assert.AreEqual(expected, Math.Round(actual, 2));
        }

        [TestMethod]
        public void Calculates14PeriodAllCorrect()
        {
            var results = new[]
                              {
                                  85.22m,
                                  86.72m,
                                  84.67m,
                                  84.70m,
                                  75.16m,
                                  78.98m,
                                  80.77m,
                                  79.55m,
                                  79.84m,
                                  80.45m
                              };
            var priceSeries = TestPriceSeries.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var target = new RelativeStrengthIndex(priceSeries);

            target.CalculateAll();

            var index = target.Head;
            var lookback = target.Lookback;
            for (var i = lookback; i < results.Length + lookback; i++, index = index.NextPeriodOpen(target.Resolution))
            {
                var expected = results[i - lookback];
                var actual = target[index];
                Assert.AreEqual(expected, Math.Round(actual, 2));
            }
        }

        [TestMethod]
        public void Calculates10PeriodSingleCorrect()
        {
            var priceSeries = TestPriceSeries.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var target = new RelativeStrengthIndex(priceSeries, 10);

            const decimal expected = 70.66m;
            var actual = target[target.Head];
            Assert.AreEqual(expected, Math.Round(actual, 2));
        }

        [TestMethod]
        public void Calculates10PeriodAllCorrect()
        {
            var results = new[]
                              {
                                  70.66m,
                                  83.04m,
                                  83.22m,
                                  80.74m,
                                  86.77m,
                                  88.52m,
                                  85.73m,
                                  85.77m,
                                  72.68m,
                                  78.22m
                              };
            var priceSeries = TestPriceSeries.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var target = new RelativeStrengthIndex(priceSeries, 10);

            target.CalculateAll();

            var index = target.Head;
            var lookback = target.Lookback;
            for (var i = lookback; i < results.Length + lookback; i++, index = index.NextPeriodOpen(target.Resolution))
            {
                var expected = results[i - lookback];
                var actual = target[index];
                Assert.AreEqual(expected, Math.Round(actual, 2));
            }
        }
    }
}
