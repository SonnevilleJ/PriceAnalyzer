using NUnit.Framework;

namespace Sonneville.Statistics.Test
{
    [TestFixture]
    public class SquareRootTest
    {
        [Test]
        public void SquareRoot4Test()
        {
            const decimal expected = 2m;
            var actual = 4m.SquareRoot();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SquareRoot9Test()
        {
            const decimal expected = 3m;
            var actual = 9m.SquareRoot();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SquareRootIntelVarianceTest()
        {
            const decimal expected = 0.631270296703367m;
            var actual = TestData.IntelPrices.Variance().SquareRoot();

            StatisticsTestUtilities.AssertAreEqual(expected, actual);
        }

        [Test]
        public void SquareRootQqqVarianceTest()
        {
            const decimal expected = 1.402674662921060m;
            var actual = TestData.QqqPrices.Variance().SquareRoot();

            StatisticsTestUtilities.AssertAreEqual(expected, actual);
        }
    }
}