using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Yahoo
{
    /// <summary>
    ///   Parses an <see cref = "IPriceSeries" /> from Yahoo! CSV files.
    /// </summary>
    public sealed class YahooPriceDataProvider : CsvPriceDataProvider
    {
        public YahooPriceDataProvider()
            : this(new WebClientWrapper(), new YahooUrlManager())
        {
        }

        public YahooPriceDataProvider(IWebClient webClient, IUrlManager urlManager)
            : base(webClient, urlManager)
        {
        }

        //
        // Yahoo has many features beyond price history - i.e. fundamental indicators.
        // See http://www.codeproject.com/KB/aspnet/StockQuote.aspx for details.
        //
        // Also, a REST API is now available: http://www.jarloo.com/yahoo_finance/
        //

        /// <summary>
        /// Gets the ticker symbol for a <see cref="StockIndex"/> used by this <see cref="PriceDataProvider"/>.
        /// </summary>
        /// <param name="index">The <see cref="StockIndex"/> to retrieve.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns>A string representing the ticker symbol of the requested <see cref="StockIndex"/>.</returns>
        public override IEnumerable<IPricePeriod> GetPriceData(StockIndex index, DateTime head, DateTime tail, Resolution resolution)
        {
            return GetPriceData(GetIndexTicker(index), head, tail, resolution);
        }

        /// <summary>
        /// Creates a new instance of a <see cref="PriceHistoryCsvFile"/> that will be used by this PriceDataProvider.
        /// </summary>
        /// <param name="stream">The CSV data stream containing the price history.</param>
        /// <param name="head">The head of the price data to retrieve.</param>
        /// <param name="tail">The tail of the price data to retrieve.</param>
        /// <param name="impliedResolution">The <see cref="Resolution"/> of price data to retrieve.</param>
        /// <returns>A <see cref="PriceHistoryCsvFile"/>.</returns>
        protected override PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail, Resolution? impliedResolution = null)
        {
            return new YahooPriceHistoryCsvFile(stream, head, tail, impliedResolution);
        }

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this PriceDataProvider.
        /// </summary>
        public override Resolution BestResolution
        {
            get { return Resolution.Days; }
        }

        private static string GetIndexTicker(StockIndex index)
        {
            switch (index)
            {
                case StockIndex.StandardAndPoors500:
                    return "^GSPC";
                case StockIndex.DowJonesIndustrialAverage:
                    return "^DJI";
                case StockIndex.NasdaqCompositeIndex:
                    return "^IXIC";
                default:
                    throw new NotSupportedException(String.Format(CultureInfo.CurrentCulture, "Unknown Stock Index: {0}.", index));
            }
        }
    }
}