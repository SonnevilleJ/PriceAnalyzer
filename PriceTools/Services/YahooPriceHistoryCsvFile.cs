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
        public YahooPriceHistoryCsvFile(Stream stream) : base(stream)
        {
        }

        /// <summary>
        /// Constructs a YahooPriceHistoryCsvFile.
        /// </summary>
        /// <param name="csvText">The raw CSV data to parse.</param>
        public YahooPriceHistoryCsvFile(string csvText) : base(csvText)
        {
        }

        #endregion
    }
}