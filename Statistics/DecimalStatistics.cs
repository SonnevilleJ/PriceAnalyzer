﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace Sonneville.Statistics
{
    public static class DecimalStatistics
    {
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
            return one.MultiplyBy(two).Average() - (one.Average()*two.Average());
        }

        private static decimal Multiply(decimal x, decimal y)
        {
            return x * y;
        }

        public static IEnumerable<decimal> MultiplyBy(this IEnumerable<decimal> one, IEnumerable<decimal> two)
        {
            return one.Zip(two, Multiply);
        }

        public static decimal Correlation(this IEnumerable<decimal> one, IEnumerable<decimal> two)
        {
            var numerator = Covariance(one, two);
            var v1 = one.Variance();
            var v2 = two.Variance();
            var denominator = SquareRoot(v1*v2);
            return numerator/denominator;
        }

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

        public static decimal SquareRoot(this int i)
        {
            return ((decimal)i).SquareRoot();
        }

        public static decimal SquareRoot(this long l)
        {
            return ((decimal)l).SquareRoot();
        }

        public static decimal StudentTScore(this IEnumerable<decimal> decimals)
        {
            var size = decimals.Count();
            var mean = decimals.Average();
            var standardDeviation = decimals.StandardDeviation();
            return (mean - 0)/(standardDeviation/size.SquareRoot());
        }

        public static decimal StudentTDistribution(this IEnumerable<decimal> decimals)
        {
            var tScore = decimals.StudentTScore();
            var degreesOfFreedom = decimals.Count() - 1;
            return StudentTDistribution(tScore, degreesOfFreedom);
        }

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
