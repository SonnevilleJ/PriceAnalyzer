using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs <see cref="PriceTick"/>s.
    /// </summary>
    public static class PriceTickFactory
    {
        /// <summary>
        /// Constructs a PriceTick.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> for which the quote is valid.</param>
        /// <param name="price">The quoted price.</param>
        /// <param name="volume">The number of shares for which the quote is valid.</param>
        public static PriceTick ConstructPriceTick(DateTime settlementDate, decimal price, long? volume = null)
        {
            return new PriceTickImpl(settlementDate, price, volume);
        }
    }
}
