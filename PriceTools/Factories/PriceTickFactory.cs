﻿using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs <see cref="IPriceTick"/>s.
    /// </summary>
    public class PriceTickFactory : IPriceTickFactory
    {
        /// <summary>
        /// Constructs a PriceTick.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> for which the quote is valid.</param>
        /// <param name="price">The quoted price.</param>
        /// <param name="volume">The number of shares for which the quote is valid.</param>
        public IPriceTick ConstructPriceTick(DateTime settlementDate, decimal price, long? volume = null)
        {
            return new PriceTickImpl(settlementDate, price, volume);
        }
    }
}
