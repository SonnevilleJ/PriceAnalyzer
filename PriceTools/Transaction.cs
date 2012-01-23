using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a financial transaction.
    /// </summary>
    public interface Transaction
    {
        /// <summary>
        ///   Gets the DateTime that the Transaction occurred.
        /// </summary>
        DateTime SettlementDate { get; }
    }
}
