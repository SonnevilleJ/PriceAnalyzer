using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Statistics.Test
{
    [TestClass]
    public class StatisticsTest
    {
        [TestMethod]
        public void VarianceTest()
        {
            var decimals = new List<decimal>
                {
                    21.395m, 21.71m, 21.20m, 21.34m, 21.49m,
                    21.39m, 22.16m, 22.53m, 22.44m, 22.75m,
                    23.23m, 23.09m, 22.85m, 22.45m, 22.48m,
                    22.27m, 22.37m, 22.28m, 23.06m, 22.99m,
                };

            const decimal expected = 0.3985021875m;
            var actual = decimals.Variance();

            Assert.AreEqual(expected, actual);
        }
    }
}
