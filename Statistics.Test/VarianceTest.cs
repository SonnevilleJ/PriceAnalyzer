using NUnit.Framework;

namespace Sonneville.Statistics.Test
{
    [TestFixture]
    public class VarianceTest
    {
        [Test]
        public void IntelVarianceTest()
        {
            const decimal expected = 0.3985021875m;
            var actual = TestData.IntelPrices.Variance();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void QqqVarianceTest()
        {
            const decimal expected = 1.96749621m;
            var actual = TestData.QqqPrices.Variance();

            Assert.AreEqual(expected, actual);
        }
    }
}