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
        public GooglePriceHistoryCsvFile(Stream stream) : base(stream)
        {
        }

        /// <summary>
        /// Constructs a GooglePriceHistoryCsvFile.
        /// </summary>
        /// <param name="csvText">The raw CSV data to parse.</param>
        public GooglePriceHistoryCsvFile(string csvText) : base(csvText)
        {
        }
    }
}