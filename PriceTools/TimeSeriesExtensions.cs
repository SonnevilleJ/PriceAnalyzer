using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for TimeSeries objects.
    /// </summary>
    public static class TimeSeriesExtensions
    {
        /// <summary>
        ///   Gets a <see cref = "System.TimeSpan" /> value indicating the length of time covered by the TimeSeries.
        /// </summary>
        public static TimeSpan TimeSpan(this TimeSeries timeSeries)
        {
            return timeSeries.Tail - timeSeries.Head;
        }
    }
}
