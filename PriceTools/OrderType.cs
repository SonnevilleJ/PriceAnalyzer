using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Specifies the type of order for a transaction.
    /// </summary>
    [Flags]
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
        ///   A cash dividend was reinvested.
        /// </summary>
        DividendReinvestment    = 4,

        /// <summary>
        ///   An order to sell.
        /// </summary>
        Sell                    = 8,

        /// <summary>
        ///   An order to buy used to cover a <see cref="OrderType.SellShort"/> order.
        /// </summary>
        BuyToCover              = 16,

        /// <summary>
        ///   An order to buy.
        /// </summary>
        Buy                     = 32,

        /// <summary>
        ///   An order to sell short.
        /// </summary>
        SellShort               = 64,

        /// <summary>
        ///   A cash withdrawal.
        /// </summary>
        Withdrawal              = 128
    }
}