using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to buy shares to cover a previous short sell.
    /// </summary>
    [Serializable]
    public sealed class BuyToCover : ShareTransactionImpl, ShortTransaction, AccumulationTransaction, ClosingTransaction
    {
        /// <summary>
        /// Constructs a BuyToCover.
        /// </summary>
        internal BuyToCover(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(ticker, settlementDate, shares, -Math.Abs(price), commission)
        {
        }
    }
}