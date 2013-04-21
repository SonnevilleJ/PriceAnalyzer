using System.Collections.Generic;

namespace Statistics.Test
{
    public static class TestData
    {
        public static readonly IEnumerable<decimal> IntelPrices = new List<decimal>
            {
                21.395m, 21.71m, 21.20m, 21.34m, 21.49m,
                21.39m, 22.16m, 22.53m, 22.44m, 22.75m,
                23.23m, 23.09m, 22.85m, 22.45m, 22.48m,
                22.27m, 22.37m, 22.28m, 23.06m, 22.99m,
            };

        public static readonly IEnumerable<decimal> QqqPrices = new List<decimal>
            {
                54.831m, 55.34m, 54.38m, 55.245m, 56.07m,
                56.30m, 57.05m, 57.91m, 58.20m, 58.39m,
                59.19m, 59.03m, 57.96m, 57.52m, 57.76m,
                57.09m, 57.85m, 57.54m, 58.85m, 58.60m,
            };
    }
}