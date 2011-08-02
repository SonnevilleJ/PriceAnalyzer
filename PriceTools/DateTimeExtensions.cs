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
            do
            {
                dateTime = dateTime.AddDays(1);
            } while (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday);
            return dateTime.Date;
        }

        /// <summary>
        /// Gets the next closing DateTime. This method does not consider holidays.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetNextClose(this DateTime dateTime)
        {
            return dateTime.AddSeconds(1).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
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
