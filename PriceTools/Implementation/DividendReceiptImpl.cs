using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction for the receipt of a dividend.
    /// </summary>
    [Serializable]
    internal sealed class DividendReceiptImpl : CashTransactionImpl, IDividendReceipt
    {
        /// <summary>
        /// Constructs a dividend-type transaction where funds were received.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds received.</param>
        /// <returns></returns>
        internal DividendReceiptImpl(DateTime settlementDate, decimal amount)
            : base(settlementDate, Math.Abs(amount))
        {
        }
    }
}