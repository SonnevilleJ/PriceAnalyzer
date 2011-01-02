namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Specifies the type of order for a transaction.
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        ///   A cash deposit.
        /// </summary>
        Deposit,

        /// <summary>
        ///   A cash withdrawal.
        /// </summary>
        Withdrwawal,

        /// <summary>
        ///   A cash dividend was received.
        /// </summary>
        Dividend,

        /// <summary>
        ///   A buy transaction or a new investment.
        /// </summary>
        Buy,

        /// <summary>
        ///   A sell transaction.
        /// </summary>
        Sell,

        /// <summary>
        ///   A buy transaction used to cover a <see cref="OrderType.SellShort"/> order.
        /// </summary>
        BuyToCover,

        /// <summary>
        ///   An order to sell short.
        /// </summary>
        SellShort
    }
}