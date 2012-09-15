using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs TimePeriod objects.
    /// </summary>
    public static class TimePeriodFactory
    {
        /// <summary>
        /// Constructs an immutable <see cref="ITimePeriod"/> object.
        /// </summary>
        /// <param name="head"></param>
        /// <param name="tail"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ITimePeriod ConstructTimePeriod(DateTime head, DateTime tail, decimal value)
        {
            return new SimplePeriodImpl(head, tail, value);
        }
    }
}
