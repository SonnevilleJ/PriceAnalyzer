﻿using System;

namespace Sonneville.PriceTools.Extensions
{
    /// <summary>
    /// A class which holds extension methods for the DateTime class.
    /// </summary>
    public static class DateTimeExtensions
    {
        private static readonly TimeSpan TimeFromOpenToClose = new TimeSpan(23, 59, 59);

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
            } while (!date.IsTradingDay());
            return date;
        }

        #region Private Methods

        private static DateTime TodaysClose(this DateTime dateTime)
        {
            return dateTime.TodaysOpen().Add(TimeFromOpenToClose);
        }

        private static DateTime TodaysOpen(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        private static bool IsTradingDay(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Monday ||
                   dateTime.DayOfWeek == DayOfWeek.Tuesday ||
                   dateTime.DayOfWeek == DayOfWeek.Wednesday ||
                   dateTime.DayOfWeek == DayOfWeek.Thursday ||
                   dateTime.DayOfWeek == DayOfWeek.Friday;
        }

        #endregion
    }
}
