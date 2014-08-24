using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public class TimeSeriesFactory<TPeriodValue> : ITimeSeriesFactory<TPeriodValue>
    {
        public ITimeSeries<ITimePeriod<TPeriodValue>, TPeriodValue> ConstructMutable()
        {
            return ConstructMutable(new List<ITimePeriod<TPeriodValue>>());
        }

        public ITimeSeries<ITimePeriod<TPeriodValue>, TPeriodValue> ConstructMutable(IEnumerable<ITimePeriod<TPeriodValue>> timePeriods)
        {
            return new TimeSeries<TPeriodValue>(timePeriods);
        }

        public ITimeSeries<ITimePeriod<TPeriodValue>, TPeriodValue> ConstructImmutable(IEnumerable<ITimePeriod<TPeriodValue>> timePeriods)
        {

            return new TimeSeries<TPeriodValue>(new List<ITimePeriod<TPeriodValue>>(timePeriods).AsReadOnly());
        }
    }
}
