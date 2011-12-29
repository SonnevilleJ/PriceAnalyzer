﻿using System;
using System.IO;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Services;

namespace Sonneville.Utilities
{
    public abstract class MockProvider : PriceDataProvider
    {
        protected override void UpdatePriceSeries(IPriceSeries priceSeries, DateTime head, DateTime tail)
        {
            if (UpdateAction == null) throw new NotImplementedException();

            UpdateAction(priceSeries, head, tail);
        }

        public Action<IPriceSeries, DateTime, DateTime> UpdateAction { get; set; }

        #region Not Implemented
        public override string GetIndexTicker(StockIndex index) { throw new NotImplementedException(); }
        protected override string GetUrlBase() { throw new NotImplementedException(); }
        protected override string GetUrlTicker(string symbol) { throw new NotImplementedException(); }
        protected override string GetUrlHeadDate(DateTime head) { throw new NotImplementedException(); }
        protected override string GetUrlTailDate(DateTime tail) { throw new NotImplementedException(); }
        protected override string GetUrlResolution(Resolution resolution) { throw new NotImplementedException(); }
        protected override string GetUrlDividends() { throw new NotImplementedException(); }
        protected override string GetUrlCsvMarker() { throw new NotImplementedException(); }
        protected override PriceHistoryCsvFile CreatePriceHistoryCsvFile(string ticker, Stream stream, DateTime head, DateTime tail) { throw new NotImplementedException(); }
        #endregion
    }
}