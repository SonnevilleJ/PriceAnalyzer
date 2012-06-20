using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to short-sell shares.
    /// </summary>
    [Serializable]
    public sealed class SellShort : ShareTransaction, ShortTransaction, DistributionTransaction, OpeningTransaction
    {
        /// <summary>
        /// Constructs a SellShort.
        /// </summary>
        internal SellShort(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(ticker, settlementDate, shares, Math.Abs(price), commission)
        {
        }
    }
}