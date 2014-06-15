using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a stock index.
    /// </summary>
    public interface IMarketIndex
    {
        /// <summary>
        /// Gets the tickers contained in this <see cref="IMarketIndex"/>.
        /// </summary>
        IList<string> GetTickers();
    }
}