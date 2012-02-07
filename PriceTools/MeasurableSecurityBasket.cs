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
    }
}
