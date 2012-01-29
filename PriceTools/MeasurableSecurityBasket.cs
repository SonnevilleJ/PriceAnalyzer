using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a measurable basket of securities whose value changes over time.
    /// </summary>
    public interface MeasurableSecurityBasket : TimeSeries
    {
        /// <summary>
        ///   Gets an enumeration of all <see cref = "ShareTransaction" />s in this MeasurableSecurityBasket.
        /// </summary>
        IList<Transaction> Transactions { get; }

        /// <summary>
        ///   Gets the value of the MeasurableSecurityBasket, excluding any commissions, as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the MeasurableSecurityBasket as of the given date.</returns>
        decimal CalculateGrossProfit(DateTime settlementDate);

        /// <summary>
        ///   Gets the gross investment of this MeasurableSecurityBasket, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases.</returns>
        decimal CalculateCost(DateTime settlementDate);

        /// <summary>
        ///   Gets the gross proceeds of this MeasurableSecurityBasket, ignoring all costs and commissions.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales.</returns>
        decimal CalculateProceeds(DateTime settlementDate);

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "ShareTransaction" />s.</returns>
        decimal CalculateCommissions(DateTime settlementDate);

        /// <summary>
        ///   Gets the value of any shares held the MeasurableSecurityBasket as of a given date.
        /// </summary>
        /// <param name="provider">The <see cref="IPriceDataProvider"/> to use when requesting price data.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the shares held in the MeasurableSecurityBasket as of the given date.</returns>
        decimal CalculateInvestedValue(IPriceDataProvider provider, DateTime settlementDate);
    }
}
