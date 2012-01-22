using System;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for <see cref="MeasurableSecurityBasket"/>.
    /// </summary>
    public static class MeasurableSecurityBasketExtensions
    {
        /// <summary>
        ///   Gets the average cost of all held shares in a <see cref="IPosition"/> as of a given date.
        /// </summary>
        /// <param name="position">The <see cref="IPosition"/> for which to calculate average cost.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The average cost of all shares held at <paramref name = "settlementDate" />.</returns>
        public static decimal CalculateAverageCost(this IPosition position, DateTime settlementDate)
        {
            var transactions = position.Transactions.Cast<ShareTransaction>()
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .OrderBy(transaction => transaction.SettlementDate).ToList();
            var count = transactions.Count();

            var totalCost = 0.00m;
            var shares = 0.0;

            for (var i = 0; i < count; i++)
            {
                if (transactions[i] is Buy || transactions[i] is SellShort ||
                    transactions[i] is DividendReinvestment)
                {
                    totalCost += (transactions[i].Price*(decimal) transactions[i].Shares);
                    shares += transactions[i].Shares;
                }
                else if (transactions[i] is Sell || transactions[i] is BuyToCover)
                {
                    totalCost -= ((totalCost/(decimal) shares)*(decimal) transactions[i].Shares);
                    shares -= transactions[i].Shares;
                }
            }

            return totalCost / (decimal)shares;
        }

        /// <summary>
        ///   Gets the total rate of return on an annual basis for this MeasurableSecurityBasket.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public static decimal? CalculateAverageAnnualReturn(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var totalReturn = basket.CalculateTotalReturn(settlementDate);
            if (totalReturn == null) return null;

            var time = ((basket.Tail - basket.Head).Days / 365.0m);
            return totalReturn/time;
        }

        /// <summary>
        ///   Gets the total rate of return for this MeasurableSecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the total rate of return, after commission, expressed as a percentage. Returns null if return cannot be calculated.</returns>
        public static decimal? CalculateTotalReturn(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var proceeds = basket.CalculateProceeds(settlementDate);
            if (proceeds == 0) return null;

            var costs = basket.CalculateCost(settlementDate);
            var commissions = basket.CalculateCommissions(settlementDate);
            var profit = proceeds - costs - commissions;
            return profit/costs;
        }

        /// <summary>
        ///   Gets the raw rate of return for this MeasurableSecurityBasket, not accounting for commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the raw rate of return, before commission, expressed as a percentage. Returns null if return cannot be calculated.</returns>
        public static decimal? CalculateRawReturn(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var proceeds = basket.CalculateProceeds(settlementDate);
            if (proceeds == 0) return null;

            var costs = basket.CalculateCost(settlementDate);
            var profit = proceeds - costs;
            return profit/costs;
        }

    }
}