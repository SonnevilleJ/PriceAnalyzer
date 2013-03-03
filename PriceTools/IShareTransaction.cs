using System;

namespace Sonneville.PriceTools
{
    public interface IShareTransaction : ITransaction, IEquatable<IShareTransaction>
    {
        /// <summary>
        ///   Gets the ticker symbol of the security traded in this ShareTransaction.
        /// </summary>
        string Ticker { get; }

        /// <summary>
        ///   Gets the amount of securities traded in this ShareTransaction.
        /// </summary>
        decimal Shares { get; }

        /// <summary>
        ///   Gets the value of all securities traded in this ShareTransaction.
        /// </summary>
        decimal Price { get; }

        /// <summary>
        ///   Gets the commission charged for this ShareTransaction.
        /// </summary>
        decimal Commission { get; }

        /// <summary>
        ///   Gets the total value of this ShareTransaction, including commissions.
        /// </summary>
        decimal TotalValue { get; }

        /// <summary>
        ///    Gets the DateTime that the Transaction occurred.
        ///  </summary>
        DateTime SettlementDate { get; }
    }
}