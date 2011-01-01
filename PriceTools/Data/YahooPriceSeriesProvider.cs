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

        protected override string DateHeader { get { return "Date"; } }

        protected override string OpenHeader { get { return "Open"; } }

        protected override string HighHeader { get { return "High"; } }

        protected override string LowHeader { get { return "Low"; } }

        protected override string CloseHeader { get { return "Close"; } }

        protected override string VolumeHeader { get { return "Volume"; } }

        protected override string DividendsHeader { get { return "Dividends"; } }

        #region URL Management

        protected override string GetUrlBase()
        {
            return "http://ichart.finance.yahoo.com/table.csv?";
        }

        protected override string GetUrlTicker(string symbol)
        {
            return String.Format(CultureInfo.InvariantCulture, "s={0}", symbol);
        }

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

        protected override string GetUrlDividends()
        {
            return "g=v&";
        }

        protected override string GetUrlCsvMarker()
        {
            return "ignore=.csv&";
        }

        #endregion

        #endregion
    }
}