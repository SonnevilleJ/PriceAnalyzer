using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction for a cash withdrawal.
    /// </summary>
    [Serializable]
    public sealed class Withdrawal : CashTransaction
    {
        /// <summary>
        /// Constructs a withdrawal-type transaction.
        /// </summary>
        /// <param name="factoryGuid"></param>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds withdrawn.</param>
        /// <returns></returns>
        internal Withdrawal(Guid factoryGuid, DateTime settlementDate, decimal amount)
            : base(factoryGuid, settlementDate, -Math.Abs(amount))
        {
        }
    }
}