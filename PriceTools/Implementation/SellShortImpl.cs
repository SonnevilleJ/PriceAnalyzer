﻿using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to short-sell shares.
    /// </summary>
    [Serializable]
    internal sealed class SellShortImpl : ShareTransactionImpl, SellShort
    {
        /// <summary>
        /// Constructs a SellShort Transaction.
        /// </summary>
        internal SellShortImpl()
        {
            OrderType = OrderType.SellShort;
        }
    }
}