using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs <see cref="PriceQuote"/>s.
    /// </summary>
    public static class PriceQuoteFactory
    {
        /// <summary>
        /// Constructs a PriceQuote.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> for which the quote is valid.</param>
        /// <param name="price">The quoted price.</param>
        /// <param name="volume">The number of shares for which the quote is valid.</param>
        public static PriceQuote ConstructPriceQuote(DateTime settlementDate, decimal price, long? volume = null)
        {
            return new PriceQuoteImpl(settlementDate, price, volume);
        }
    }
}
