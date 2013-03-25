using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to sell shares.
    /// </summary>
    [Serializable]
    internal sealed class SellImpl : ShareTransactionImpl, ISell
    {
        /// <summary>
        /// Constructs a Sell.
        /// </summary>
        internal SellImpl(Guid factoryGuid, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(factoryGuid, ticker, settlementDate, shares, -Math.Abs(price), commission)
        {
        }
    }
}