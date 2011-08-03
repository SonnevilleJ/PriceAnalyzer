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
            return GetNextWeekday(dateTime).GetBeginningOfDay();
        }

        /// <summary>
        /// Gets the next closing DateTime. This method does not consider holidays.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetFollowingClose(this DateTime dateTime)
        {
            return EnsureWeekday(dateTime.AddSeconds(1)).GetEndOfDay();
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
            return dateTime.GetBeginningOfDay();
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
            return dateTime.GetEndOfDay();
        }

        public static DateTime EnsureWeekday(DateTime dateTime)
        {
            while (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                dateTime = dateTime.AddDays(1);
            }
            return dateTime;
        }

        #region Private Methods

        private static DateTime GetNextWeekday(DateTime dateTime)
        {
            return EnsureWeekday(dateTime.AddDays(1));
        }

        #endregion

        public static DateTime GetBeginningOfDay(this DateTime date)
        {
            return date.Date;
        }

        public static DateTime GetBeginningOfWeek(this DateTime date)
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
            return GetBeginningOfDay(date);
        }

        public static DateTime GetEndOfDay(this DateTime date)
        {
            return date.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public static DateTime GetEndOfWeek(this DateTime date)
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
            return GetEndOfDay(date);
        }
    }
}
