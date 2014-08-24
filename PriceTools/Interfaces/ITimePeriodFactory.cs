using System;

namespace Sonneville.PriceTools
{
    public interface ITimePeriodFactory<TPeriodValue>
    {
        ITimePeriod<TPeriodValue> ConstructTimePeriod(DateTime head, DateTime tail, TPeriodValue value);
    }
}