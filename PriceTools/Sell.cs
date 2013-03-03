using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to sell shares.
    /// </summary>
    [Serializable]
    public sealed class Sell : ShareTransactionImpl, LongTransaction, DistributionTransaction, ClosingTransaction
    {
        /// <summary>
        /// Constructs a Sell.
        /// </summary>
        internal Sell(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(ticker, settlementDate, shares, -Math.Abs(price), commission)
        {
        }
    }
}