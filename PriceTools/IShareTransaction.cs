using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a transaction (or order) for a financial security.
    /// </summary>
    public interface IShareTransaction : ISerializable, IEquatable<IShareTransaction>
    {
        /// <summary>
        ///   Gets the DateTime that the IShareTransaction occurred.
        /// </summary>
        DateTime SettlementDate { get; }

        /// <summary>
        ///   Gets the <see cref = "PriceTools.OrderType" /> of this IShareTransaction.
        /// </summary>
        OrderType OrderType { get; }

        /// <summary>
        ///   Gets the ticker symbol of the security traded in this IShareTransaction.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        ///   Gets the amount of securities traded in this IShareTransaction.
        /// </summary>
        double Shares { get; }

        /// <summary>
        ///   Gets the value of all securities traded in this IShareTransaction.
        /// </summary>
        decimal Price { get; }

        /// <summary>
        ///   Gets the commission charged for this IShareTransaction.
        /// </summary>
        decimal Commission { get; }
    }
}