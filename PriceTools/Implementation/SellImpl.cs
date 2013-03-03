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
        internal SellImpl(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(ticker, settlementDate, shares, -Math.Abs(price), commission)
        {
        }
    }
}