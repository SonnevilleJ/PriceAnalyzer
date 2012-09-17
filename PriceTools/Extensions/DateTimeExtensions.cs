using System;

namespace Sonneville.PriceTools.Extensions
{
    /// <summary>
    /// A class which holds extension methods for the DateTime class.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets the opening DateTime of the next <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution">The <see cref="Resolution"/> to use when determining <see cref="ITimePeriod"/> boundaries.</param>
        /// <returns></returns>
        public static DateTime NextPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.CurrentPeriodClose(resolution).AddTicks(1).CurrentPeriodOpen(resolution);
        }

        /// <summary>
        /// Gets the closing DateTime of the next <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution">The <see cref="Resolution"/> to use when determining <see cref="ITimePeriod"/> boundaries.</param>
        /// <returns></returns>
        public static DateTime NextPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.CurrentPeriodClose(resolution).AddTicks(1).CurrentPeriodClose(resolution);
        }

        /// <summary>
        /// Gets the opening DateTime of the previous <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution">The <see cref="Resolution"/> to use when determining <see cref="ITimePeriod"/> boundaries.</param>
        /// <returns></returns>
        public static DateTime PreviousPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.CurrentPeriodOpen(resolution).AddTicks(-1).CurrentPeriodOpen(resolution);
        }

        /// <summary>
        /// Gets the closing DateTime of the previous <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution">The <see cref="Resolution"/> to use when determining <see cref="ITimePeriod"/> boundaries.</param>
        /// <returns></returns>
        public static DateTime PreviousPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.CurrentPeriodOpen(resolution).AddTicks(-1).CurrentPeriodClose(resolution);
        }

        /// <summary>
        /// Gets the closing DateTime of the current <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static DateTime CurrentPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.CurrentPeriodOpen(resolution).AddTicks(-1 + (long) resolution);
        }

        /// <summary>
        /// Gets the opening DateTime of the current <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static DateTime CurrentPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            // TODO: handle other resolutions
            return dateTime.Date;
        }

        /// <summary>
        /// Gets a value indicating if the DateTime is within market trading hours.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsInTradingPeriod(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Monday ||
                   dateTime.DayOfWeek == DayOfWeek.Tuesday ||
                   dateTime.DayOfWeek == DayOfWeek.Wednesday ||
                   dateTime.DayOfWeek == DayOfWeek.Thursday ||
                   dateTime.DayOfWeek == DayOfWeek.Friday;
        }

        /// <summary>
        /// Gets the opening DateTime of the next trading period <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static DateTime NextTradingPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            while (!dateTime.IsInTradingPeriod())
            {
                dateTime = dateTime.NextPeriodOpen(resolution);
            }
            return dateTime;
        }

        /// <summary>
        /// Gets the closing DateTime of the next trading period <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static DateTime NextTradingPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            while (!dateTime.IsInTradingPeriod())
            {
                dateTime = dateTime.NextPeriodClose(resolution);
            }
            return dateTime;
        }

        /// <summary>
        /// Gets the opening DateTime of the previous trading period <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static DateTime PreviousTradingPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            while (!dateTime.IsInTradingPeriod())
            {
                dateTime = dateTime.PreviousPeriodOpen(resolution);
            }
            return dateTime;
        }

        /// <summary>
        /// Gets the closing DateTime of the previous trading period <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static DateTime PreviousTradingPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            while (!dateTime.IsInTradingPeriod())
            {
                dateTime = dateTime.PreviousPeriodClose(resolution);
            }
            return dateTime;
        }
    }
}
