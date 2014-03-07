﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.SampleData;
using Sonneville.Statistics;

namespace Sonneville.PriceTools.TechnicalAnalysis.Test
{
    [TestClass]
    public class IndicatorExtensionsTest
    {
        private readonly IPriceSeries _ibm = SamplePriceDatas.IBM_Daily.PriceSeries;
        private readonly IPriceSeries _de = SamplePriceDatas.Deere.PriceSeries;

        [TestMethod]
        public void CorrelationIbmDeereTest()
        {
            const int lookback = 20;
            var expected = GetExpectedCorrelation(lookback);
            var actual = new Correlation(_ibm, 20, _de)[_ibm.Tail];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CorrelationIbmDeereTestWithLookback()
        {
            const int lookback = 10;
            var expected = GetExpectedCorrelation(lookback);
            var actual = new Correlation(_ibm, lookback, _de)[_ibm.Tail];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CorrelationIbmDeereTestAllValues()
        {
            const int lookback = 10;
            var expected = GetExpectedCorrelation(lookback);
            var actual = new Correlation(_ibm, lookback, _de).TimePeriods.Last().Value();
            Assert.AreEqual(expected, actual);
        }

        private decimal GetExpectedCorrelation(int lookback)
        {
            var ibmDecimals = new TimeSeriesUtility().GetPreviousPricePeriods(_ibm, lookback, _ibm.Tail).Select(x => x.Close);
            var deDecimals = new TimeSeriesUtility().GetPreviousPricePeriods(_de, lookback, _ibm.Tail).Select(x => x.Close);

            var expected = ibmDecimals.Correlation(deDecimals);
            return expected;
        }
    }
}
