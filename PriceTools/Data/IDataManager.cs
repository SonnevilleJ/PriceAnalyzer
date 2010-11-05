using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// An IDataManager interacts with a database containing Price objects.
    /// </summary>
    public interface IDataManager
    {
        /// <summary>
        /// Queries the data source for the per-share price of a security at a certain date.
        /// </summary>
        /// <param name="ticker">The ticker symbol of the security to price.</param>
        /// <param name="date">The historical date for which the returned price should be valid.</param>
        /// <returns>A <see cref="decimal"/> representing the per-share price of the security.</returns>
        decimal GetPrice(string ticker, DateTime date);

        /// <summary>
        /// Queries the data source for all transactions for a given date range.
        /// </summary>
        /// <param name="head">The first date of the date range.</param>
        /// <param name="tail">The last date of the date range.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ITransaction"/> objects.</returns>
        List<ITransaction> GetTransactions(DateTime head, DateTime tail);

        /// <summary>
        /// Queries the data source for all transactions for a given security and date range.
        /// </summary>
        /// <param name="ticker">The ticker of the security.</param>
        /// <param name="head">The first date of the date range.</param>
        /// <param name="tail">The last date of the date range.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ITransaction"/> objects.</returns>
        List<ITransaction> GetTransactions(string ticker, DateTime head, DateTime tail);

        /// <summary>
        /// Queries the data source for all transactions of a given <see cref="OrderType"/> and date range.
        /// </summary>
        /// <param name="type">The <see cref="OrderType"/> of transactions to query.</param>
        /// <param name="head">The first date of the date range.</param>
        /// <param name="tail">The last date of the date range.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ITransaction"/> objects.</returns>
        List<ITransaction> GetTransactions(OrderType type, DateTime head, DateTime tail);

        /// <summary>
        /// Queries the data source for all transactions of a given <see cref="OrderType"/>, ticker, and date range.
        /// </summary>
        /// <param name="type">The <see cref="OrderType"/> of transactions to query.</param>
        /// <param name="ticker">The ticker of the security.</param>
        /// <param name="head">The first date of the date range.</param>
        /// <param name="tail">The last date of the date range.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ITransaction"/> objects.</returns>
        List<ITransaction> GetTransactions(OrderType type, string ticker, DateTime head, DateTime tail);

        /// <summary>
        /// Stores a <see cref="List{T}"/> of ITransactions to the data source.
        /// </summary>
        /// <param name="transactions">The <see cref="List{T}"/> of ITransactions to store in the data source.</param>
        void StoreTransactions(List<ITransaction> transactions);
    }
}