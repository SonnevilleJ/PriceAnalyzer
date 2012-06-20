using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for deposit.
    /// </summary>
    [Serializable]
    public sealed class Deposit : CashTransaction
    {
        /// <summary>
        /// Constructs a deposit-type transaction.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds deposited.</param>
        internal Deposit(DateTime settlementDate, decimal amount)
            : base(settlementDate, Math.Abs(amount))
        {}
    }
}