using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction for deposit.
    /// </summary>
    [Serializable]
    internal sealed class Deposit : CashTransaction, IDeposit
    {
        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        internal Deposit()
        {
            OrderType = OrderType.Deposit;
        }
    }
}