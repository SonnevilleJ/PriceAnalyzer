using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a Position taken using one or more <see cref = "ShareTransaction" />s.
    /// </summary>
    public interface Position : SecurityBasket
    {
        /// <summary>
        ///   Gets the ticker symbol held by this Position.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        ///   Buys shares of the ticker held by this Position.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        void Buy(DateTime settlementDate, double shares, decimal price, decimal commission);

        /// <summary>
        ///   Buys shares of the ticker held by this Position to cover a previous ShortSell.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction. Shares cannot exceed currently shorted shares.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        void BuyToCover(DateTime settlementDate, double shares, decimal price, decimal commission);

        /// <summary>
        ///   Sells shares of the ticker held by this Position.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction. Shares connot exceed currently held shares.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        void Sell(DateTime settlementDate, double shares, decimal price, decimal commission);

        /// <summary>
        ///   Sell short shares of the ticker held by this Position.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        void SellShort(DateTime settlementDate, double shares, decimal price, decimal commission);

        /// <summary>
        ///   Adds an ShareTransaction to the Position.
        /// </summary>
        /// <param name = "shareTransaction"></param>
        void AddTransaction(ShareTransaction shareTransaction);

        /// <summary>
        /// Validates a transaction without adding it to the Position.
        /// </summary>
        /// <param name="shareTransaction"></param>
        bool TransactionIsValid(ShareTransaction shareTransaction);
    }
}
