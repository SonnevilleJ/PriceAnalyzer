using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a transaction (or order) for a financial security.
    /// </summary>
    public interface IShareTransaction : ITransaction
    {
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