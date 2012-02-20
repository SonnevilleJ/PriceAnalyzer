using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction for the receipt of a dividend.
    /// </summary>
    [Serializable]
    internal sealed class DividendReceiptImpl : CashTransactionImpl, DividendReceipt
    {
    }
}