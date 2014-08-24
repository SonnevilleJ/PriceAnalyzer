using System;

namespace Sonneville.PriceTools
{
    public enum Resolution : long
    {
        Seconds = TimeSpan.TicksPerSecond,

        Minutes = TimeSpan.TicksPerMinute,

        TwoMinutes = TimeSpan.TicksPerMinute*2,

        FiveMinutes = TimeSpan.TicksPerMinute*5,

        TwentyMinutes = TimeSpan.TicksPerMinute*20,

        Hours = TimeSpan.TicksPerHour,

        Days = TimeSpan.TicksPerDay,

        Weeks = TimeSpan.TicksPerDay*7,

        Months = TimeSpan.TicksPerDay*31
    }
}