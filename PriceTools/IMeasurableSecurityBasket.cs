using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a measurable basket of securities whose value changes over time.
    /// </summary>
    public interface IMeasurableSecurityBasket : ITimeSeries
    {
        /// <summary>
        ///   Gets the value of the IMeasurableSecurityBasket as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the IMeasurableSecurityBasket as of the given date.</returns>
        decimal GetValue(DateTime settlementDate);

        /// <summary>
        ///   Gets the raw rate of return for this IMeasurableSecurityBasket, not accounting for commissions.
        /// </summary>
        decimal GetRawReturn(DateTime settlementDate);

        /// <summary>
        ///   Gets the total rate of return for this IMeasurableSecurityBasket, after commissions.
        /// </summary>
        decimal GetTotalReturn(DateTime settlementDate);

        /// <summary>
        ///   Gets the total rate of return on an annual basis for this IMeasurableSecurityBasket.
        /// </summary>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        decimal GetAverageAnnualReturn(DateTime settlementDate);

        /// <summary>
        ///   Gets the gross investment of this IMeasurableSecurityBasket, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases.</returns>
        decimal GetCost(DateTime settlementDate);

        /// <summary>
        ///   Gets the gross proceeds of this IMeasurableSecurityBasket, ignoring all totalCosts and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales.</returns>
        decimal GetProceeds(DateTime settlementDate);

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "IShareTransaction" />s.</returns>
        decimal GetCommissions(DateTime settlementDate);
    }
}