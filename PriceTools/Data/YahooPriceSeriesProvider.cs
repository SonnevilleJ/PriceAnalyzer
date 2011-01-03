using System;
using System.Globalization;
using System.Text;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    ///   Parses an <see cref = "IPriceSeries" /> from Yahoo! CSV files.
    /// </summary>
    public sealed class YahooPriceSeriesProvider : PriceSeriesProvider
    {
        #region Constructors

        internal YahooPriceSeriesProvider()
        {
        }

        #endregion

        #region IPriceSeriesProvider Implementation

        /// <summary>
        /// Gets the ticker symbol for a <see cref="StockIndex"/> used by this <see cref="PriceSeriesProvider"/>.
        /// </summary>
        /// <param name="index">The <see cref="StockIndex"/> to retrieve.</param>
        /// <returns>A string representing the ticker symbol of the requested <see cref="StockIndex"/>.</returns>
        public override string GetIndexTicker(StockIndex index)
        {
            string ticker;
            switch(index)
            {
                case StockIndex.StandardAndPoors500:
                    ticker = "^GSPC";
                    break;
                case StockIndex.DowJonesIndustrialAverage:
                    ticker = "^DJI";
                    break;
                case StockIndex.NasdaqCompositeIndex:
                    ticker = "^IXIC";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Unknown StockIndex {0}.", index));
            }
            return ticker;
        }

        /// <summary>
        /// Represents the string qualifier used in the Date column header.
        /// </summary>
        protected override string DateHeader { get { return "Date"; } }

        /// <summary>
        /// Represents the string qualifier used in the Opening Price column header.
        /// </summary>
        protected override string OpenHeader { get { return "Open"; } }

        /// <summary>
        /// Represents the string qualifier used in the High Price column header.
        /// </summary>
        protected override string HighHeader { get { return "High"; } }

        /// <summary>
        /// Represents the string qualifier used in the Low Price column header.
        /// </summary>
        protected override string LowHeader { get { return "Low"; } }

        /// <summary>
        /// Represents the string qualifier used in the Closing Price column header.
        /// </summary>
        protected override string CloseHeader { get { return "Close"; } }

        /// <summary>
        /// Represents the string qualifier used in the Volume column header.
        /// </summary>
        protected override string VolumeHeader { get { return "Volume"; } }

        /// <summary>
        /// Represents the string qualifier used in the Dividend Amount column header.
        /// </summary>
        protected override string DividendHeader { get { return "Dividends"; } }

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
            return String.Format(CultureInfo.InvariantCulture, "s={0}", symbol);
        }

        /// <summary>
        /// Gets the beginning date component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <param name="head">The first period for which to request price history.</param>
        /// <returns>A partial URL query string containing the given beginning date.</returns>
        protected override string GetUrlHeadDate(DateTime head)
        {
            string month = string.Format(CultureInfo.InvariantCulture, "a={0}&", head.Month - 1);
            string day = string.Format(CultureInfo.InvariantCulture, "b={0}&", head.Day);
            string year = string.Format(CultureInfo.InvariantCulture, "c={0}&", head.Year);

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
            string month = string.Format(CultureInfo.InvariantCulture, "d={0}&", tail.Month - 1);
            string day = string.Format(CultureInfo.InvariantCulture, "e={0}&", tail.Day);
            string year = string.Format(CultureInfo.InvariantCulture, "f={0}&", tail.Year);

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
            string result;
            switch (resolution)
            {
                case PriceSeriesResolution.Days:
                    result = "g=d&";
                    break;
                case PriceSeriesResolution.Weeks:
                    result = "g=w&";
                    break;
                case PriceSeriesResolution.Months:
                    result = "g=m&";
                    break;
                default:
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture,
                                                                  "Resolution {0} is not supported by this provider.",
                                                                  resolution));
            }
            return result;
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