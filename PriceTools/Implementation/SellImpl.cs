using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to sell shares.
    /// </summary>
    [Serializable]
    internal sealed class SellImpl : ShareTransaction, ISell
    {
        /// <summary>
        /// Constructs a Sell Transaction.
        /// </summary>
        internal SellImpl()
        {
            OrderType = OrderType.Sell;
        }
    }
}