using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to buy shares to cover a previous short sell.
    /// </summary>
    [Serializable]
    internal sealed class BuyToCoverImpl : ShareTransactionImpl, BuyToCover
    {
        /// <summary>
        /// Constructs a BuyToCover Transaction.
        /// </summary>
        internal BuyToCoverImpl()
        {
            OrderType = OrderType.BuyToCover;
        }
    }
}