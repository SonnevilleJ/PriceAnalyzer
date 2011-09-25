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
        Deposit                 = 1,

        /// <summary>
        ///   A cash dividend was received.
        /// </summary>
        DividendReceipt         = 2,

        /// <summary>
        /// A cash dividend was reinvested.
        /// </summary>
        DividendReinvestment    = 3,

        /// <summary>
        ///   A sell transaction.
        /// </summary>
        Sell                    = 4,

        /// <summary>
        ///   A buy transaction used to cover a <see cref="OrderType.SellShort"/> order.
        /// </summary>
        BuyToCover              = 5,

        /// <summary>
        ///   A buy transaction or a new investment.
        /// </summary>
        Buy                     = 6,

        /// <summary>
        ///   An order to sell short.
        /// </summary>
        SellShort               = 7,

        /// <summary>
        ///   A cash withdrawal.
        /// </summary>
        Withdrawal              = 8
    }
}