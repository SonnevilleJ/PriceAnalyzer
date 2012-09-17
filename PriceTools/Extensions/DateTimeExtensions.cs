using System;

namespace Sonneville.PriceTools.Extensions
{
    /// <summary>
    /// A class which holds extension methods for the DateTime class.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets the next opening DateTime. This method does not consider holidays.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution">The <see cref="Resolution"/> to use when determining <see cref="ITimePeriod"/> boundaries.</param>
        /// <returns></returns>
        public static DateTime NextPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.CurrentPeriodClose(resolution).AddTicks(1).CurrentPeriodOpen(resolution);
        }

        /// <summary>
        /// Gets the next closing DateTime. This method does not consider holidays.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution">The <see cref="Resolution"/> to use when determining <see cref="ITimePeriod"/> boundaries.</param>
        /// <returns></returns>
        public static DateTime NextPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.CurrentPeriodClose(resolution).AddTicks(1).CurrentPeriodClose(resolution);
        }

        /// <summary>
        /// Gets the DateTime of the most recent daily open.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution">The <see cref="Resolution"/> to use when determining <see cref="ITimePeriod"/> boundaries.</param>
        /// <returns></returns>
        public static DateTime PreviousPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.CurrentPeriodOpen(resolution).AddTicks(-1).CurrentPeriodOpen(resolution);
        }

        /// <summary>
        /// Gets the DateTime of the most recent daily close.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution">The <see cref="Resolution"/> to use when determining <see cref="ITimePeriod"/> boundaries.</param>
        /// <returns></returns>
        public static DateTime PreviousPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.CurrentPeriodOpen(resolution).AddTicks(-1).CurrentPeriodClose(resolution);
        }

        /// <summary>
        /// Adds a single period's duration to the given date, while accounting for non-trading days.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static DateTime AddPeriod (this DateTime date, Resolution resolution)
        {
            var ticks = (long) resolution;
            do
            {
                date = date.AddTicks(ticks);
            } while (!date.IsInTradingPeriod());
            return date;
        }

        #region Private Methods

        public static DateTime CurrentPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.CurrentPeriodOpen(resolution).AddTicks(-1 + (long) resolution);
        }

        public static DateTime CurrentPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            // TODO: handle other resolutions
            return dateTime.Date;
        }

        public static bool IsInTradingPeriod(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Monday ||
                   dateTime.DayOfWeek == DayOfWeek.Tuesday ||
                   dateTime.DayOfWeek == DayOfWeek.Wednesday ||
                   dateTime.DayOfWeek == DayOfWeek.Thursday ||
                   dateTime.DayOfWeek == DayOfWeek.Friday;
        }

        public static DateTime NextTradingPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            while (!dateTime.IsInTradingPeriod())
            {
                dateTime = dateTime.NextPeriodOpen(resolution);
            }
            return dateTime;
        }

        public static DateTime NextTradingPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            while (!dateTime.IsInTradingPeriod())
            {
                dateTime = dateTime.NextPeriodClose(resolution);
            }
            return dateTime;
        }

        public static DateTime PreviousTradingPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            while (!dateTime.IsInTradingPeriod())
            {
                dateTime = dateTime.PreviousPeriodOpen(resolution);
            }
            return dateTime;
        }

        public static DateTime PreviousTradingPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            while (!dateTime.IsInTradingPeriod())
            {
                dateTime = dateTime.PreviousPeriodClose(resolution);
            }
            return dateTime;
        }

        #endregion
    }
}
