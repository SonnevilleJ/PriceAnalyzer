using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Google
{
    /// <summary>
    /// Parses an <see cref = "IPriceSeries" /> from Google Finance CSV files.
    /// </summary>
    public sealed class GooglePriceDataProvider : CsvPriceDataProvider
    {
        public GooglePriceDataProvider()
            : this(new WebClientWrapper(), new GoogleUrlManager())
        {
        }

        public GooglePriceDataProvider(IWebClient webClient, IUrlManager urlManager)
            : base(webClient, urlManager)
        {
        }

        #region Overrides of PriceDataProvider

        public override IEnumerable<IPricePeriod> GetPriceData(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            return base.GetPriceData(ticker, head, tail, BestResolution).ResizePricePeriods(resolution);
        }

        /// <summary>
        /// Gets the ticker symbol for a given stock index.
        /// </summary>
        /// <param name="index">The stock index to lookup.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns>The ticker symbol of <paramref name="index"/> for this PriceDataProvider.</returns>
        public override IEnumerable<IPricePeriod> GetPriceData(StockIndex index, DateTime head, DateTime tail, Resolution resolution)
        {
            throw new NotImplementedException();
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
            return new GooglePriceHistoryCsvFile(stream, head, tail, impliedResolution);
        }

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this PriceDataProvider.
        /// </summary>
        public override Resolution BestResolution
        {
            get { return Resolution.Days; }
        }

        #endregion
    }
}