using System;
using System.Text;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools.Google
{
    public sealed class GooglePriceHistoryQueryUrlBuilder : IPriceHistoryQueryUrlBuilder
    {
        public string GetUrlBase()
        {
            return "http://www.google.com/finance/historical?";
        }

        public string GetUrlTicker(string symbol)
        {
            return String.Format("q={0}&", symbol);
        }

        public string GetUrlHeadDate(DateTime head)
        {
            return String.Format("startdate={0}+{1}%2C+{2}&", TranslateMonth(head.Month), head.Day, head.Year);
        }

        public string GetUrlTailDate(DateTime tail)
        {
            return String.Format("enddate={0}+{1}%2C+{2}&", TranslateMonth(tail.Month), tail.Day, tail.Year);
        }

        public string GetUrlResolution(Resolution resolution)
        {
            return String.Format("histperiod={0}&", TranslateResolution(resolution));
        }

        public string GetUrlDividends()
        {
            throw new NotSupportedException();
        }

        public string GetUrlCsvMarker()
        {
            return "output=csv&";
        }

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