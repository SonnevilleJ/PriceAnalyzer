using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a financial transaction.
    /// </summary>
    public abstract class Transaction
    {
        /// <summary>
        ///    Gets the DateTime that the Transaction occurred.
        ///  </summary>
        public DateTime SettlementDate { get; protected set; }
    }
}
