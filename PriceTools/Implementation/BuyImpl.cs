using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to buy shares.
    /// </summary>
    [Serializable]
    internal sealed class BuyImpl : ShareTransactionImpl, IBuy
    {
        /// <summary>
        /// Constructs a Buy.
        /// </summary>
        internal BuyImpl(Guid factoryGuid, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(factoryGuid, ticker, settlementDate, shares, Math.Abs(price), commission)
        {
        }
    }
}