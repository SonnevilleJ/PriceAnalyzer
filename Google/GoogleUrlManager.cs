using System;
using System.Text;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Google
{
    public sealed class GoogleUrlManager : UrlManager
    {
        /// <summary>
        /// Gets the base component of the URL used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <returns>A URL scheme, host, path, and miscellaneous query string.</returns>
        public string GetUrlBase()
        {
            return "http://www.google.com/finance/historical?";
        }

        /// <summary>
        /// Gets the ticker symbol component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <param name="symbol">The ticker symbol to retrieve.</param>
        /// <returns>A partial URL query string containing the given ticker symbol.</returns>
        public string GetUrlTicker(string symbol)
        {
            return String.Format("q={0}&", symbol);
        }

        /// <summary>
        /// Gets the beginning date component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <param name="head">The first period for which to request price history.</param>
        /// <returns>A partial URL query string containing the given beginning date.</returns>
        public string GetUrlHeadDate(DateTime head)
        {
            return String.Format("startdate={0}+{1}%2C+{2}&", TranslateMonth(head.Month), head.Day, head.Year);
        }

        /// <summary>
        /// Gets the ending date component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <param name="tail">The last period for which to request price history.</param>
        /// <returns>A partial URL query string containing the given ending date.</returns>
        public string GetUrlTailDate(DateTime tail)
        {
            return String.Format("enddate={0}+{1}%2C+{2}&", TranslateMonth(tail.Month), tail.Day, tail.Year);
        }

        /// <summary>
        /// Gets the <see cref="Resolution"/> component of the URL query string used to retrieve price history.
        /// </summary>
        /// <param name="resolution">The <see cref="Resolution"/> to request.</param>
        /// <returns>A partial URL query string containing a marker which requests the given <see cref="Resolution"/>.</returns>
        public string GetUrlResolution(Resolution resolution)
        {
            return String.Format("histperiod={0}&", TranslateResolution(resolution));
        }

        /// <summary>
        /// Gets the dividend component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <returns>A partial URL query string containing a marker which requests dividend data.</returns>
        public string GetUrlDividends()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the CSV marker component of the URL query string used to retrieve the PriceHistoryCsvFile.
        /// </summary>
        /// <returns>A partial URL qery string containing a marker which requests CSV data.</returns>
        public string GetUrlCsvMarker()
        {
            return "output=csv&";
        }

        /// <summary>
        /// Formulates a URL that when queried returns a CSV data stream containing the requested price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to request.</param>
        /// <param name="head">The first date to request.</param>
        /// <param name="tail">The last date to request.</param>
        /// <param name="resolution"></param>
        /// <returns>A fully formed URL.</returns>
        public override string FormUrlQuery(string ticker, DateTime head, DateTime tail, Resolution resolution)
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

        private static string TranslateResolution(Resolution resolution)
        {
            switch (resolution)
            {
                case Resolution.Days:
                    return "daily";
                case Resolution.Weeks:
                    return "weekly";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}