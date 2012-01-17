using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// An order to take a position on a financial security.
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// The DateTime this order was issued.
        /// </summary>
        DateTime Issued { get; set; }

        /// <summary>
        /// The DateTime this order expires.
        /// </summary>
        DateTime Expiration { get; set; }

        /// <summary>
        /// The ticker symbol for this order.
        /// </summary>
        string Ticker { get; set; }

        /// <summary>
        /// The price at which the order should be executed.
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        /// Specifies the type of order.
        /// </summary>
        OrderType OrderType { get; set; }

        /// <summary>
        /// The <see cref="PricingType"/> (market or limit) which should be used when submitting this order.
        /// </summary>
        PricingType PricingType { get; set; }

        /// <summary>
        /// The number of shares for this order.
        /// </summary>
        double Shares { get; set; }
    }
}