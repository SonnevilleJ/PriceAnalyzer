using NUnit.Framework;

namespace Sonneville.Statistics.Test
{
    [TestFixture]
    public class CovarianceTest
    {
        [Test]
        public void IntelCovarianceTest()
        {
            const decimal expected = 0.848423875m;
            var actual = TestData.IntelPrices.Covariance(TestData.QqqPrices);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void QqqCovarianceTest()
        {
            const decimal expected = 0.848423875m;
            var actual = TestData.QqqPrices.Covariance(TestData.IntelPrices);

            Assert.AreEqual(expected, actual);
        }
    }
}