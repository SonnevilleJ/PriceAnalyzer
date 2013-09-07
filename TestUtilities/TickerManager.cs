using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools;

namespace TestUtilities.Sonneville.PriceTools
{
    public static class TickerManager
    {
        private static readonly IList<string> TickersUsed = new List<string>();
        private static readonly IList<string> Tickers = StockIndexInfo.GetTickers(StockIndex.StandardAndPoors500).ToList();

        /// <summary>
        /// Gets a ticker symbol guaranteed to be unique across all threads creating tickers with this method.
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueTicker()
        {
            lock (TickersUsed)
            {
                var ticker = Tickers[0];
                Tickers.Remove(ticker);
                TickersUsed.Add(ticker);
                return ticker;
            }
        }
    }
}
