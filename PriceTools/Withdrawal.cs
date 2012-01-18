using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for a cash withdrawal.
    /// </summary>
    [Serializable]
    internal sealed class Withdrawal : CashTransaction, IWithdrawal
    {
        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        internal Withdrawal()
        {
            OrderType = OrderType.Withdrawal;
        }
    }
}