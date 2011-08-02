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
        public static DateTime GetNextOpen(this DateTime dateTime)
        {
            return GetNextWeekday(dateTime).Date;
        }

        private static DateTime GetNextWeekday(DateTime dateTime)
        {
            return EnsureWeekday(dateTime.AddDays(1));
        }

        private static DateTime EnsureWeekday(DateTime dateTime)
        {
            while (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                dateTime = dateTime.AddDays(1);
            }
            return dateTime;
        }

        /// <summary>
        /// Gets the next closing DateTime. This method does not consider holidays.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetNextClose(this DateTime dateTime)
        {
            var date = dateTime.AddSeconds(1).Date;
            date = EnsureWeekday(date);
            return date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        /// <summary>
        /// Gets the opening DateTime of the following Monday. This method does not consider holidays.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetNextMondayOpen(this DateTime dateTime)
        {
            do
            {
                dateTime = dateTime.AddDays(1);
            } while (dateTime.DayOfWeek != DayOfWeek.Monday);
            return dateTime.Date;
        }

        /// <summary>
        /// Gets the closing DateTime of the following Friday. This method does not consider holidays.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetNextFridayClose(this DateTime dateTime)
        {
            while (dateTime.DayOfWeek != DayOfWeek.Friday)
            {
                dateTime = dateTime.AddDays(1);
            }
            return dateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }
    }
}
