using System.Collections.Generic;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// A collection of historical <see cref="ITransaction"/>s.
    /// </summary>
    public interface ITransactionHistory
    {
        /// <summary>
        /// Gets a list of all <see cref="ITransaction"/>s in the file.
        /// </summary>
        IEnumerable<ITransaction> Transactions { get; }
    }
}