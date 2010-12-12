using System.Data;
using System.IO;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Parses an <see cref="IPortfolio"/> from CSV data for a single investment portfolio.
    /// </summary>
    public interface IPortfolioCsvParser
    {
        /// <summary>
        /// Parses an <see cref="IPortfolio"/> from CSV data.
        /// </summary>
        /// <param name="csvStream">A CSV <see cref="Stream"/> containing portfolio data.</param>
        /// <returns>An <see cref="IPortfolio"/> containing the data from the CSV data.</returns>
        IPortfolio ParsePortfolio(Stream csvStream);
    }
}