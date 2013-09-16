using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to buy shares.
    /// </summary>
    [Serializable]
    public sealed class Buy : ShareTransaction, IOpeningTransaction
    {
        /// <summary>
        /// Constructs a Buy.
        /// </summary>
        internal Buy(Guid factoryGuid, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(factoryGuid, ticker, settlementDate, shares, Math.Abs(price), commission)
        {
        }
    }
}