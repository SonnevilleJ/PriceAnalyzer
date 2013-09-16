using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to buy shares to cover a previous short sell.
    /// </summary>
    [Serializable]
    public sealed class BuyToCover : ShareTransaction, IClosingTransaction
    {
        /// <summary>
        /// Constructs a BuyToCover.
        /// </summary>
        internal BuyToCover(Guid factoryGuid, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(factoryGuid, ticker, settlementDate, shares, -Math.Abs(price), commission)
        {
        }
    }
}