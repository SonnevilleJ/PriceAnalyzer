using System;

namespace Sonneville.PriceTools.Implementation
{
    [Serializable]
    public sealed class Buy : ShareTransaction
    {
        internal Buy(Guid factoryGuid, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(factoryGuid, ticker, settlementDate, shares, Math.Abs(price), commission)
        {
        }
    }
}