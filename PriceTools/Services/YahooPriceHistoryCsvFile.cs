using System;
using System.IO;

namespace Sonneville.PriceTools.Services
{
    /// <summary>
    /// Represents a file store containing historical price data from Yahoo! Finance.
    /// </summary>
    public sealed class YahooPriceHistoryCsvFile : PriceHistoryCsvFile
    {
        #region Constructors

        /// <summary>
        /// Constructs a YahooPriceHistoryCsvFile.
        /// </summary>
        /// <param name="stream">The CSV data stream to parse.</param>
        /// <param name="head">The head of the price data contained in the CSV data.</param>
        /// <param name="tail">The tail of the price data contained in the CSV data.</param>
        public YahooPriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail)
            : base(stream, head, tail)
        {
        }

        /// <summary>
        /// Constructs a YahooPriceHistoryCsvFile.
        /// </summary>
        /// <param name="csvText">The raw CSV data to parse.</param>
        /// <param name="head">The head of the price data contained in the CSV data.</param>
        /// <param name="tail">The tail of the price data contained in the CSV data.</param>
        public YahooPriceHistoryCsvFile(string csvText, DateTime head, DateTime tail)
            : base(csvText, head, tail)
        {
        }

        #endregion
    }
}