using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to buy shares to cover a previous short sell.
    /// </summary>
    [Serializable]
    internal sealed class BuyToCoverImpl : ShareTransactionImpl, IBuyToCover
    {
        /// <summary>
        /// Constructs a BuyToCover.
        /// </summary>
        internal BuyToCoverImpl(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(ticker, settlementDate, shares, -Math.Abs(price), commission)
        {
        }
    }
}