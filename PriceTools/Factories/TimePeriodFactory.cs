using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public class TimePeriodFactory<TPeriodValue> : ITimePeriodFactory<TPeriodValue>
    {
        public ITimePeriod<TPeriodValue> ConstructTimePeriod(DateTime head, DateTime tail, TPeriodValue value)
        {
            return new TimePeriod<TPeriodValue>(head, tail, value);
        }
    }
}
