using System.Data.Objects.DataClasses;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of price data.
    /// </summary>
    public interface IPriceSeries : IPricePeriod, IPriceDataRetriever
    {
        /// <summary>
        /// Gets the ticker symbol priced by this IPriceSeries.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries.
        /// </summary>
        EntityCollection<PricePeriod> PricePeriods { get; }
    }
}
