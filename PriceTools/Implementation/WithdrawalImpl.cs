using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction for a cash withdrawal.
    /// </summary>
    [Serializable]
    internal sealed class WithdrawalImpl : CashTransaction, Withdrawal
    {
        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        internal WithdrawalImpl()
        {
            OrderType = OrderType.Withdrawal;
        }
    }
}