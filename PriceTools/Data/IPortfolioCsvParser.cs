using System;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Parses an <see cref="IPortfolio"/> from CSV data for a single investment portfolio.
    /// </summary>
    public interface IPortfolioCsvParser : IDisposable
    {
        /// <summary>
        /// Parses an <see cref="IPortfolio"/> from CSV data.
        /// </summary>
        /// <returns>An <see cref="IPortfolio"/> containing the data from the CSV data.</returns>
        IPortfolio ParsePortfolio();
    }
}