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
            if (resolution < ((Resolution)(4 * (long)Resolution.Weeks)))
            {
                return dateTime.CurrentPeriodOpen(resolution).AddTicks(-1 + (long)resolution);
            }
            if (resolution == Resolution.Months)
            {
                var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
                return firstDayOfMonth.AddMonths(1).AddDays(-1).CurrentPeriodClose(Resolution.Days);
            }
            throw new ArgumentOutOfRangeException("resolution", String.Format("Unable to determine boundaries for an ITimePeriod with Resolution: {0}", resolution));
        }

        /// <summary>
        /// Gets the opening DateTime of the current <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static DateTime CurrentPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            if (resolution < Resolution.Weeks)
            {
                var ticksFromNearestBarrier = dateTime.Ticks % (long) resolution;
                return dateTime.AddTicks(0 - ticksFromNearestBarrier);
            }
            if (resolution < (Resolution)(4 * (long)Resolution.Weeks))
            {
                var local = dateTime;
                while (local.DayOfWeek != DayOfWeek.Sunday)
                {
                    local = local.PreviousPeriodOpen(Resolution.Days);
                }
                return local.CurrentPeriodOpen(Resolution.Days);
            }
            if (resolution == Resolution.Months)
            {
                return new DateTime(dateTime.Year, dateTime.Month, 1);
            }
            //if (resolution == resolution.Years)
            //{
            //}
            throw new ArgumentOutOfRangeException("resolution", String.Format("Unable to determine boundaries for an ITimePeriod with Resolution: {0}", resolution));
        }

        /// <summary>
        /// Gets a value indicating if the DateTime is within market trading hours.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static bool IsInTradingPeriod(this DateTime dateTime, Resolution resolution)
        {
            // Weeks, Months, Years, etc do not have "trading periods"
            if (resolution >= Resolution.Weeks) return true;
            
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
            do
            {
                dateTime = dateTime.NextPeriodOpen(resolution);
            } while (!dateTime.IsInTradingPeriod(resolution));
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
            do
            {
                dateTime = dateTime.NextPeriodClose(resolution);
            } while (!dateTime.IsInTradingPeriod(resolution));
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
            do
            {
                dateTime = dateTime.PreviousPeriodOpen(resolution);
            } while (!dateTime.IsInTradingPeriod(resolution));
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
            do
            {
                dateTime = dateTime.PreviousPeriodClose(resolution);
            } while (!dateTime.IsInTradingPeriod(resolution));
            return dateTime;
        }

        public static DateTime SeekPeriods(this DateTime dateTime, int periods)
        {
            throw new NotImplementedException();
        }

        public static DateTime SeekPeriods(this DateTime dateTime, int periods, Resolution resolution)
        {
            throw new NotImplementedException();
        }
    }
}
