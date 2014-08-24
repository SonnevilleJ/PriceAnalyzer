using NUnit.Framework;

namespace Sonneville.Statistics.Test
{
    [TestFixture]
    public class CorrelationTest
    {
        [Test]
        public void IntelCorrelationTest()
        {
            const decimal expected = 0.95816559861761m;
            var actual = TestData.IntelPrices.Correlation(TestData.QqqPrices);

            StatisticsTestUtilities.AssertAreEqual(expected, actual);
        }

        [Test]
        public void QqqCorrelationTest()
        {
            const decimal expected = 0.95816559861761m;
            var actual = TestData.QqqPrices.Correlation(TestData.IntelPrices);

            StatisticsTestUtilities.AssertAreEqual(expected, actual);
        }
    }
}