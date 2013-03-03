using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for the receipt of a dividend.
    /// </summary>
    [Serializable]
    public sealed class DividendReceipt : CashTransactionImpl
    {
        /// <summary>
        /// Constructs a dividend-type transaction where funds were received.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds received.</param>
        /// <returns></returns>
        internal DividendReceipt(DateTime settlementDate, decimal amount)
            : base(settlementDate, Math.Abs(amount))
        {
        }
    }
}