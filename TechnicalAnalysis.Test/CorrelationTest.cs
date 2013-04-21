using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.TechnicalAnalysis;
using Sonneville.PriceTools.Test.PriceData;
using Statistics;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public class CorrelationTest
    {
        [TestMethod]
        public void CorrelationIbmDeereTest()
        {
            var ibm = TestPriceSeries.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            var de = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            var ibmDecimals = ibm.GetPreviousTimePeriods(20, ibm.Tail).Select(x => x.Value());
            var deDecimals = de.GetPreviousTimePeriods(20, ibm.Tail).Select(x => x.Value());

            var expected = ibmDecimals.Correlation(deDecimals);
            var actual = new Correlation(ibm, 20, de)[ibm.Tail];
            Assert.AreEqual(expected, actual);
        }
    }
}
