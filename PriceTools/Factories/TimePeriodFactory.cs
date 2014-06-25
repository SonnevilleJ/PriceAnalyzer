using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs TimePeriod objects.
    /// </summary>
    public class TimePeriodFactory<TPeriodValue> : ITimePeriodFactory<TPeriodValue>
    {
        /// <summary>
        /// Constructs an immutable <see cref="ITimePeriod"/> object.
        /// </summary>
        /// <param name="head"></param>
        /// <param name="tail"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITimePeriod<TPeriodValue> ConstructTimePeriod(DateTime head, DateTime tail, TPeriodValue value)
        {
            return new TimePeriod<TPeriodValue>(head, tail, value);
        }
    }
}
