using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// An order to take a position on a financial security.
    /// </summary>
    public interface Order
    {
        /// <summary>
        /// The DateTime this order was issued.
        /// </summary>
        DateTime Issued { get; }

        /// <summary>
        /// The DateTime this order expires.
        /// </summary>
        DateTime Expiration { get; }

        /// <summary>
        /// The ticker symbol for this order.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        /// The price at which the order should be executed.
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// Specifies the type of order.
        /// </summary>
        OrderType OrderType { get; }

        /// <summary>
        /// The <see cref="PricingType"/> (market or limit) which should be used when submitting this order.
        /// </summary>
        PricingType PricingType { get; }

        /// <summary>
        /// The number of shares for this order.
        /// </summary>
        decimal Shares { get; }
    }
}