using System;

namespace Sonneville.PriceTools.Implementation
{
    [Serializable]
    public sealed class Withdrawal : CashTransaction
    {
        internal Withdrawal(Guid factoryGuid, DateTime settlementDate, decimal amount)
            : base(factoryGuid, settlementDate, -Math.Abs(amount))
        {
        }
    }
}