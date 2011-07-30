using System;
using System.IO;

namespace Sonneville.PriceTools.Services
{
    /// <summary>
    /// Represents a file store containing historical price data from Google Finance.
    /// </summary>
    public sealed class GooglePriceHistoryCsvFile : PriceHistoryCsvFile
    {
        /// <summary>
        /// Constructs a GooglePriceHistoryCsvFile.
        /// </summary>
        /// <param name="stream">The CSV data stream to parse.</param>
        /// <param name="head">The head of the price data contained in the CSV data.</param>
        /// <param name="tail">The tail of the price data contained in the CSV data.</param>
        public GooglePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail)
            : base(stream, head, tail)
        {
        }

        /// <summary>
        /// Constructs a GooglePriceHistoryCsvFile.
        /// </summary>
        /// <param name="csvText">The raw CSV data to parse.</param>
        /// <param name="head">The head of the price data contained in the CSV data.</param>
        /// <param name="tail">The tail of the price data contained in the CSV data.</param>
        public GooglePriceHistoryCsvFile(string csvText, DateTime head, DateTime tail)
            : base(csvText, head, tail)
        {
        }
    }
}