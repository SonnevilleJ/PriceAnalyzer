using System;

namespace Sonneville.PriceTools
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
        /// <returns></returns>
        public static DateTime GetFollowingOpen(this DateTime dateTime)
        {
            return GetNextTradingDay(dateTime).GetMostRecentOpen();
        }

        /// <summary>
        /// Gets the next closing DateTime. This method does not consider holidays.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetFollowingClose(this DateTime dateTime)
        {
            return GetCurrentOrFollowingTradingDay(dateTime.AddSeconds(1)).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        /// <summary>
        /// Gets the opening DateTime of the following Monday. This method does not consider holidays.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetFollowingWeekOpen(this DateTime dateTime)
        {
            do
            {
                dateTime = dateTime.AddDays(1);
            } while (dateTime.DayOfWeek != DayOfWeek.Monday);
            return dateTime.GetMostRecentOpen();
        }

        /// <summary>
        /// Gets the closing DateTime of the following Friday. This method does not consider holidays.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetFollowingWeekClose(this DateTime dateTime)
        {
            while (dateTime.DayOfWeek != DayOfWeek.Friday)
            {
                dateTime = dateTime.AddDays(1);
            }
            return dateTime.GetFollowingClose();
        }

        /// <summary>
        /// Returns the nearest trading day. If the given day is not a trading day, the date is advanced to the next trading day.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetCurrentOrFollowingTradingDay(this DateTime dateTime)
        {
            while (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                dateTime = dateTime.AddDays(1);
            }
            return dateTime;
        }

        #region Private Methods

        private static DateTime GetNextTradingDay(DateTime dateTime)
        {
            return GetCurrentOrFollowingTradingDay(dateTime.AddDays(1));
        }

        #endregion

        /// <summary>
        /// Gets the beginning of the given trading day.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetMostRecentOpen(this DateTime date)
        {
            return date.Date;
        }

        public static DateTime GetBeginningOfTradingWeek(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    while (date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        date = date.AddDays(-1);
                    }
                    break;
                default:
                    while (date.DayOfWeek != DayOfWeek.Monday)
                    {
                        date = date.AddDays(-1);
                    }
                    break;
            }
            return GetMostRecentOpen(date);
        }

        public static DateTime GetFollowingWeeklyClose(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    while (date.DayOfWeek != DayOfWeek.Saturday)
                    {
                        date = date.AddDays(1);
                    }
                    break;
                default:
                    while (date.DayOfWeek != DayOfWeek.Friday)
                    {
                        date = date.AddDays(1);
                    }
                    break;
            }
            return GetFollowingClose(date);
        }
    }
}
