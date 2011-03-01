using System;
using System.Data.Objects.DataClasses;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of price data.
    /// </summary>
    public interface IPriceSeries : IPricePeriod, IEquatable<IPriceSeries>
    {
        /// <summary>
        /// Gets the data point stored at a given index within this IPriceSeries.
        /// </summary>
        /// <param name="index">The index to retrieve.</param>
        /// <returns>The data point stored at the given index.</returns>
        new IPriceQuote this[DateTime index] { get; }

        /// <summary>
        /// Event which is invoked when new price data is available for the IPriceSeries.
        /// </summary>
        event EventHandler<NewPriceDataAvailableEventArgs> NewPriceDataAvailable;
        
        /// <summary>
        /// Gets a <see cref="TimeSpan"/> value indicating the length of time covered by this IPriceSeries.
        /// </summary>
        TimeSpan TimeSpan { get; }

        /// <summary>
        /// Gets a collection of the <see cref="IPriceQuote"/>s in this IPriceSeries.
        /// </summary>
        EntityCollection<PriceQuote> PriceQuotes { get; }

        /// <summary>
        /// Adds one or more <see cref="IPriceQuote"/>s to the IPriceSeries.
        /// </summary>
        /// <param name="priceQuote">The <see cref="IPriceQuote"/>s to add.</param>
        void AddPriceQuote(params IPriceQuote[] priceQuote);
    }
}
