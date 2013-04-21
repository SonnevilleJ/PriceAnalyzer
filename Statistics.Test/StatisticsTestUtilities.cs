using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Statistics.Test
{
    public static class StatisticsTestUtilities
    {
        private const decimal DecimalError = 0.000000000001m;

        public static void AssertAreEqual(decimal expected, decimal actual, decimal epsilon = DecimalError)
        {
            var diff = Math.Abs(actual - expected);
            Assert.IsTrue(diff <= epsilon, string.Format("Actual was off by {0}", diff));
        }
    }
}