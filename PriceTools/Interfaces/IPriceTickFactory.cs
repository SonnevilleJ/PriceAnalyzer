using System;

namespace Sonneville.PriceTools
{
    public interface IPriceTickFactory
    {
        /// <summary>
        /// Constructs a PriceTick.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> for which the quote is valid.</param>
        /// <param name="price">The quoted price.</param>
        /// <param name="volume">The number of shares for which the quote is valid.</param>
        IPriceTick ConstructPriceTick(DateTime settlementDate, decimal price, long? volume = null);
    }
}