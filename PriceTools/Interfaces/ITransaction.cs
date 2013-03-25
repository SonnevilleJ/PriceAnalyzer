using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a financial transaction.
    /// </summary>
    public interface ITransaction : IEquatable<ITransaction>
    {
        /// <summary>
        ///    Gets the DateTime that the Transaction occurred.
        ///  </summary>
        DateTime SettlementDate { get; }

        /// <summary>
        ///     The unique identifier of this transaction.
        /// </summary>
        Guid Id { get; }
    }
}