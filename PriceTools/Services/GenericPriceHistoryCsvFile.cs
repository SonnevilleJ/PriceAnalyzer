using System.IO;

namespace Sonneville.PriceTools.Services
{
    /// <summary>
    /// Represents a file store containing historical price data from Yahoo! Finance.
    /// </summary>
    public sealed class GenericPriceHistoryCsvFile : PriceHistoryCsvFile
    {
        #region Constructors

        /// <summary>
        /// Constructs a GenericPriceHistoryCsvFile.
        /// </summary>
        /// <param name="stream">The CSV data stream to parse.</param>
        public GenericPriceHistoryCsvFile(Stream stream) : base(stream)
        {
        }

        /// <summary>
        /// Constructs a GenericPriceHistoryCsvFile.
        /// </summary>
        /// <param name="csvText">The raw CSV data to parse.</param>
        public GenericPriceHistoryCsvFile(string csvText) : base(csvText)
        {
        }

        #endregion

        #region Overrides of PriceHistoryCsvFile

        /// <summary>
        /// Parses the column headers of a PriceHistoryCsvFile.
        /// </summary>
        /// <param name="header">A column header from the CSV file.</param>
        /// <returns>The <see cref="PriceColumn"/> of <paramref name="header"/>.</returns>
        protected override PriceColumn ParseColumnHeader(string header)
        {
            switch (header.ToLowerInvariant())
            {
                case "date":
                    return PriceColumn.Date;
                case "open":
                    return PriceColumn.Open;
                case "high":
                    return PriceColumn.High;
                case "low":
                    return PriceColumn.Low;
                case "close":
                    return PriceColumn.Close;
                case "volume":
                    return PriceColumn.Volume;
                default:
                    return PriceColumn.None;
            }
        }

        #endregion
    }
}