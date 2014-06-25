using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs TimePeriod objects.
    /// </summary>
    public class TimePeriodFactory : ITimePeriodFactory<decimal>
    {
        /// <summary>
        /// Constructs an immutable <see cref="ITimePeriod"/> object.
        /// </summary>
        /// <param name="head"></param>
        /// <param name="tail"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITimePeriod<decimal> ConstructTimePeriod(DateTime head, DateTime tail, decimal value)
        {
            return new TimePeriod<decimal>(head, tail, value);
        }
    }
}
