using System;
using System.IO;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Yahoo
{
    /// <summary>
    /// Represents a file store containing historical price data from Yahoo! Finance.
    /// </summary>
    public sealed class YahooPriceHistoryCsvFile : PriceHistoryCsvFile
    {
        /// <summary>
        /// Constructs a YahooPriceHistoryCsvFile.
        /// </summary>
        public YahooPriceHistoryCsvFile()
        {
        }

        /// <summary>
        /// Constructs a YahooPriceHistoryCsvFile from CSV data in a stream.
        /// </summary>
        /// <param name="ticker">The ticker which should be assigned to the <see cref="PriceSeries"/>.</param>
        /// <param name="stream">The CSV data stream to parse.</param>
        /// <param name="impliedHead">The head of the price data contained in the CSV data.</param>
        /// <param name="impliedTail">The tail of the price data contained in the CSV data.</param>
        public YahooPriceHistoryCsvFile(string ticker, Stream stream, DateTime impliedHead, DateTime impliedTail)
            : base(ticker, stream, impliedHead, impliedTail)
        {
        }

        /// <summary>
        /// Constructs a YahooPriceHistoryCsvFile from CSV data in a stream.
        /// </summary>
        /// <param name="ticker">The ticker which should be assigned to the <see cref="PriceSeries"/>.</param>
        /// <param name="stream">The CSV data stream to parse.</param>
        public YahooPriceHistoryCsvFile(string ticker, Stream stream)
            : base(ticker, stream)
        {
        }
    }
}