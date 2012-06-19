using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to sell shares.
    /// </summary>
    [Serializable]
    public sealed class Sell : ShareTransaction, LongTransaction, DistributionTransaction, ClosingTransaction
    {
    }
}