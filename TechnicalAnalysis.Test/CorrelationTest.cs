using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.SampleData;
using Sonneville.Statistics;

namespace Sonneville.PriceTools.TechnicalAnalysis.Test
{
    [TestClass]
    public class CorrelationTest
    {
        private ITimeSeriesUtility _timeSeriesUtility;

        [TestInitialize]
        public void Initialize()
        {
            _timeSeriesUtility = new TimeSeriesUtility();
        }

        [TestMethod]
        public void CorrelationIbmDeereTest()
        {
            var ibm = SamplePriceDatas.IBM_Daily.PriceSeries;
            var de = SamplePriceDatas.Deere.PriceSeries;

            const int lookback = 20;
            var ibmDecimals = _timeSeriesUtility.GetPreviousPricePeriods(ibm, lookback, ibm.Tail).Select(x => x.Close);
            var deDecimals = _timeSeriesUtility.GetPreviousPricePeriods(de, lookback, ibm.Tail).Select(x => x.Close);

            var expected = ibmDecimals.Correlation(deDecimals);
            var actual = new Correlation(ibm, lookback, de)[ibm.Tail];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CorrelationMsftDeereTest()
        {
            var ibm = SamplePriceDatas.IBM_Daily.PriceSeries;
            var msft = SamplePriceDatas.MSFT.PriceSeries;

            const int lookback = 20;
            var actual = new Correlation(ibm, lookback, msft)[ibm.Tail];

            Assert.IsNotNull(actual);
        }
    }
}
