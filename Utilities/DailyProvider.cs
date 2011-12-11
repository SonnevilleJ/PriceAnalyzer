using System;
using System.IO;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Services;

namespace Sonneville.Utilities
{
    public class DailyProvider : PriceSeriesProvider
    {
        public override Resolution BestResolution { get { return Resolution.Days; } }
        #region Not Implemented
        public override string GetIndexTicker(StockIndex index) { throw new NotImplementedException(); }
        protected override string GetUrlBase() { throw new NotImplementedException(); }
        protected override string GetUrlTicker(string symbol) { throw new NotImplementedException(); }
        protected override string GetUrlHeadDate(DateTime head) { throw new NotImplementedException(); }
        protected override string GetUrlTailDate(DateTime tail) { throw new NotImplementedException(); }
        protected override string GetUrlResolution(Resolution resolution) { throw new NotImplementedException(); }
        protected override string GetUrlDividends() { throw new NotImplementedException(); }
        protected override string GetUrlCsvMarker() { throw new NotImplementedException(); }
        protected override PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail) { throw new NotImplementedException(); }
        #endregion
    }
}