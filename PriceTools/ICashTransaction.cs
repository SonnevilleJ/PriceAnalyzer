using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for cash.
    /// </summary>
    public interface ICashTransaction : ITransaction, IEquatable<ICashTransaction>
    {
        /// <summary>
        ///   Gets the amount of cash in this CashTransaction.
        /// </summary>
        decimal Amount { get; }
    }
}