using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to short-sell shares.
    /// </summary>
    [Serializable]
    public sealed class SellShort : ShareTransaction, ShortTransaction, DistributionTransaction, OpeningTransaction
    {
    }
}