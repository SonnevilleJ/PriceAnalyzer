using System;

namespace Sonneville.PriceTools
{
    public static class MathExtensions
    {
        /// <summary>
        /// Returns the square root of a specified number.
        /// </summary>
        /// <param name="d">A number.</param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static decimal SquareRoot(this decimal d, decimal epsilon = 0.0M)
        {
            if (d < 0) throw new OverflowException("Cannot calculate square root from a negative number");

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