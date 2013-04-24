using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.TechnicalAnalysis;
using Sonneville.PriceTools.Test.PriceData;
using Statistics;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public class ExtensionsTest
    {
        [TestMethod]
        public void CorrelationIbmDeereTest()
        {
            var ibm = TestPriceSeries.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var de = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            const int lookback = 20;
            var ibmDecimals = ibm.GetPreviousPricePeriods(lookback, ibm.Tail).Select(x => x.Close);
            var deDecimals = de.GetPreviousPricePeriods(lookback, ibm.Tail).Select(x => x.Close);

            var expected = ibmDecimals.Correlation(deDecimals);
            var actual = ibm.Correlation(de, ibm.Tail);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CorrelationIbmDeereTestWithLookback()
        {
            var ibm = TestPriceSeries.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var de = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            const int lookback = 10;
            var ibmDecimals = ibm.GetPreviousPricePeriods(lookback, ibm.Tail).Select(x => x.Close);
            var deDecimals = de.GetPreviousPricePeriods(lookback, ibm.Tail).Select(x => x.Close);

            var expected = ibmDecimals.Correlation(deDecimals);
            var actual = ibm.Correlation(de, ibm.Tail, lookback);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CorrelationIbmDeereTestAllValues()
        {
            var ibm = TestPriceSeries.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var de = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            const int lookback = 10;
            var ibmDecimals = ibm.GetPreviousPricePeriods(lookback, ibm.Tail).Select(x => x.Close);
            var deDecimals = de.GetPreviousPricePeriods(lookback, ibm.Tail).Select(x => x.Close);

            var expected = ibmDecimals.Correlation(deDecimals);
            var actual = ibm.Correlation(de, lookback).TimePeriods.Last().Value();
            Assert.AreEqual(expected, actual);
        }
    }
}
