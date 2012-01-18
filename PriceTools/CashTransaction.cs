using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for an <see cref="ICashAccount"/>.
    /// </summary>
    public interface CashTransaction : ITransaction
    {
        /// <summary>
        ///   Gets the amount of cash in this CashTransaction.
        /// </summary>
        decimal Amount { get; }
    }
}
