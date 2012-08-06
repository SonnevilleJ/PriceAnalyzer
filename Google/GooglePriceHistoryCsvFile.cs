using System;
using System.IO;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Google
{
    /// <summary>
    /// Represents a file store containing historical price data from Google Finance.
    /// </summary>
    public sealed class GooglePriceHistoryCsvFile : PriceHistoryCsvFile
    {
        /// <summary>
        /// Constructs a GooglePriceHistoryCsvFile.
        /// </summary>
        public GooglePriceHistoryCsvFile()
        {
        }

        /// <summary>
        /// Constructs a GooglePriceHistoryCsvFile from CSV data in a stream.
        /// </summary>
        /// <param name="stream">The CSV data stream to parse.</param>
        /// <param name="impliedHead">The head of the price data contained in the CSV data.</param>
        /// <param name="impliedTail">The tail of the price data contained in the CSV data.</param>
        public GooglePriceHistoryCsvFile(Stream stream, DateTime impliedHead, DateTime impliedTail)
            : base(stream, impliedHead, impliedTail)
        {
        }

        /// <summary>
        /// Constructs a GooglePriceHistoryCsvFile from CSV data in a stream.
        /// </summary>
        /// <param name="stream">The CSV data stream to parse.</param>
        public GooglePriceHistoryCsvFile(Stream stream)
            : base(stream)
        {
        }
    }
}