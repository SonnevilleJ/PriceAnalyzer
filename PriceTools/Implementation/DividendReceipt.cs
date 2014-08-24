using System;

namespace Sonneville.PriceTools.Implementation
{
    [Serializable]
    public sealed class DividendReceipt : CashTransaction
    {
        internal DividendReceipt(Guid factoryGuid, DateTime settlementDate, decimal amount)
            : base(factoryGuid, settlementDate, Math.Abs(amount))
        {
        }
    }
}