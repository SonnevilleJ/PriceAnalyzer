using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Sonneville.PriceTools.Services
{
    /// <summary>
    ///   Parses an <see cref = "IPriceSeries" /> from Yahoo! CSV files.
    /// </summary>
    public sealed class YahooPriceSeriesProvider : PriceSeriesProvider
    {
        //
        // Yahoo has many features beyond price history - i.e. fundamental indicators.
        // See http://www.codeproject.com/KB/aspnet/StockQuote.aspx for details.
        //

        #region IPriceSeriesProvider Implementation

        /// <summary>
        /// Gets the ticker symbol for a <see cref="StockIndex"/> used by this <see cref="PriceSeriesProvider"/>.
        /// </summary>
        /// <param name="index">The <see cref="StockIndex"/> to retrieve.</param>
        /// <returns>A string representing the ticker symbol of the requested <see cref="StockIndex"/>.</returns>
        public override string GetIndexTicker(StockIndex index)
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

        /// <summary>
        /// Creates a new instance of a <see cref="PriceHistoryCsvFile"/> that will be used by this PriceSeriesProvider.
        /// </summary>
        /// <param name="stream">The CSV data stream containing the price history.</param>
        /// <returns>A <see cref="PriceHistoryCsvFile"/>.</returns>
        protected override PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream)
        {
            return new GenericPriceHistoryCsvFile(stream);
        }

        public override PriceSeriesResolution HighestAvailableResolution
        {
            get { return PriceSeriesResolution.Days; }
        }

        #region URL Management

        /// <summary>
        /// Gets the base component of the URL used to retrieve the price history.
        /// </summary>
        /// <returns>A URL scheme, host, path, and miscellaneous query string.</returns>
        protected override string GetUrlBase()
        {
            return "http://ichart.finance.yahoo.com/table.csv?";
        }

        /// <summary>
        /// Gets the ticker symbol component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <param name="symbol">The ticker symbol to retrieve.</param>
        /// <returns>A partial URL query string containing the given ticker symbol.</returns>
        protected override string GetUrlTicker(string symbol)
        {
            return String.Format(CultureInfo.InvariantCulture, "s={0}&", symbol);
        }

        /// <summary>
        /// Gets the beginning date component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <param name="head">The first period for which to request price history.</param>
        /// <returns>A partial URL query string containing the given beginning date.</returns>
        protected override string GetUrlHeadDate(DateTime head)
        {
            string month = String.Format(CultureInfo.InvariantCulture, "a={0:00}&", head.Month - 1);
            string day = String.Format(CultureInfo.InvariantCulture, "b={0}&", head.Day);
            string year = String.Format(CultureInfo.InvariantCulture, "c={0}&", head.Year);

            StringBuilder builder = new StringBuilder(3);
            builder.Append(month);
            builder.Append(day);
            builder.Append(year);
            return builder.ToString();
        }

        /// <summary>
        /// Gets the ending date component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <param name="tail">The last period for which to request price history.</param>
        /// <returns>A partial URL query string containing the given ending date.</returns>
        protected override string GetUrlTailDate(DateTime tail)
        {
            string month = String.Format(CultureInfo.InvariantCulture, "d={0:00}&", tail.Month - 1);
            string day = String.Format(CultureInfo.InvariantCulture, "e={0}&", tail.Day);
            string year = String.Format(CultureInfo.InvariantCulture, "f={0}&", tail.Year);

            StringBuilder builder = new StringBuilder(3);
            builder.Append(month);
            builder.Append(day);
            builder.Append(year);
            return builder.ToString();
        }

        /// <summary>
        /// Gets the <see cref="PriceSeriesResolution"/> component of the URL query string used to retrieve price history.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceSeriesResolution"/> to request.</param>
        /// <returns>A partial URL query string containing a marker which requests the given <see cref="PriceSeriesResolution"/>.</returns>
        protected override string GetUrlResolution(PriceSeriesResolution resolution)
        {
            switch (resolution)
            {
                case PriceSeriesResolution.Days:
                    return "g=d&";
                case PriceSeriesResolution.Weeks:
                    return "g=w&";
                case PriceSeriesResolution.Months:
                    return "g=m&";
                default:
                    throw new NotSupportedException(String.Format(CultureInfo.CurrentCulture,
                                                                  "Resolution {0} is not supported by this provider.",
                                                                  resolution));
            }
        }

        /// <summary>
        /// Gets the dividend component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <returns>A partial URL query string containing a marker which requests dividend data.</returns>
        protected override string GetUrlDividends()
        {
            return "g=v&";
        }

        /// <summary>
        /// Gets the CSV marker component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <returns>A partial URL qery string containing a marker which requests CSV data.</returns>
        protected override string GetUrlCsvMarker()
        {
            return "ignore=.csv&";
        }

        #endregion

        #endregion
    }
}