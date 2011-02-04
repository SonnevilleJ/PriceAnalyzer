using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a financial transaction.
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        ///   Gets the DateTime that the ITransaction occurred.
        /// </summary>
        DateTime SettlementDate { get; }

        /// <summary>
        ///   Gets the <see cref = "PriceTools.OrderType" /> of this ITransaction.
        /// </summary>
        OrderType OrderType { get; }
    }
}
