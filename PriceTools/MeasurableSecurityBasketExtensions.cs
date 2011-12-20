using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for <see cref="IMeasurableSecurityBasket"/>.
    /// </summary>
    public static class MeasurableSecurityBasketExtensions
    {
        /// <summary>
        ///   Gets the total rate of return on an annual basis for this IMeasurableSecurityBasket.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public static decimal? CalculateAverageAnnualReturn(this IMeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var totalReturn = basket.CalculateTotalReturn(settlementDate);
            if (totalReturn == null) return null;

            var time = ((basket.Tail - basket.Head).Days / 365.0m);
            return totalReturn/time;
        }

        /// <summary>
        ///   Gets the total rate of return for this IMeasurableSecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the total rate of return, after commission, expressed as a percentage. Returns null if return cannot be calculated.</returns>
        public static decimal? CalculateTotalReturn(this IMeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var proceeds = basket.CalculateProceeds(settlementDate);
            if (proceeds == 0) return null;

            var costs = basket.CalculateCost(settlementDate);
            var commissions = basket.CalculateCommissions(settlementDate);
            var profit = proceeds - costs - commissions;
            return profit/costs;
        }

        /// <summary>
        ///   Gets the raw rate of return for this IMeasurableSecurityBasket, not accounting for commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the raw rate of return, before commission, expressed as a percentage. Returns null if return cannot be calculated.</returns>
        public static decimal? CalculateRawReturn(this IMeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var proceeds = basket.CalculateProceeds(settlementDate);
            if (proceeds == 0) return null;

            var costs = basket.CalculateCost(settlementDate);
            var profit = proceeds - costs;
            return profit/costs;
        }

    }
}