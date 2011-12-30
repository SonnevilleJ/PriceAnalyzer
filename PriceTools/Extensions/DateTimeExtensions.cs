﻿using System;

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
        /// <returns></returns>
        public static DateTime GetFollowingOpen(this DateTime dateTime)
        {
            do
            {
                dateTime = dateTime.AddDays(1);
            } while (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday);
            return dateTime.TodaysOpen();
        }

        /// <summary>
        /// Gets the next closing DateTime. This method does not consider holidays.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetFollowingClose(this DateTime dateTime)
        {
            dateTime = dateTime.AddSeconds(1);
            while (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                dateTime = dateTime.AddDays(1);
            }
            return dateTime.TodaysClose();
        }

        /// <summary>
        /// Gets the opening DateTime of the following Monday. This method does not consider holidays.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetFollowingWeeklyOpen(this DateTime dateTime)
        {
            do
            {
                dateTime = dateTime.AddDays(1);
            } while (dateTime.DayOfWeek != DayOfWeek.Monday);
            return dateTime.TodaysOpen();
        }

        /// <summary>
        /// Returns the nearest daily open. If the given day is not a trading day, the date is advanced to the next trading day.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetCurrentOrFollowingOpen(this DateTime dateTime)
        {
            while (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                dateTime = dateTime.AddDays(1);
            }
            return dateTime.GetMostRecentOpen();
        }

        #region Private Methods

        private static DateTime TodaysClose(this DateTime dateTime)
        {
            return dateTime.Date.Add(new TimeSpan(23, 59, 59));
        }

        private static DateTime TodaysOpen(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        #endregion

        /// <summary>
        /// Gets the DateTime of the most recent daily open.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetMostRecentOpen(this DateTime date)
        {
            while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(-1);
            }
            return date.TodaysOpen();
        }

        /// <summary>
        /// Gets the DateTime of the most recent daily close.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetMostRecentClose(this DateTime date)
        {
            date = date.AddDays(-1);
            while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(-1);
            }
            return date.TodaysClose();
        }

        /// <summary>
        /// Gets the most recent open of a trading week.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetMostRecentWeeklyOpen(this DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }
            return date.TodaysOpen();
        }

        /// <summary>
        /// Gets the most recent close of a trading week.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetMostRecentWeeklyClose(this DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Friday)
            {
                date = date.AddDays(-1);
            }
            return date.TodaysClose();
        }

        /// <summary>
        /// Gets the following close of a trading week.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetFollowingWeeklyClose(this DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Friday)
            {
                date = date.AddDays(1);
            }
            return date.TodaysClose();
        }

        /// <summary>
        /// Gets the following open of a trading month.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetFollowingMonthlyOpen(this DateTime date)
        {
            var next = date.AddMonths(1);
            var firstDayOfMonth = new DateTime(next.Year, next.Month, 1);
            return firstDayOfMonth.TodaysOpen();
        }

        /// <summary>
        /// Gets the following close of a trading month.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetFollowingMonthlyClose(this DateTime date)
        {
            var next = date.AddMonths(1);
            var firstDayOfMonth = new DateTime(next.Year, next.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            return lastDayOfMonth.TodaysClose();
        }

        /// <summary>
        /// Gets the most recent open of the trading month.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetMostRecentMonthlyOpen(this DateTime date)
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            return firstDayOfMonth.TodaysOpen();
        }
    }
}
