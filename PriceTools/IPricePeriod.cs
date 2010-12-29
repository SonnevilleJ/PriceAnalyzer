using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents price history for a desired length of time.
    /// </summary>
    public interface IPricePeriod : IComparable, ISerializable
    {
        /// <summary>
        /// Gets the closing price for the IPricePeriod.
        /// </summary>
        decimal Close { get; }

        /// <summary>
        /// Gets the beginning DateTime of the IPricePeriod.
        /// </summary>
        DateTime Head { get; }

        /// <summary>
        /// Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        decimal? High { get; }

        /// <summary>
        /// Gets the lowest price that occurred during  the IPricePeriod.
        /// </summary>
        decimal? Low { get; }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        decimal? Open { get; }

        /// <summary>
        /// Gets the ending DateTime of the IPricePeriod.
        /// </summary>
        DateTime Tail { get; }

        /// <summary>
        /// Gets a TimeSpan representing the total duration of the IPricePeriod.
        /// </summary>
        TimeSpan TimeSpan { get; }

        /// <summary>
        /// Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        ulong? Volume { get; }

        /// <summary>
        /// Validates an IPricePeriod.
        /// </summary>
        /// <returns>A value indicating if the instance is valid.</returns>
        void Validate();

        /// <summary>
        /// Validates an IPricePeriod.
        /// </summary>
        /// <param name="errors">A list of any validation errors.</param>
        /// <returns>A value indicating if the instance is valid.</returns>
        bool Validate(out IList<string> errors);
    }
}
