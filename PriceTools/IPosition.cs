using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a IPosition taken using one or more <see cref = "IShareTransaction" />s.
    /// </summary>
    public interface IPosition : IMeasurableSecurityBasket
    {
        /// <summary>
        ///   Gets the ticker symbol held by this IPosition.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        ///   Calculates the average cost of all held shares in this IPosition as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The average cost of all shares held at <paramref name = "settlementDate" />.</returns>
        decimal CalculateAverageCost(DateTime settlementDate);

        /// <summary>
        ///   Buys shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        void Buy(DateTime settlementDate, double shares, decimal price, decimal commission);

        /// <summary>
        ///   Buys shares of the ticker held by this IPosition to cover a previous ShortSell.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction. Shares cannot exceed currently shorted shares.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        void BuyToCover(DateTime settlementDate, double shares, decimal price, decimal commission);

        /// <summary>
        ///   Sells shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction. Shares connot exceed currently held shares.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        void Sell(DateTime settlementDate, double shares, decimal price, decimal commission);

        /// <summary>
        ///   Sell short shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name = "settlementDate">The date of this shareTransaction.</param>
        /// <param name = "shares">The number of shares in this shareTransaction.</param>
        /// <param name = "price">The per-share price of this shareTransaction.</param>
        /// <param name = "commission">The commission paid for this shareTransaction.</param>
        void SellShort(DateTime settlementDate, double shares, decimal price, decimal commission);

        /// <summary>
        ///   Adds an IShareTransaction to the IPosition.
        /// </summary>
        /// <param name = "shareTransaction"></param>
        void AddTransaction(IShareTransaction shareTransaction);

        /// <summary>
        /// Gets an <see cref="IList{IHolding}"/> from the transactions in the IPosition.
        /// </summary>
        /// <param name="settlementDate">The latest date used to include a transaction in the calculation.</param>
        /// <returns>An <see cref="IList{IHolding}"/> of the transactions in the IPosition.</returns>
        IList<IHolding> CalculateHoldings(DateTime settlementDate);
    }
}
