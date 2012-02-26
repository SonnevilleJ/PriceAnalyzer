using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// An order to take a position on a financial security. This class cannot be inherited.
    /// </summary>
    [Serializable]
    internal sealed class OrderImpl : Order
    {
        /// <summary>
        /// The DateTime this order was issued.
        /// </summary>
        public DateTime Issued { get; internal set; }

        /// <summary>
        /// The DateTime this order expires.
        /// </summary>
        public DateTime Expiration { get; internal set; }

        /// <summary>
        /// The ticker symbol for this order.
        /// </summary>
        public string Ticker { get; internal set; }

        /// <summary>
        /// The price at which the order should be executed.
        /// </summary>
        public decimal Price { get; internal set; }

        /// <summary>
        /// Specifies the type of order.
        /// </summary>
        public OrderType OrderType { get; internal set; }

        /// <summary>
        /// The <see cref="PricingType"/> (market or limit) which should be used when submitting this order.
        /// </summary>
        public PricingType PricingType { get; internal set; }

        /// <summary>
        /// The number of shares for this order.
        /// </summary>
        public decimal Shares { get; internal set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return String.Format("{0}: {1} {2} shares of {3} at {4}", Issued, OrderType, Shares, Ticker, Price);
        }
    }
}