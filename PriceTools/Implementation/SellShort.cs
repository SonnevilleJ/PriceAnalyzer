using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to short-sell shares.
    /// </summary>
    [Serializable]
    public sealed class SellShort : ShareTransaction, IShortTransaction, IDistributionTransaction, IOpeningTransaction
    {
        /// <summary>
        /// Constructs a SellShort.
        /// </summary>
        internal SellShort(Guid factoryGuid, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(factoryGuid, ticker, settlementDate, shares, Math.Abs(price), commission)
        {
        }
    }
}