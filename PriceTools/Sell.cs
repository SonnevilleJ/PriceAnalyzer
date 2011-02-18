namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to sell shares.
    /// </summary>
    public sealed partial class Sell : ShareTransaction
    {
        /// <summary>
        /// Constructs a Sell Transaction.
        /// </summary>
        public Sell()
        {
            OrderType = OrderType.Sell;
        }
    }
}