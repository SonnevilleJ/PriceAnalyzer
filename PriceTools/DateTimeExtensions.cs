﻿using System;

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
        public static DateTime GetFollowingWeeklyOpen(this DateTime dateTime)
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
            return GetMostRecentOpen(date);
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
            return GetFollowingClose(date);
        }

        public static DateTime GetFollowingMonthlyOpen(this DateTime date)
        {
            var next = date.AddMonths(1);
            var firstDayOfMonth = new DateTime(next.Year, next.Month, 1);
            return firstDayOfMonth.GetCurrentOrFollowingTradingDay();
        }

        public static DateTime GetFollowingMonthlyClose(this DateTime date)
        {
            var next = date.AddMonths(1);
            var firstDayOfMonth = new DateTime(next.Year, next.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            return lastDayOfMonth.GetCurrentOrFollowingTradingDay().GetFollowingClose();
        }

        public static DateTime GetMostRecentMonthlyOpen(this DateTime date)
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            return firstDayOfMonth.GetCurrentOrFollowingTradingDay();
        }
    }
}
