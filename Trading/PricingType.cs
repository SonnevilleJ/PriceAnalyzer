using System;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// Specifies the type of order for a transaction.
    /// </summary>
    [Flags]
    public enum PricingType
    {
        /// <summary>
        /// Indicates the order should be executed as soon as possible at the current market price.
        /// </summary>
        Market      = 1,

        /// <summary>
        /// Indicates the order should be executed at the limit price or better.
        /// </summary>
        Limit       = 2,

        /// <summary>
        /// Indicates that once the market price reaches a given price, the order should be executed as soon as possible at the current market price.
        /// </summary>
        Stop        = 4,

        /// <summary>
        /// Indicates that once the market price reaches a given price, the order should be executed as soon as possible at the current market price.
        /// </summary>
        StopMarket  = Stop | Market,

        /// <summary>
        /// Indicates that once the market price reaches a given price, the order should be executed at the limit price or better.
        /// </summary>
        StopLimit   = Stop | Limit
    }
}
