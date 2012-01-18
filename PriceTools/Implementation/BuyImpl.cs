using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to buy shares.
    /// </summary>
    [Serializable]
    internal sealed class BuyImpl : ShareTransaction, IBuy
    {
        /// <summary>
        /// Constructs a Buy Transaction.
        /// </summary>
        internal BuyImpl()
        {
            OrderType = OrderType.Buy;
        }
    }
}