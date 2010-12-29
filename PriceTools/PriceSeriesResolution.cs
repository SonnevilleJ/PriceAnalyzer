using System;


namespace Sonneville.PriceTools
{
    /// <summary>
    /// Specifies the resolution of an IPriceSeries.
    /// </summary>
    public enum PriceSeriesResolution : long
    {
        /// <summary>
        /// Data is available for each second.
        /// </summary>
        Seconds = TimeSpan.TicksPerSecond,

        /// <summary>
        /// Data is available for each minute.
        /// </summary>
        Minutes = TimeSpan.TicksPerMinute,

        /// <summary>
        /// Data is available for every two minutes.
        /// </summary>
        TwoMinutes = TimeSpan.TicksPerMinute * 2,

        /// <summary>
        /// Data is available for every five minutes.
        /// </summary>
        FiveMinutes = TimeSpan.TicksPerMinute * 5,

        /// <summary>
        /// Data is available for every twenty minutes.
        /// </summary>
        TwentyMinutes = TimeSpan.TicksPerMinute * 20,

        /// <summary>
        /// Data is available for each hour.
        /// </summary>
        Hours = TimeSpan.TicksPerHour,

        /// <summary>
        /// Data is available for each day.
        /// </summary>
        Days = TimeSpan.TicksPerDay,

        /// <summary>
        /// Data is available for each week.
        /// </summary>
        Weeks = TimeSpan.TicksPerDay * 7,

        /// <summary>
        /// Data is available for each month.
        /// </summary>
        Months = TimeSpan.TicksPerDay * 30
    }
}
