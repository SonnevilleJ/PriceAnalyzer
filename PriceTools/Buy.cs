namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to buy shares.
    /// </summary>
    public sealed class Buy : ShareTransaction
    {
        /// <summary>
        /// Constructs a Buy Transaction.
        /// </summary>
        public Buy()
        {
            OrderType = OrderType.Buy;
        }
    }
}