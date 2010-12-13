using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Parses an <see cref="IPriceSeries"/> from CSV data for a single ticker symbol.
    /// </summary>
    public interface IPriceSeriesCsvParser : IDisposable
    {
        /// <summary>
        /// Parses an <see cref="IPriceSeries"/> from CSV data.
        /// </summary>
        /// <param name="csvStream">A CSV <see cref="Stream"/> containing price data.</param>
        /// <returns>An <see cref="IPriceSeries"/> containing the price data.</returns>
        IPriceSeries ParsePriceSeries(Stream csvStream);
    }
}
