using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.SampleData;
using Sonneville.Statistics;

namespace Sonneville.PriceTools.TechnicalAnalysis.Test
{
    [TestClass]
    public class IndicatorExtensionsTest
    {
        private IPriceSeries _ibm;
        private IPriceSeries _de;
        private ITimeSeriesUtility _timeSeriesUtility;

        [TestInitialize]
        public void Initialize()
        {
            _de = SamplePriceDatas.Deere.PriceSeries;
            _ibm = SamplePriceDatas.IBM_Daily.PriceSeries;
            _timeSeriesUtility = new TimeSeriesUtility();
        }

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
            var ibmDecimals = _timeSeriesUtility.GetPreviousPricePeriods(_ibm, lookback, _ibm.Tail).Select(x => x.Close);
            var deDecimals = _timeSeriesUtility.GetPreviousPricePeriods(_de, lookback, _ibm.Tail).Select(x => x.Close);

            var expected = ibmDecimals.Correlation(deDecimals);
            return expected;
        }
    }
}
