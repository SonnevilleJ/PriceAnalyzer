using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a price quote for a financial security.
    /// </summary>
    public interface IPriceQuote
    {
        /// <summary>
        /// The <see cref="DateTime"/> which the price quote is made.
        /// </summary>
        DateTime SettlementDate { get; set; }

        /// <summary>
        /// The price at which the security is available.
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        /// The number of shares traded.
        /// </summary>
        long? Volume { get; set; }
    }
}
