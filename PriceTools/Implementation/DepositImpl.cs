using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction for deposit.
    /// </summary>
    [Serializable]
    internal sealed class DepositImpl : CashTransaction, Deposit
    {
        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        internal DepositImpl()
        {
            OrderType = OrderType.Deposit;
        }
    }
}