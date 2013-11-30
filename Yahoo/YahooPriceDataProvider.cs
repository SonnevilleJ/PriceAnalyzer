using System;
using System.IO;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Yahoo
{
    /// <summary>
    ///   Parses an <see cref = "IPriceSeries" /> from Yahoo! CSV files.
    /// </summary>
    public sealed class YahooPriceDataProvider : IPriceHistoryCsvFileFactory
    {
        //
        // Yahoo has many features beyond price history - i.e. fundamental indicators.
        // See http://www.codeproject.com/KB/aspnet/StockQuote.aspx for details.
        //
        // Also, a REST API is now available: http://www.jarloo.com/yahoo_finance/
        //

        /// <summary>
        /// Creates a new instance of a <see cref="PriceHistoryCsvFile"/> that will be used by this PriceDataProvider.
        /// </summary>
        /// <param name="stream">The CSV data stream containing the price history.</param>
        /// <param name="head">The head of the price data to retrieve.</param>
        /// <param name="tail">The tail of the price data to retrieve.</param>
        /// <param name="impliedResolution">The <see cref="Resolution"/> of price data to retrieve.</param>
        /// <returns>A <see cref="PriceHistoryCsvFile"/>.</returns>
        public PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail, Resolution? impliedResolution = null)
        {
            return new YahooPriceHistoryCsvFile(stream, head, tail, impliedResolution);
        }
    }
}