namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to short-sell shares.
    /// </summary>
    public sealed class SellShort : ShareTransaction
    {
        /// <summary>
        /// Constructs a SellShort Transaction.
        /// </summary>
        public SellShort()
        {
            OrderType = OrderType.SellShort;
        }
    }
}