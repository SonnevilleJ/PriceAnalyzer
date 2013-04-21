using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Statistics.Test
{
    [TestClass]
    public class StatisticsTest
    {
        private readonly IEnumerable<decimal> _intelPrices = new List<decimal>
            {
                21.395m, 21.71m, 21.20m, 21.34m, 21.49m,
                21.39m, 22.16m, 22.53m, 22.44m, 22.75m,
                23.23m, 23.09m, 22.85m, 22.45m, 22.48m,
                22.27m, 22.37m, 22.28m, 23.06m, 22.99m,
            };

        private readonly IEnumerable<decimal> _qqqPrices = new List<decimal>
            {
                54.831m, 55.34m, 54.38m, 55.245m, 56.07m,
                56.30m, 57.05m, 57.91m, 58.20m, 58.39m,
                59.19m, 59.03m, 57.96m, 57.52m, 57.76m,
                57.09m, 57.85m, 57.54m, 58.85m, 58.60m,
            };
            
        [TestMethod]
        public void IntelVarianceTest()
        {
            const decimal expected = 0.3985021875m;
            var actual = _intelPrices.Variance();

            Assert.AreEqual(expected, actual);
        }
            
        [TestMethod]
        public void QqqVarianceTest()
        {
            const decimal expected = 1.96749621m;
            var actual = _qqqPrices.Variance();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IntelMultiplySequencesTest()
        {
            DoSequenceMultiplyTest(_intelPrices, _qqqPrices);
        }

        [TestMethod]
        public void QqqMultiplySequencesTest()
        {
            DoSequenceMultiplyTest(_qqqPrices, _intelPrices);
        }

        private void DoSequenceMultiplyTest(IEnumerable<decimal> one, IEnumerable<decimal> two)
        {
            var actual = one.MultiplyBy(two);
            var expected = new List<decimal>();
            for (var i = 0; i < one.Count(); i++)
            {
                expected.Add(one.ElementAt(i)*two.ElementAt(i));
            }

            for (var i = 0; i < one.Count(); i++)
            {
                Assert.AreEqual(one.ElementAt(i)*two.ElementAt(i), actual.ElementAt(i));
            }
        }

        [TestMethod]
        public void IntelCovarianceTest()
        {
            const decimal expected = 0.848423875m;
            var actual = _intelPrices.Covariance(_qqqPrices);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void QqqCovarianceTest()
        {
            const decimal expected = 0.848423875m;
            var actual = _qqqPrices.Covariance(_intelPrices);

            Assert.AreEqual(expected, actual);
        }
    }
}
