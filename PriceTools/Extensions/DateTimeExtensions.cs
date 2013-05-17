using System;
using System.Globalization;

namespace Sonneville.PriceTools
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
            return dateTime.SeekPeriods(1, resolution).CurrentPeriodOpen(resolution);
        }

        /// <summary>
        /// Gets the closing DateTime of the next <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution">The <see cref="Resolution"/> to use when determining <see cref="ITimePeriod"/> boundaries.</param>
        /// <returns></returns>
        public static DateTime NextPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekPeriods(1, resolution).CurrentPeriodClose(resolution);
        }

        /// <summary>
        /// Gets the opening DateTime of the previous <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution">The <see cref="Resolution"/> to use when determining <see cref="ITimePeriod"/> boundaries.</param>
        /// <returns></returns>
        public static DateTime PreviousPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekPeriods(-1, resolution).CurrentPeriodOpen(resolution);
        }

        /// <summary>
        /// Gets the closing DateTime of the previous <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution">The <see cref="Resolution"/> to use when determining <see cref="ITimePeriod"/> boundaries.</param>
        /// <returns></returns>
        public static DateTime PreviousPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekPeriods(-1, resolution).CurrentPeriodClose(resolution);
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
            throw new ArgumentOutOfRangeException("resolution", String.Format(CultureInfo.InvariantCulture, Strings.DateTimeExtensions_SeekPeriods_Unable_to_determine_boundaries_for_an_ITimePeriod_with_Resolution___0_, resolution));
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
            throw new ArgumentOutOfRangeException("resolution", String.Format(CultureInfo.InvariantCulture, Strings.DateTimeExtensions_SeekPeriods_Unable_to_determine_boundaries_for_an_ITimePeriod_with_Resolution___0_, resolution));
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
            return dateTime.SeekTradingPeriods(1, resolution).CurrentPeriodOpen(resolution);
        }

        /// <summary>
        /// Gets the closing DateTime of the next trading period <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static DateTime NextTradingPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekTradingPeriods(1, resolution).CurrentPeriodClose(resolution);
        }

        /// <summary>
        /// Gets the opening DateTime of the previous trading period <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static DateTime PreviousTradingPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekTradingPeriods(-1, resolution).CurrentPeriodOpen(resolution);
        }

        /// <summary>
        /// Gets the closing DateTime of the previous trading period <see cref="ITimePeriod"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static DateTime PreviousTradingPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekTradingPeriods(-1, resolution).CurrentPeriodClose(resolution);
        }

        /// <summary>
        /// Gets a point in time in another <see cref="ITimePeriod"/> a relative number of periods away.
        /// </summary>
        /// <param name="origin">The origin <see cref="DateTime"/>.</param>
        /// <param name="periods">The number of <see cref="ITimePeriod"/> to seek. Positive numbers seek forward in time; negative numbers seek backward in time.</param>
        /// <param name="resolution">The resolution of <see cref="ITimePeriod"/> to seek.</param>
        /// <returns>A point in time with the same relative distance from the relative <see cref="ITimePeriod"/>'s <see cref="CurrentPeriodOpen"/> as the origin.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the resolution is unknown and <see cref="ITimePeriod"/> boundaries cannot be determined.</exception>
        public static DateTime SeekPeriods(this DateTime origin, int periods, Resolution resolution)
        {
            if (resolution <= (Resolution) (4*(long) Resolution.Weeks))
            {
                return origin.AddTicks(periods*(long) resolution);
            }
            if (resolution <= Resolution.Months)
            {
                return origin.AddMonths(periods);
            }
            //if (resolution <= Resolution.Years)
            //{
            //    return origin.AddYears(1);
            //}
            throw new ArgumentOutOfRangeException("resolution", String.Format(CultureInfo.InvariantCulture, Strings.DateTimeExtensions_SeekPeriods_Unable_to_determine_boundaries_for_an_ITimePeriod_with_Resolution___0_, resolution));
        }

        /// <summary>
        /// Gets a point in time in another <see cref="ITimePeriod"/> a relative number of trading periods away.
        /// </summary>
        /// <param name="origin">The origin <see cref="DateTime"/>.</param>
        /// <param name="periods">The number of <see cref="ITimePeriod"/> to seek. Positive numbers seek forward in time; negative numbers seek backward in time.</param>
        /// <param name="resolution">The resolution of <see cref="ITimePeriod"/> to seek.</param>
        /// <returns>A point in time with the same relative distance from the relative <see cref="ITimePeriod"/>'s <see cref="CurrentPeriodOpen"/> as the origin.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the resolution is unknown and <see cref="ITimePeriod"/> boundaries cannot be determined.</exception>
        public static DateTime SeekTradingPeriods(this DateTime origin, int periods, Resolution resolution)
        {
            var temp = origin;
            var increment = periods < 0 ? -1 : 1;

            for (var i = 0; i < Math.Abs(periods); i++)
            {
                do
                {
                    temp = temp.SeekPeriods(increment, resolution);
                } while (!temp.IsInTradingPeriod(resolution));
            }
            return temp;
        }
    }
}
