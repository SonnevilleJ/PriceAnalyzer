using System;
using System.Globalization;
using System.Text;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools.Yahoo
{
    public sealed class YahooPriceHistoryQueryUrlBuilder : IPriceHistoryQueryUrlBuilder
    {
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

        private string GetUrlBase()
        {
            return "http://ichart.finance.yahoo.com/table.csv?";
        }

        private string GetUrlTicker(string symbol)
        {
            return String.Format(CultureInfo.InvariantCulture, "s={0}&", symbol);
        }

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

        private string GetUrlDividends()
        {
            return "g=v&";
        }

        private string GetUrlCsvMarker()
        {
            return "ignore=.csv&";
        }
    }
}