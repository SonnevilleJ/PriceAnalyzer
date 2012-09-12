using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a basket of securities whose value changes over time.
    /// </summary>
    public interface SecurityBasket : TimePeriod
    {
        /// <summary>
        ///   Gets an enumeration of all <see cref = "ShareTransaction" />s in this SecurityBasket.
        /// </summary>
        IList<Transaction> Transactions { get; }
    }
}
