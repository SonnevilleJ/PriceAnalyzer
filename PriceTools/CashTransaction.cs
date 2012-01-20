using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for an <see cref="CashAccount"/>.
    /// </summary>
    public interface CashTransaction : Transaction
    {
        /// <summary>
        ///   Gets the amount of cash in this CashTransaction.
        /// </summary>
        decimal Amount { get; }
    }
}
