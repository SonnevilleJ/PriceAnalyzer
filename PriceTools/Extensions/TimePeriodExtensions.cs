using System;

namespace Sonneville.PriceTools
{
    public static class TimePeriodExtensions
    {
        /// <summary>
        /// Determines if the ITimePeriod has a valid value for a given date.
        /// </summary>
        /// <param name="timePeriod">The <see cref="ITimePeriod"/> to inspect.</param>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimePeriod has a valid value for the given date.</returns>
        public static bool HasValueInRange<T>(this ITimePeriod<T> timePeriod, DateTime settlementDate)
        {
            return timePeriod.Head <= settlementDate && timePeriod.Tail >= settlementDate;
        }
    }
}