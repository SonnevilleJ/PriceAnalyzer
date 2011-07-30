using System;
using System.IO;

namespace Sonneville.PriceTools.Services
{
    /// <summary>
    /// Parses an <see cref = "IPriceSeries" /> from Google Finance CSV files.
    /// </summary>
    public sealed class GooglePriceSeriesProvider : PriceSeriesProvider
    {
        #region Overrides of PriceSeriesProvider

        /// <summary>
        /// Gets the ticker symbol for a given stock index.
        /// </summary>
        /// <param name="index">The stock index to lookup.</param>
        /// <returns>The ticker symbol of <paramref name="index"/> for this PriceSeriesProvider.</returns>
        public override string GetIndexTicker(StockIndex index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the base component of the URL used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <returns>A URL scheme, host, path, and miscellaneous query string.</returns>
        protected override string GetUrlBase()
        {
            return "http://www.google.com/finance/historical?";
        }

        /// <summary>
        /// Gets the ticker symbol component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <param name="symbol">The ticker symbol to retrieve.</param>
        /// <returns>A partial URL query string containing the given ticker symbol.</returns>
        protected override string GetUrlTicker(string symbol)
        {
            return String.Format("q=\"{0}\"&", symbol);
        }

        /// <summary>
        /// Gets the beginning date component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <param name="head">The first period for which to request price history.</param>
        /// <returns>A partial URL query string containing the given beginning date.</returns>
        protected override string GetUrlHeadDate(DateTime head)
        {
            return String.Format("startdate={0}+{1}%2C+{2}&", TranslateMonth(head.Month), head.Day, head.Year);
        }

        /// <summary>
        /// Gets the ending date component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <param name="tail">The last period for which to request price history.</param>
        /// <returns>A partial URL query string containing the given ending date.</returns>
        protected override string GetUrlTailDate(DateTime tail)
        {
            int day = tail.Day - 1; // Google Finance returns an extra day of price history for some reason
            return String.Format("enddate={0}+{1}%2C+{2}&", TranslateMonth(tail.Month), day, tail.Year);
        }

        /// <summary>
        /// Gets the <see cref="PriceSeriesResolution"/> component of the URL query string used to retrieve price history.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceSeriesResolution"/> to request.</param>
        /// <returns>A partial URL query string containing a marker which requests the given <see cref="PriceSeriesResolution"/>.</returns>
        protected override string GetUrlResolution(PriceSeriesResolution resolution)
        {
            return String.Format("histperiod={0}&", TranslateResolution(resolution));
        }

        /// <summary>
        /// Gets the dividend component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <returns>A partial URL query string containing a marker which requests dividend data.</returns>
        protected override string GetUrlDividends()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the CSV marker component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <returns>A partial URL qery string containing a marker which requests CSV data.</returns>
        protected override string GetUrlCsvMarker()
        {
            return "output=csv&";
        }

        /// <summary>
        /// Creates a new instance of a <see cref="PriceHistoryCsvFile"/> that will be used by this PriceSeriesProvider.
        /// </summary>
        /// <param name="stream">The CSV data stream containing the price history.</param>
        /// <returns>A <see cref="PriceHistoryCsvFile"/>.</returns>
        protected override PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream)
        {
            return new GooglePriceHistoryCsvFile(stream);
        }

        /// <summary>
        /// Gets the smallest <see cref="PriceSeriesResolution"/> available from this PriceSeriesProvider.
        /// </summary>
        public override PriceSeriesResolution BestResolution
        {
            get { return PriceSeriesResolution.Days; }
        }

        #endregion

        private static string TranslateMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "jan";
                case 2:
                    return "feb";
                case 3:
                    return "mar";
                case 4:
                    return "apr";
                case 5:
                    return "may";
                case 6:
                    return "jun";
                case 7:
                    return "jul";
                case 8:
                    return "aug";
                case 9:
                    return "sep";
                case 10:
                    return "oct";
                case 11:
                    return "nov";
                case 12:
                    return "dec";
                default:
                    throw new ArgumentOutOfRangeException("month");
            }
        }

        private static string TranslateResolution(PriceSeriesResolution resolution)
        {
            switch (resolution)
            {
                case PriceSeriesResolution.Days:
                    return "daily";
                case PriceSeriesResolution.Weeks:
                    return "weekly";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}