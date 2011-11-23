using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for ITimeSeries objects.
    /// </summary>
    public static class TimeSeriesExtensions
    {
        /// <summary>
        ///   Gets a <see cref = "System.TimeSpan" /> value indicating the length of time covered by the ITimeSeries.
        /// </summary>
        public static TimeSpan TimeSpan(this ITimeSeries timeSeries)
        {
            return timeSeries.Tail - timeSeries.Head;
        }
    }
}
