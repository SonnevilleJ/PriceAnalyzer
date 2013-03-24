using System;

namespace Sonneville.PriceTools
{
    public interface ITimePeriodFactory
    {
        /// <summary>
        /// Constructs an immutable <see cref="ITimePeriod"/> object.
        /// </summary>
        /// <param name="head"></param>
        /// <param name="tail"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        ITimePeriod ConstructTimePeriod(DateTime head, DateTime tail, decimal value);
    }
}