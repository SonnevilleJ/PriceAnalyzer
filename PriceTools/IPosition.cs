using System;
using System.Data.Objects.DataClasses;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a IPosition taken using one or more <see cref = "IShareTransaction" />s.
    /// </summary>
    public interface IPosition : IMeasurableSecurityBasket
    {
        /// <summary>
        ///   Gets an enumeration of all <see cref = "IShareTransaction" />s in this IPosition.
        /// </summary>
        EntityCollection<ShareTransaction> Transactions { get; }

        /// <summary>
        /// Gets the ticker symbol held by this IPosition.
        /// </summary>
        string Ticker { get; set; }

        /// <summary>
        /// Gets the average cost of all held shares in this IPosition as of a given date.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        /// <returns>The average cost of all shares held at <paramref name="settlementDate"/>.</returns>
        decimal GetAverageCost(DateTime settlementDate);

        /// <summary>
        ///   Gets the value of any shares held the IPortfolio as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the shares held in the IPortfolio as of the given date.</returns>
        decimal GetInvestedValue(DateTime settlementDate);

        /// <summary>
        /// Buys shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name="settlementDate">The date of this shareTransaction.</param>
        /// <param name="shares">The number of shares in this shareTransaction.</param>
        /// <param name="price">The per-share price of this shareTransaction.</param>
        /// <param name="commission">The commission paid for this shareTransaction.</param>
        void Buy(DateTime settlementDate, double shares, decimal price, decimal commission);

        /// <summary>
        /// Buys shares of the ticker held by this IPosition to cover a previous ShortSell.
        /// </summary>
        /// <param name="settlementDate">The date of this shareTransaction.</param>
        /// <param name="shares">The number of shares in this shareTransaction. Shares cannot exceed currently shorted shares.</param>
        /// <param name="price">The per-share price of this shareTransaction.</param>
        /// <param name="commission">The commission paid for this shareTransaction.</param>
        void BuyToCover(DateTime settlementDate, double shares, decimal price, decimal commission);

        /// <summary>
        /// Sells shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name="settlementDate">The date of this shareTransaction.</param>
        /// <param name="shares">The number of shares in this shareTransaction. Shares connot exceed currently held shares.</param>
        /// <param name="price">The per-share price of this shareTransaction.</param>
        /// <param name="commission">The commission paid for this shareTransaction.</param>
        void Sell(DateTime settlementDate, double shares, decimal price, decimal commission);

        /// <summary>
        /// Sell short shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name="settlementDate">The date of this shareTransaction.</param>
        /// <param name="shares">The number of shares in this shareTransaction.</param>
        /// <param name="price">The per-share price of this shareTransaction.</param>
        /// <param name="commission">The commission paid for this shareTransaction.</param>
        void SellShort(DateTime settlementDate, double shares, decimal price, decimal commission);

        /// <summary>
        /// Adds an IShareTransaction to the Position.
        /// </summary>
        /// <param name="shareTransaction"></param>
        void AddTransaction(IShareTransaction shareTransaction);
    }
}