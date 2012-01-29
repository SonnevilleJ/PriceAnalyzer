using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A <see cref="PricePeriodImpl"/> made from <see cref="PriceQuotes"/>.
    /// </summary>
    public interface QuotedPricePeriod : PricePeriod
    {
        /// <summary>
        /// The <see cref="PriceQuoteImpl"/>s contained within this QuotedPricePeriod.
        /// </summary>
        IList<PriceQuote> PriceQuotes { get; }

        /// <summary>
        ///   Adds one or more <see cref = "PriceQuote" />s to the PriceSeries.
        /// </summary>
        /// <param name = "priceQuotes">The <see cref = "PriceQuote" />s to add.</param>
        void AddPriceQuotes(params PriceQuote[] priceQuotes);
    }
}