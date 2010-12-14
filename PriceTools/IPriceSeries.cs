using System.Collections.ObjectModel;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of price data.
    /// </summary>
    public interface IPriceSeries : IPricePeriod, ITimeSeries
    {
        /// <summary>
        /// Inserts an IPricePeriod to this IPriceSeries.
        /// </summary>
        /// <param name="period">The IPricePeriod to insert into the IPriceSeries.</param>
        void InsertPeriod(IPricePeriod period);

        /// <summary>
        /// Converts this IPriceSeries to an IPricePeriod.
        /// </summary>
        /// <remarks>Note that this will result in a loss of resolution.</remarks>
        /// <returns>A new IPricePeriod with the summarized price data from this IPriceSeries.</returns>
        IPricePeriod ToPricePeriod();

        /// <summary>
        /// Gets the internal array of IPricePeriod objects stored within this IPriceSeries.
        /// </summary>
        ReadOnlyCollection<PricePeriod> Periods { get; }

        /// <summary>
        /// Gets the IPricePeriod at a given index within this IPriceSeries.
        /// </summary>
        /// <param name="index">The index of the IPricePeriod to retrieve.</param>
        /// <returns>The IPricePeriod stored at the given index.</returns>
        new IPricePeriod this[int index] { get; }
    }
}
