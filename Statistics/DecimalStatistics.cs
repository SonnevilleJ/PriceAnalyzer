using System;
using System.Collections.Generic;
using System.Linq;

namespace Statistics
{
    /// <summary>
    /// Calculates statistics for <see cref="decimal"/> numbers.
    /// </summary>
    public static class DecimalStatistics
    {
        public static decimal Median(this IEnumerable<decimal> decimals)
        {
            var list = decimals.OrderBy(d => d);
            if (list.Count() == 0) return 0m;

            var midpoint = list.Count() / 2;
            if (list.Count() % 2 == 0)
            {
                return (list.ElementAt(midpoint - 1) + list.ElementAt(midpoint)) / 2;
            }
            return list.ElementAt(midpoint);
        }

        public static decimal StandardDeviation(this IEnumerable<decimal> decimals)
        {
            if (decimals.Count() <= 1) return 0;

            var parallel = decimals.AsParallel();
            var mean = parallel.Average();
            var squares = parallel.Select(holding => holding - mean).Select(deviation => deviation*deviation);
            var sum = squares.Sum();
            return ((sum / parallel.Count()) - 1).SquareRoot();
        }

        /// <summary>
        /// Returns the square root of a specified number.
        /// </summary>
        /// <param name="d">A number.</param>
        /// <param name="epsilon">The tolerance of the function. Must be greater than or equal to zero.</param>
        /// <returns></returns>
        public static decimal SquareRoot(this decimal d, decimal epsilon = 0.0M)
        {
            if (d < 0) throw new OverflowException("Cannot calculate square root from a negative number");
            if (epsilon < 0m) throw new ArgumentOutOfRangeException("epsilon", epsilon, "Epsilon must be greater than or equal to zero.");

            decimal current = (decimal)Math.Sqrt((double)d), previous;
            do
            {
                previous = current;
                if (previous == 0.0M) return 0;
                current = (previous + d / previous) / 2;
            }
            while (Math.Abs(previous - current) > epsilon);
            return current;
        }
    }
}
