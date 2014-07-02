using System;
using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface ISecurityBasketCalculator
    {
        /// <summary>
        ///   Gets the gross investment of this Position, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases as a negative number.</returns>
        decimal CalculateCost(ISecurityBasket basket, DateTime settlementDate);

        /// <summary>
        ///   Gets the gross proceeds of this Position, ignoring all totalCosts and commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales as a positive number.</returns>
        decimal CalculateProceeds(ISecurityBasket basket, DateTime settlementDate);

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "ShareTransaction" />s as a negative number.</returns>
        decimal CalculateCommissions(ISecurityBasket basket, DateTime settlementDate);

        /// <summary>
        ///   Gets the value of any shares held the Position as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="provider">The <see cref="IPriceDataProvider"/> to use when requesting price data.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <param name="priceHistoryCsvFileFactory"></param>
        /// <returns>The value of the shares held in the Position as of the given date.</returns>
        decimal CalculateMarketValue(ISecurityBasket basket, IPriceDataProvider provider, DateTime settlementDate, IPriceHistoryCsvFileFactory priceHistoryCsvFileFactory);

        /// <summary>
        ///   Gets the net shares held at a given date.
        /// </summary>
        /// <param name="shareTransactions"></param>
        /// <param name = "dateTime">The <see cref = "DateTime" /> to use.</param>
        decimal GetHeldShares(IEnumerable<ShareTransaction> shareTransactions, DateTime dateTime);

        /// <summary>
        ///   Gets the net shares held at a given date.
        /// </summary>
        /// <param name="cashTransactions"></param>
        /// <param name = "dateTime">The <see cref = "DateTime" /> to use.</param>
        decimal GetHeldShares(ICollection<CashTransaction> cashTransactions, DateTime dateTime);

        /// <summary>
        ///   Gets the average cost of all held shares in a <see cref="Position"/> as of a given date.
        /// </summary>
        /// <param name="position">The <see cref="Position"/> for which to calculate average cost.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The average cost of all shares held at <paramref name = "settlementDate" />.</returns>
        decimal CalculateAverageCost(Position position, DateTime settlementDate);

        /// <summary>
        ///   Gets the net profit of this SecurityBasket, including any commissions, as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        decimal CalculateNetProfit(ISecurityBasket basket, DateTime settlementDate);

        /// <summary>
        ///   Gets the gross profit of this SecurityBasket, excluding any commissions, as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        decimal CalculateGrossProfit(ISecurityBasket basket, DateTime settlementDate);

        /// <summary>
        ///   Gets the net rate of return on an annual basis for this SecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        decimal? CalculateAnnualGrossReturn(ISecurityBasket basket, DateTime settlementDate);

        /// <summary>
        ///   Gets the net rate of return on an annual basis for this SecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        decimal? CalculateAnnualNetReturn(ISecurityBasket basket, DateTime settlementDate);

        /// <summary>
        ///   Gets the net rate of return for this SecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the net rate of return, after commission, expressed as a percentage. Returns null if return cannot be calculated.</returns>
        decimal? CalculateNetReturn(ISecurityBasket basket, DateTime settlementDate);

        /// <summary>
        ///   Gets the gross rate of return for this SecurityBasket, not accounting for commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the gross rate of return, before commission, expressed as a percentage. Returns null if return cannot be calculated.</returns>
        decimal? CalculateGrossReturn(ISecurityBasket basket, DateTime settlementDate);

        /// <summary>
        /// Calculates the mean gross profit of a basket's <see cref="Holding"/>s.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the mean gross profit from a <see cref="ISecurityBasket"/>.</returns>
        decimal CalculateAverageProfit(ISecurityBasket basket, DateTime settlementDate);

        /// <summary>
        /// Calculates the median gross profit of a basket's <see cref="Holding"/>s.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the median gross profit from a <see cref="ISecurityBasket"/>.</returns>
        decimal CalculateMedianProfit(ISecurityBasket basket, DateTime settlementDate);

        /// <summary>
        /// Calculates the standard deviation of all profits in a <see cref="ISecurityBasket"/>.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        /// <returns>Returns the standard deviation of the profits from a <see cref="ISecurityBasket"/>.</returns>
        decimal CalculateStandardDeviation(ISecurityBasket basket, DateTime settlementDate);
    }
}