using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction for deposit.
    /// </summary>
    [Serializable]
    internal sealed class DepositImpl : CashTransactionImpl, Deposit
    {
    }
}