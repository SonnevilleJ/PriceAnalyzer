using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for <see cref="ITimePeriod"/> objects.
    /// </summary>
    public static class TimePeriodExtensions
    {
        /// <summary>
        ///   Gets a <see cref = "System.TimeSpan" /> value indicating the length of time covered by the <see cref="ITimePeriod"/>.
        /// </summary>
        public static TimeSpan TimeSpan(this ITimePeriod timePeriod)
        {
            return timePeriod.Tail - timePeriod.Head;
        }

        /// <summary>
        /// Gets the value stored in the <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <returns></returns>
        public static decimal Value(this ITimePeriod timePeriod)
        {
            return timePeriod[timePeriod.Tail];
        }
    }
}
