using System;

namespace Sonneville.PriceTools.Data.Csv
{
    public abstract class PriceHistoryQueryUrlBuilder : IPriceHistoryQueryUrlBuilder
    {
        /// <summary>
        /// Formulates a URL that when queried returns a CSV data stream containing the requested price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to request.</param>
        /// <param name="head">The first date to request.</param>
        /// <param name="tail">The last date to request.</param>
        /// <param name="resolution"></param>
        /// <returns>A fully formed URL.</returns>
        public abstract string FormPriceHistoryQueryUrl(string ticker, DateTime head, DateTime tail, Resolution resolution);
    }
}