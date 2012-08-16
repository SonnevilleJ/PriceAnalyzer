using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities.Sonneville.PriceTools
{
    public static class GenericTestUtilities
    {
        /// <summary>
        /// Asserts that each property is identical for the two instances of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AssertSameState<T>(T expected, T actual)
        {
            var properties = expected.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.GetIndexParameters().Length != 0) continue;

                Assert.AreEqual(propertyInfo.GetValue(expected, null), propertyInfo.GetValue(actual, null));
            }
        }
    }
}