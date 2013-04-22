using Microsoft.VisualStudio.TestTools.UnitTesting;
using Statistics;

namespace Test.Statistics
{
    [TestClass]
    public class CovarianceTest
    {
        [TestMethod]
        public void IntelCovarianceTest()
        {
            const decimal expected = 0.848423875m;
            var actual = TestData.IntelPrices.Covariance(TestData.QqqPrices);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void QqqCovarianceTest()
        {
            const decimal expected = 0.848423875m;
            var actual = TestData.QqqPrices.Covariance(TestData.IntelPrices);

            Assert.AreEqual(expected, actual);
        }
    }
}