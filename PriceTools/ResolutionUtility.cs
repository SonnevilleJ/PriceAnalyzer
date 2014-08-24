using System;
using System.Globalization;
using System.Linq;

namespace Sonneville.PriceTools
{
    public class ResolutionUtility
    {
        private static readonly TimeSpan TimeBetweenTailAndHead = new TimeSpan(0, 0, 0, 0, 1);

        public DateTime ConstructTail(DateTime head, Resolution resolution)
        {
            var result = head;
            switch (resolution)
            {
                case Resolution.Days:
                    result = head.AddDays(1);
                    break;
                case Resolution.Weeks:
                    switch (result.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            result = result.AddDays(5);
                            break;
                        case DayOfWeek.Tuesday:
                            result = result.AddDays(4);
                            break;
                        case DayOfWeek.Wednesday:
                            result = result.AddDays(3);
                            break;
                        case DayOfWeek.Thursday:
                            result = result.AddDays(2);
                            break;
                        case DayOfWeek.Friday:
                            result = result.AddDays(1);
                            break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("resolution", resolution, String.Format(CultureInfo.InvariantCulture, Strings.StaticPricePeriodImpl_ConstructTail_Unable_to_identify_the_period_tail_for_resolution__0__, resolution));
            }
            return result.Subtract(TimeBetweenTailAndHead);
        }

        public Resolution CalculateResolution(TimeSpan timeSpan)
        {
            var thisTicks = timeSpan.Ticks;
            var resolutions = Enum.GetValues(typeof(Resolution)).Cast<long>().OrderBy(ticks => ticks);
            return (Resolution)Enum.ToObject(typeof(Resolution), resolutions.First(ticks => thisTicks <= ticks));
        }
    }
}