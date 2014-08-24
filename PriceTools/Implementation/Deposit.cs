using System;

namespace Sonneville.PriceTools.Implementation
{
    [Serializable]
    public sealed class Deposit : CashTransaction
    {
        internal Deposit(Guid factoryGuid, DateTime settlementDate, decimal amount)
            : base(factoryGuid, settlementDate, Math.Abs(amount))
        {}
    }
}