using System.Linq;
using NUnit.Framework;

namespace Sonneville.PriceTools.TestUtilities
{
    public static class GenericTestUtilities
    {
        /// <summary>
        /// Asserts that each property is identical for the two instances of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AssertSameReflectedProperties<T>(T expected, T actual)
        {
            var properties = expected.GetType().GetProperties();
            foreach (var propertyInfo in properties.Where(propertyInfo => !propertyInfo.GetIndexParameters().Any()))
            {
                Assert.AreEqual(propertyInfo.GetValue(expected, null), propertyInfo.GetValue(actual, null));
            }
        }
    }
}