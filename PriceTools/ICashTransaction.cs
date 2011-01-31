using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for an <see cref="ICashAccount"/>.
    /// </summary>
    public interface ICashTransaction : ISerializable, IEquatable<ICashTransaction>
    {
        /// <summary>
        ///   Gets the DateTime that the ICashTransaction occurred.
        /// </summary>
        DateTime SettlementDate { get; }

        /// <summary>
        ///   Gets the <see cref = "PriceTools.OrderType" /> of this ICashTransaction.
        /// </summary>
        OrderType OrderType { get; }

        /// <summary>
        ///   Gets the amount of cash in this ICashTransaction.
        /// </summary>
        decimal Amount { get; }
    }
}
