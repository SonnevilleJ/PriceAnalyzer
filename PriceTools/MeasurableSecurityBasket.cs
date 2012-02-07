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
        ///   Gets the value of any shares held the MeasurableSecurityBasket as of a given date.
        /// </summary>
        /// <param name="provider">The <see cref="IPriceDataProvider"/> to use when requesting price data.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the shares held in the MeasurableSecurityBasket as of the given date.</returns>
        decimal CalculateInvestedValue(IPriceDataProvider provider, DateTime settlementDate);
    }
}
