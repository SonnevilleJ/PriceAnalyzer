using System;
using System.Globalization;
using System.Text;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools.Yahoo
{
    public sealed class YahooPriceHistoryQueryUrlBuilder : IPriceHistoryQueryUrlBuilder
    {
        /// <summary>
        /// Formulates a URL that when queried returns a CSV data stream containing the requested price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to request.</param>
        /// <param name="head">The first date to request.</param>
        /// <param name="tail">The last date to request.</param>
        /// <param name="resolution"></param>
        /// <returns>A fully formed URL.</returns>
        public string FormPriceHistoryQueryUrl(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            var builder = new StringBuilder();
            builder.Append(GetUrlBase());
            builder.Append(GetUrlTicker(ticker));
            builder.Append(GetUrlHeadDate(head));
            builder.Append(GetUrlTailDate(tail));
            builder.Append(GetUrlResolution(resolution));
            builder.Append(GetUrlCsvMarker());

            return builder.ToString();
        }

        /// <summary>
        /// Gets the base component of the URL used to retrieve the price history.
        /// </summary>
        /// <returns>A URL scheme, host, path, and miscellaneous query string.</returns>
        private string GetUrlBase()
        {
            return "http://ichart.finance.yahoo.com/table.csv?";
        }

        /// <summary>
        /// Gets the ticker symbol component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <param name="symbol">The ticker symbol to retrieve.</param>
        /// <returns>A partial URL query string containing the given ticker symbol.</returns>
        private string GetUrlTicker(string symbol)
        {
            return String.Format(CultureInfo.InvariantCulture, "s={0}&", symbol);
        }

        /// <summary>
        /// Gets the beginning date component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <param name="head">The first period for which to request price history.</param>
        /// <returns>A partial URL query string containing the given beginning date.</returns>
        private string GetUrlHeadDate(DateTime head)
        {
            var month = String.Format(CultureInfo.InvariantCulture, "a={0:00}&", head.Month - 1);
            var day = String.Format(CultureInfo.InvariantCulture, "b={0}&", head.Day);
            var year = String.Format(CultureInfo.InvariantCulture, "c={0}&", head.Year);

            var builder = new StringBuilder(3);
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
        private string GetUrlTailDate(DateTime tail)
        {
            var month = String.Format(CultureInfo.InvariantCulture, "d={0:00}&", tail.Month - 1);
            var day = String.Format(CultureInfo.InvariantCulture, "e={0}&", tail.Day);
            var year = String.Format(CultureInfo.InvariantCulture, "f={0}&", tail.Year);

            var builder = new StringBuilder(3);
            builder.Append(month);
            builder.Append(day);
            builder.Append(year);
            return builder.ToString();
        }

        /// <summary>
        /// Gets the <see cref="Resolution"/> component of the URL query string used to retrieve price history.
        /// </summary>
        /// <param name="resolution">The <see cref="Resolution"/> to request.</param>
        /// <returns>A partial URL query string containing a marker which requests the given <see cref="Resolution"/>.</returns>
        private string GetUrlResolution(Resolution resolution)
        {
            switch (resolution)
            {
                case Resolution.Days:
                    return "g=d&";
                case Resolution.Weeks:
                    return "g=w&";
                case Resolution.Months:
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
        private string GetUrlDividends()
        {
            return "g=v&";
        }

        /// <summary>
        /// Gets the CSV marker component of the URL query string used to retrieve the price history.
        /// </summary>
        /// <returns>A partial URL qery string containing a marker which requests CSV data.</returns>
        private string GetUrlCsvMarker()
        {
            return "ignore=.csv&";
        }
    }
}