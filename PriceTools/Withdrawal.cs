using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for a cash withdrawal.
    /// </summary>
    [Serializable]
    public sealed class Withdrawal : CashTransactionImpl
    {
        /// <summary>
        /// Constructs a withdrawal-type transaction.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds withdrawn.</param>
        /// <returns></returns>
        internal Withdrawal(DateTime settlementDate, decimal amount)
            : base(settlementDate, -Math.Abs(amount))
        {
        }
    }
}