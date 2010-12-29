namespace Sonneville.PriceTools
{
    /// <summary>
    /// Specifies the type of order for a transaction.
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// A BUY transaction.
        /// </summary>
        Buy,

        /// <summary>
        /// A SELL transaction.
        /// </summary>
        Sell,

        /// <summary>
        /// A BUY transaction used to cover a SHORTSELL.
        /// </summary>
        BuyToCover,

        /// <summary>
        /// An order to sell short.
        /// </summary>
        SellShort
    }
}
