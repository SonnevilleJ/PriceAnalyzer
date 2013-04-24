using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace Statistics
{
    /// <summary>
    /// Calculates statistics for <see cref="decimal"/> numbers.
    /// </summary>
    public static class DecimalStatistics
    {
        /// <summary>
        /// Returns the median value of a series.
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal Median(this IEnumerable<decimal> decimals)
        {
            var list = decimals.OrderBy(d => d);
            if (!list.Any()) return 0m;

            var midpoint = list.Count() / 2;
            if (list.Count() % 2 == 0)
            {
                return (list.ElementAt(midpoint - 1) + list.ElementAt(midpoint)) / 2;
            }
            return list.ElementAt(midpoint);
        }

        public static decimal Variance(this IEnumerable<decimal> decimals)
        {
            var array = decimals.ToArray();
            var average = array.Average();
            var squares = array.Select(x => x*x);
            var squaresAverage = squares.Average();
            return squaresAverage - average*average;
        }

        public static decimal Covariance(this IEnumerable<decimal> one, IEnumerable<decimal> two)
        {
            var d1 = one.ToArray();
            var d2 = two.ToArray();
            var multiples = MultiplyBy(d1, d2);
            return multiples.Average() - (d1.Average()*d2.Average());
        }

        public static IEnumerable<decimal> MultiplyBy(this IEnumerable<decimal> one, IEnumerable<decimal> two)
        {
            //return Enumerable.Range(0, two.Count()).Select(x => two.Skip(x).Select(y => one.ElementAt(x)*y));
            //var multiples = new List<decimal>();
            for (var i = 0; i < one.Count(); i++)
            {
                yield return one.ElementAt(i) * two.ElementAt(i);
            }
            //return multiples;
        }

        public static decimal Correlation(this IEnumerable<decimal> one, IEnumerable<decimal> two)
        {
            var numerator = Covariance(one, two);
            var v1 = one.Variance();
            var v2 = two.Variance();
            var denominator = SquareRoot(v1*v2);
            return numerator/denominator;
        }

        /// <summary>
        /// Returns the standard deviation of a series.
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal StandardDeviation(this IEnumerable<decimal> decimals)
        {
            if (decimals.Count() <= 1) return 0;

            var parallel = decimals.AsParallel();
            var mean = parallel.Average();
            var squares = parallel.Select(holding => holding - mean).Select(deviation => deviation*deviation);
            var sum = squares.Sum();
            if (sum == 0m)
            {
                return 0m;
            }
            return ((sum / parallel.Count()) - 1).SquareRoot();
        }

        public static decimal SquareRoot(this decimal x, decimal? guess = null)
        {
            if (x < 0)
            {
                throw new OverflowException("Cannot calculate square root from a negative number");
            }

            var ourGuess = guess.GetValueOrDefault(x / 2m);
            var result = x / ourGuess;
            var average = (ourGuess + result) / 2m;

            if (average == ourGuess) // This checks for the maximum precision possible with a decimal.
                return average;
            else
                return SquareRoot(x, average);
        }

        ///// <summary>
        ///// Returns the square root of a specified number.
        ///// </summary>
        ///// <param name="d">A <see cref="decimal"/> number.</param>
        ///// <param name="epsilon">The tolerance of the function. Must be greater than or equal to zero.</param>
        ///// <returns></returns>
        //public static decimal SquareRoot(this decimal d, decimal epsilon = 0M)
        //{
        //    if (d < 0)
        //    {
        //        // throw new OverflowException("Cannot calculate square root from a negative number");
        //        return 0;   // imaginary number
        //    }
        //    if (epsilon < 0m) throw new ArgumentOutOfRangeException("epsilon", epsilon, "Epsilon must be greater than or equal to zero.");

        //    decimal current = (decimal)Math.Sqrt((double)d), previous;
        //    do
        //    {
        //        previous = current;
        //        if (previous == 0.0M) return 0;
        //        current = (previous + d / previous) / 2;
        //    }
        //    while (Math.Abs(previous - current) > epsilon);
        //    return current;
        //}

        /// <summary>
        /// Returns the square root of a specified number.
        /// </summary>
        /// <param name="i">A <see cref="int"/> number.</param>
        /// <returns></returns>
        public static decimal SquareRoot(this int i)
        {
            return ((decimal)i).SquareRoot();
        }

        /// <summary>
        /// Returns the square root of a specified number.
        /// </summary>
        /// <param name="l">A <see cref="long"/> number.</param>
        /// <returns></returns>
        public static decimal SquareRoot(this long l)
        {
            return ((decimal)l).SquareRoot();
        }

        /// <summary>
        /// Returns the Student T-score of a set of values.
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal StudentTScore(this IEnumerable<decimal> decimals)
        {
            var size = decimals.Count();
            var mean = decimals.Average();
            var standardDeviation = decimals.StandardDeviation();
            return (mean - 0)/(standardDeviation/size.SquareRoot());
        }

        /// <summary>
        /// Returns the Student T-distribution of a set of values.
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal StudentTDistribution(this IEnumerable<decimal> decimals)
        {
            var tScore = decimals.StudentTScore();
            var degreesOfFreedom = decimals.Count() - 1;
            return StudentTDistribution(tScore, degreesOfFreedom);
        }

        /// <summary>
        /// Returns the T-distribution of a set of values.
        /// </summary>
        /// <param name="tScore"></param>
        /// <param name="degreesOfFreedom"></param>
        /// <returns></returns>
        public static decimal StudentTDistribution(decimal tScore, int degreesOfFreedom)
        {
            if(degreesOfFreedom <= 0) throw new ArgumentOutOfRangeException("degreesOfFreedom", degreesOfFreedom, "Degrees of freedom must be positive.");

            using (var chart = new Chart())
            {
                return (decimal)chart.DataManipulator.Statistics.TDistribution((double)tScore, degreesOfFreedom, true);
            }
        }
    }
}
