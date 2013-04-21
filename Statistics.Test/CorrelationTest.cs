using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Statistics.Test
{
    [TestClass]
    public class CorrelationTest
    {
        [TestMethod]
        public void IntelCorrelationTest()
        {
            const decimal expected = 0.95816559861761m;
            var actual = TestData.IntelPrices.Correlation(TestData.QqqPrices);

            StatisticsTestUtilities.AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void QqqCorrelationTest()
        {
            const decimal expected = 0.95816559861761m;
            var actual = TestData.QqqPrices.Correlation(TestData.IntelPrices);

            StatisticsTestUtilities.AssertAreEqual(expected, actual);
        }
    }
}