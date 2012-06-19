using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to buy shares to cover a previous short sell.
    /// </summary>
    [Serializable]
    public sealed class BuyToCover : ShareTransaction, ShortTransaction, AccumulationTransaction, ClosingTransaction
    {
    }
}