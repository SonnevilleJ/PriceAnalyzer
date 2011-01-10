using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a IPosition taken using one or more <see cref = "ITransaction" />s.
    /// </summary>
    public interface IPosition : ITimeSeries, ISerializable
    {
        /// <summary>
        ///   Gets an enumeration of all <see cref = "ITransaction" />s in this IPosition.
        /// </summary>
        IList<ITransaction> Transactions { get; }

        /// <summary>
        ///   Gets the total number of currently held shares.
        /// </summary>
        double OpenShares { get; }

        /// <summary>
        /// Gets the ticker symbol held by this IPosition.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        /// Buys shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name="date">The date of this transaction.</param>
        /// <param name="shares">The number of shares in this transaction.</param>
        /// <param name="price">The per-share price of this transaction.</param>
        /// <param name="commission">The commission paid for this transaction.</param>
        void Buy(DateTime date, double shares, decimal price, decimal commission);

        /// <summary>
        /// Buys shares of the ticker held by this IPosition to cover a previous ShortSell.
        /// </summary>
        /// <param name="date">The date of this transaction.</param>
        /// <param name="shares">The number of shares in this transaction. Shares cannot exceed currently shorted shares.</param>
        /// <param name="price">The per-share price of this transaction.</param>
        /// <param name="commission">The commission paid for this transaction.</param>
        void BuyToCover(DateTime date, double shares, decimal price, decimal commission);

        /// <summary>
        /// Sells shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name="date">The date of this transaction.</param>
        /// <param name="shares">The number of shares in this transaction. Shares connot exceed currently held shares.</param>
        /// <param name="price">The per-share price of this transaction.</param>
        /// <param name="commission">The commission paid for this transaction.</param>
        void Sell(DateTime date, double shares, decimal price, decimal commission);

        /// <summary>
        /// Sell short shares of the ticker held by this IPosition.
        /// </summary>
        /// <param name="date">The date of this transaction.</param>
        /// <param name="shares">The number of shares in this transaction.</param>
        /// <param name="price">The per-share price of this transaction.</param>
        /// <param name="commission">The commission paid for this transaction.</param>
        void SellShort(DateTime date, double shares, decimal price, decimal commission);

        /// <summary>
        ///   Gets the value of the IPortfolio as of a given date.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the IPortfolio as of the given date.</returns>
        decimal GetValue(DateTime date);

        /// <summary>
        ///   Gets the value of the IPortfolio as of a given date.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <param name = "considerCommissions">A value indicating whether commissions should be included in the result.</param>
        /// <returns>The value of the IPortfolio as of the given date.</returns>
        decimal GetValue(DateTime date, bool considerCommissions);

        /// <summary>
        ///   Gets the gross investment of this IPosition, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases.</returns>
        decimal GetCost(DateTime date);

        /// <summary>
        ///   Gets the gross proceeds of this IPosition, ignoring all costs and commissions.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales.</returns>
        decimal GetProceeds(DateTime date);

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "ITransaction" />s.</returns>
        decimal GetCommissions(DateTime date);

        /// <summary>
        ///   Gets the raw rate of return for this IPosition, not accounting for commissions.
        /// </summary>
        decimal GetRawReturn(DateTime date);

        /// <summary>
        ///   Gets the total rate of return for this IPosition, after commissions.
        /// </summary>
        decimal GetTotalReturn(DateTime date);

        /// <summary>
        ///   Gets the total rate of return on an annual basis for this IPosition.
        /// </summary>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        decimal GetTotalAnnualReturn(DateTime date);
    }
}