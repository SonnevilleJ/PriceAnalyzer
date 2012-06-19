using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to buy shares.
    /// </summary>
    [Serializable]
    public sealed class Buy : ShareTransaction, LongTransaction, AccumulationTransaction, OpeningTransaction
    {
    }
}