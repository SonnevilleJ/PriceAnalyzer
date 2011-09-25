using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a measurable basket of securities whose value changes over time.
    /// </summary>
    public interface IMeasurableSecurityBasket : ITimeSeries
    {
        /// <summary>
        ///   Gets an enumeration of all <see cref = "ShareTransaction" />s in this IMeasurableSecurityBasket.
        /// </summary>
        IList<ITransaction> Transactions { get; }

        /// <summary>
        ///   Gets the value of the IMeasurableSecurityBasket as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the IMeasurableSecurityBasket as of the given date.</returns>
        decimal CalculateValue(DateTime settlementDate);

        /// <summary>
        ///   Gets the raw rate of return for this IMeasurableSecurityBasket, not accounting for commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        decimal? CalculateRawReturn(DateTime settlementDate);

        /// <summary>
        ///   Gets the total rate of return for this IMeasurableSecurityBasket, after commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        decimal? CalculateTotalReturn(DateTime settlementDate);

        /// <summary>
        ///   Gets the total rate of return on an annual basis for this IMeasurableSecurityBasket.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        decimal? CalculateAverageAnnualReturn(DateTime settlementDate);

        /// <summary>
        ///   Gets the gross investment of this IMeasurableSecurityBasket, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases.</returns>
        decimal CalculateCost(DateTime settlementDate);

        /// <summary>
        ///   Gets the gross proceeds of this IMeasurableSecurityBasket, ignoring all costs and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales.</returns>
        decimal CalculateProceeds(DateTime settlementDate);

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "IShareTransaction" />s.</returns>
        decimal CalculateCommissions(DateTime settlementDate);

        /// <summary>
        ///   Gets the value of any shares held the IMeasurableSecurityBasket as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the shares held in the IMeasurableSecurityBasket as of the given date.</returns>
        decimal CalculateInvestedValue(DateTime settlementDate);
    }
}
