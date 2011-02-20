using System;
using System.Data.Objects.DataClasses;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of price data.
    /// </summary>
    public interface IPriceSeries : IPricePeriod
    {
        /// <summary>
        /// Inserts an IPricePeriod to this IPriceSeries.
        /// </summary>
        /// <param name="period">The IPricePeriod to insert into the IPriceSeries.</param>
        void InsertPeriod(IPricePeriod period);

        /// <summary>
        /// Gets the internal array of IPricePeriod objects stored within this IPriceSeries.
        /// </summary>
        EntityCollection<PricePeriod> Periods { get; }

        /// <summary>
        /// Gets the <see cref="IPricePeriod"/> at a given index within this IPriceSeries.
        /// </summary>
        /// <param name="index">The index of the <see cref="IPricePeriod"/> to retrieve.</param>
        /// <returns>The <see cref="IPricePeriod"/> stored at the given index.</returns>
        new IPricePeriod this[DateTime index] { get; }
    }
}
