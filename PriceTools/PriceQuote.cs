using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a price quote for a financial security.
    /// </summary>
    public class PriceQuote : IPriceQuote
    {
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
        public DateTime SettlementDate { get; set; }

        /// <summary>
        /// The price at which the security is available.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The number of shares traded.
        /// </summary>
        public long? Volume { get; set; }

        #endregion
    }
}
