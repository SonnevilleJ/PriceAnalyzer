using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a price quote for a financial security.
    /// </summary>
    [Serializable]
    internal class PriceQuoteImpl : IPriceQuote
    {
        /// <summary>
        /// Constructs a PriceQuote.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> for which the quote is valid.</param>
        /// <param name="price">The quoted price.</param>
        /// <param name="volume">The number of shares for which the quote is valid.</param>
        internal PriceQuoteImpl(DateTime settlementDate, decimal price, long? volume = null)
        {
            if (price <= 0)
                throw new ArgumentOutOfRangeException("price", price, Strings.PriceQuoteImpl_PriceQuoteImpl_Quoted_price_must_be_greater_than_zero_);
            if (volume.HasValue && volume <= 0)
                throw new ArgumentOutOfRangeException("volume", volume, Strings.PriceQuoteImpl_PriceQuoteImpl_Quoted_volume__if_specified__must_be_greater_than_zero_);

            SettlementDate = settlementDate;
            Price = price;
            Volume = volume;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("{0}: {1} shares @ {2:c}", SettlementDate, Volume, Price);
        }

        #region Implementation of IPriceQuote

        /// <summary>
        /// The <see cref="DateTime"/> which the price quote is made.
        /// </summary>
        public DateTime SettlementDate { get; private set; }

        /// <summary>
        /// The price at which the security is available.
        /// </summary>
        public decimal Price { get; private set; }

        /// <summary>
        /// The number of shares traded.
        /// </summary>
        public long? Volume { get; private set; }

        #endregion
    }
}
