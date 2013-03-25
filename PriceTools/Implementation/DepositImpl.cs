using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction for deposit.
    /// </summary>
    [Serializable]
    internal sealed class DepositImpl : CashTransactionImpl, IDeposit
    {
        /// <summary>
        /// Constructs a deposit-type transaction.
        /// </summary>
        /// <param name="factoryGuid"></param>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds deposited.</param>
        internal DepositImpl(Guid factoryGuid, DateTime settlementDate, decimal amount)
            : base(factoryGuid, settlementDate, Math.Abs(amount))
        {}
    }
}