using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a financial transaction or trade.
    /// </summary>
    public interface ITransaction : ISerializable, IEquatable<ITransaction>
    {
        /// <summary>
        ///   Gets the DateTime that the ITransaction occurred.
        /// </summary>
        DateTime SettlementDate { get; }

        /// <summary>
        ///   Gets the <see cref = "OrderType" /> of this ITransaction.
        /// </summary>
        OrderType OrderType { get; }

        /// <summary>
        ///   Gets the ticker symbol of the security traded in this ITransaction.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        ///   Gets the amount of securities traded in this ITransaction.
        /// </summary>
        double Shares { get; }

        /// <summary>
        ///   Gets the value of all securities traded in this ITransaction.
        /// </summary>
        decimal Price { get; }

        /// <summary>
        ///   Gets the commission charged for this ITransaction.
        /// </summary>
        decimal Commission { get; }
    }
}