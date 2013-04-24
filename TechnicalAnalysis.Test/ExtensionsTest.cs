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
        private readonly IPriceSeries _ibm = TestPriceSeries.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
        private readonly IPriceSeries _de = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

        [TestMethod]
        public void CorrelationIbmDeereTest()
        {
            const int lookback = 20;
            var expected = GetExpected(lookback);
            var actual = _ibm.Correlation(_de, _ibm.Tail);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CorrelationIbmDeereTestWithLookback()
        {
            const int lookback = 10;
            var expected = GetExpected(lookback);
            var actual = _ibm.Correlation(_de, _ibm.Tail, lookback);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CorrelationIbmDeereTestAllValues()
        {
            const int lookback = 10;
            var expected = GetExpected(lookback);
            var actual = _ibm.Correlation(_de, lookback).TimePeriods.Last().Value();
            Assert.AreEqual(expected, actual);
        }

        private decimal GetExpected(int lookback)
        {
            var ibmDecimals = _ibm.GetPreviousPricePeriods(lookback, _ibm.Tail).Select(x => x.Close);
            var deDecimals = _de.GetPreviousPricePeriods(lookback, _ibm.Tail).Select(x => x.Close);

            var expected = ibmDecimals.Correlation(deDecimals);
            return expected;
        }
    }
}
