using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for <see cref="MeasurableSecurityBasket"/>.
    /// </summary>
    public static class MeasurableSecurityBasketExtensions
    {
        /// <summary>
        ///   Gets the net shares held at a given date.
        /// </summary>
        /// <param name="shareTransactions"></param>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        public static double GetHeldShares(this IEnumerable<ShareTransaction> shareTransactions, DateTime date)
        {
            return shareTransactions.GetOpenedShares(date) - shareTransactions.GetClosedShares(date);
        }

        /// <summary>
        ///   Gets the cumulative number of shares that have ever been owned before a given date.
        /// </summary>
        /// <param name="shareTransactions"></param>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        private static double GetOpenedShares(this IEnumerable<ShareTransaction> shareTransactions, DateTime date)
        {
            var transactions = shareTransactions.Where(t => t is OpeningTransaction);
            return transactions.Where(transaction => transaction.SettlementDate <= date).Sum(transaction => transaction.Shares);
        }

        /// <summary>
        ///   Gets the total number of shares that were owned but are no longer owned.
        /// </summary>
        /// <param name="shareTransactions"></param>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        private static double GetClosedShares(this IEnumerable<ShareTransaction> shareTransactions, DateTime date)
        {
            var transactions = shareTransactions.Where(t => t is ClosingTransaction);
            return transactions.Where(transaction => transaction.SettlementDate <= date).Sum(transaction => transaction.Shares);
        }

        /// <summary>
        ///   Gets the average cost of all held shares in a <see cref="Position"/> as of a given date.
        /// </summary>
        /// <param name="position">The <see cref="Position"/> for which to calculate average cost.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The average cost of all shares held at <paramref name = "settlementDate" />.</returns>
        public static decimal CalculateAverageCost(this Position position, DateTime settlementDate)
        {
            var transactions = position.Transactions.Cast<ShareTransaction>()
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .OrderBy(transaction => transaction.SettlementDate).ToList();
            var count = transactions.Count();

            var totalCost = 0.00m;
            var shares = 0.0;

            for (var i = 0; i < count; i++)
            {
                if (transactions[i] is OpeningTransaction)
                {
                    totalCost += (transactions[i].Price*(decimal) transactions[i].Shares);
                    shares += transactions[i].Shares;
                }
                else if (transactions[i] is ClosingTransaction)
                {
                    totalCost -= ((totalCost/(decimal) shares)*(decimal) transactions[i].Shares);
                    shares -= transactions[i].Shares;
                }
            }

            return totalCost / (decimal)shares;
        }

        /// <summary>
        ///   Gets the value of this Portfolio, excluding any commissions, as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        public static decimal CalculateGrossProfit(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var sum = 0.00m;

            var allHoldings = basket.CalculateHoldings(settlementDate);
            if (allHoldings.Count == 0) return 0;

            var positionGroups = allHoldings.GroupBy(h => h.Ticker);
            foreach (var holdings in positionGroups)
            {
                foreach (var holding in holdings)
                {
                    var open = holding.OpenPrice;
                    var close = holding.ClosePrice;
                    var profit = close - open;
                    var shares = (decimal) holding.Shares;
                    sum += profit;
                }
            }
            return sum;
        }

        /// <summary>
        ///   Gets the total rate of return on an annual basis for this MeasurableSecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public static decimal? CalculateAverageAnnualReturn(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var totalReturn = basket.CalculateNetReturn(settlementDate);
            if (totalReturn == null) return null;

            var time = ((basket.Tail - basket.Head).Days / 365.0m);
            return totalReturn/time;
        }

        /// <summary>
        ///   Gets the net rate of return for this MeasurableSecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the net rate of return, after commission, expressed as a percentage. Returns null if return cannot be calculated.</returns>
        public static decimal? CalculateNetReturn(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var proceeds = basket.CalculateProceeds(settlementDate);
            if (proceeds == 0) return null;

            var costs = basket.CalculateCost(settlementDate);
            var commissions = basket.CalculateCommissions(settlementDate);
            var profit = proceeds - costs - commissions;
            return profit/costs;
        }

        /// <summary>
        ///   Gets the gross rate of return for this MeasurableSecurityBasket, not accounting for commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the gross rate of return, before commission, expressed as a percentage. Returns null if return cannot be calculated.</returns>
        public static decimal? CalculateGrossReturn(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var sum = 0.00m;

            var allHoldings = basket.CalculateHoldings(settlementDate);
            if (allHoldings.Count == 0) return null;

            var totalShares = allHoldings.Sum(h => h.Shares);
            var positionGroups = allHoldings.GroupBy(h => h.Ticker);
            foreach (var holdings in positionGroups)
            {
                var positionShares = holdings.Sum(h => h.Shares);
                foreach (var holding in holdings)
                {
                    var open = holding.OpenPrice;
                    var close = holding.ClosePrice;
                    var profit = close - open;
                    var increase = profit/open;
                    sum += increase * (decimal) ((holding.Shares / positionShares) * (positionShares / totalShares));
                }
            }
            return sum;
        }

    }
}