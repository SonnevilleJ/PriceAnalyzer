using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A value which changes with time.
    /// </summary>
    public interface IVariableValue
    {
        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimePeriod.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimePeriod as of the given DateTime.</returns>
        decimal this[DateTime dateTime] { get; }

        /// <summary>
        /// Gets the first DateTime for which a value exists.
        /// </summary>
        DateTime Head { get; }

        /// <summary>
        /// Gets the last DateTime for which a value exists.
        /// </summary>
        DateTime Tail { get; }
    }
}