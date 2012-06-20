using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to buy shares.
    /// </summary>
    [Serializable]
    public sealed class Buy : ShareTransaction, LongTransaction, AccumulationTransaction, OpeningTransaction
    {
        /// <summary>
        /// Constructs a Buy.
        /// </summary>
        internal Buy(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(ticker, settlementDate, shares, Math.Abs(price), commission)
        {
        }
    }
}