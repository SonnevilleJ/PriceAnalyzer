using System.Collections.Generic;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// A collection of historical <see cref="Transaction"/>s.
    /// </summary>
    public interface TransactionHistory
    {
        /// <summary>
        /// Gets a list of all <see cref="Transaction"/>s in the file.
        /// </summary>
        IEnumerable<Transaction> Transactions { get; }
    }
}