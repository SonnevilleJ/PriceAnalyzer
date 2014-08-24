using System;

namespace Sonneville.PriceTools
{
    public static class TimePeriodExtensions
    {
        public static bool HasValueInRange<T>(this ITimePeriod<T> timePeriod, DateTime settlementDate)
        {
            return timePeriod.Head <= settlementDate && timePeriod.Tail >= settlementDate;
        }
    }
}