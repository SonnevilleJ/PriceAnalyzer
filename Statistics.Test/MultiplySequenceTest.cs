using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Sonneville.Statistics.Test
{
    [TestFixture]
    public class MultiplySequenceTest
    {
        [Test]
        public void IntelMultiplySequencesTest()
        {
            DoSequenceMultiplyTest(TestData.IntelPrices.ToArray(), TestData.QqqPrices.ToArray());
        }

        [Test]
        public void QqqMultiplySequencesTest()
        {
            DoSequenceMultiplyTest(TestData.QqqPrices.ToArray(), TestData.IntelPrices.ToArray());
        }

        private static void DoSequenceMultiplyTest(decimal[] one, decimal[] two)
        {
            DoSequenceOperationTest(one, two, (x, y) => x*y);
        }

        private static void DoSequenceOperationTest(decimal[] one, decimal[] two, Func<decimal, decimal, decimal> operation)
        {
            var expected = new List<decimal>();
            for (var i = 0; i < one.Count(); i++)
            {
                expected.Add(one.ElementAt(i)*two.ElementAt(i));
            }

            var actual = one.MultiplyBy(two).ToArray();
            for (var i = 0; i < one.Count(); i++)
            {
                Assert.AreEqual(operation(one.ElementAt(i), two.ElementAt(i)), actual.ElementAt(i));
            }
        }
    }
}