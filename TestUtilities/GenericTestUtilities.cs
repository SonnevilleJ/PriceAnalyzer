using System.Linq;
using NUnit.Framework;

namespace Sonneville.PriceTools.TestUtilities
{
    public static class GenericTestUtilities
    {
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