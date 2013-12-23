using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.Statistics.Test
{
    [TestClass]
    public class VarianceTest
    {
        [TestMethod]
        public void IntelVarianceTest()
        {
            const decimal expected = 0.3985021875m;
            var actual = TestData.IntelPrices.Variance();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void QqqVarianceTest()
        {
            const decimal expected = 1.96749621m;
            var actual = TestData.QqqPrices.Variance();

            Assert.AreEqual(expected, actual);
        }
    }
}