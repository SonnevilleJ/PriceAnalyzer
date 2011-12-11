namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to buy shares to cover a previous short sell.
    /// </summary>
    public sealed class BuyToCover : ShareTransaction
    {
        /// <summary>
        /// Constructs a BuyToCover Transaction.
        /// </summary>
        public BuyToCover()
        {
            OrderType = OrderType.BuyToCover;
        }
    }
}