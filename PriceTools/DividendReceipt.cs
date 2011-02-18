namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for receipt of dividends.
    /// </summary>
    public sealed partial class DividendReceipt : ShareTransaction
    {
        /// <summary>
        /// Constructs a DividendReceipt Transaction.
        /// </summary>
        public DividendReceipt()
        {
            OrderType = OrderType.DividendReceipt;
        }
    }
}