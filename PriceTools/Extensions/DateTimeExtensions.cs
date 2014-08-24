using System;
using System.Globalization;

namespace Sonneville.PriceTools
{
    public static class DateTimeExtensions
    {
        public static DateTime NextPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekPeriods(1, resolution).CurrentPeriodOpen(resolution);
        }

        public static DateTime NextPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekPeriods(1, resolution).CurrentPeriodClose(resolution);
        }

        public static DateTime PreviousPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekPeriods(-1, resolution).CurrentPeriodOpen(resolution);
        }

        public static DateTime PreviousPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekPeriods(-1, resolution).CurrentPeriodClose(resolution);
        }

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

        public static DateTime NextTradingPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekTradingPeriods(1, resolution).CurrentPeriodOpen(resolution);
        }

        public static DateTime NextTradingPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekTradingPeriods(1, resolution).CurrentPeriodClose(resolution);
        }

        public static DateTime PreviousTradingPeriodOpen(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekTradingPeriods(-1, resolution).CurrentPeriodOpen(resolution);
        }

        public static DateTime PreviousTradingPeriodClose(this DateTime dateTime, Resolution resolution)
        {
            return dateTime.SeekTradingPeriods(-1, resolution).CurrentPeriodClose(resolution);
        }

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
