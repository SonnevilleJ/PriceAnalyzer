using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to short-sell shares.
    /// </summary>
    [Serializable]
    internal sealed class SellShortImpl : ShareTransactionImpl, ISellShort
    {
        /// <summary>
        /// Constructs a SellShort.
        /// </summary>
        internal SellShortImpl(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(ticker, settlementDate, shares, Math.Abs(price), commission)
        {
        }
    }
}