using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to sell shares.
    /// </summary>
    [Serializable]
    public sealed class Sell : ShareTransaction, ILongTransaction, IDistributionTransaction, IClosingTransaction
    {
        /// <summary>
        /// Constructs a Sell.
        /// </summary>
        internal Sell(Guid factoryGuid, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(factoryGuid, ticker, settlementDate, shares, -Math.Abs(price), commission)
        {
        }
    }
}