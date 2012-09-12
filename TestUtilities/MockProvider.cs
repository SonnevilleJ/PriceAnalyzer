using System;
using System.Collections.Generic;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data;

namespace TestUtilities.Sonneville.PriceTools
{
    public abstract class MockProvider : PriceDataProvider
    {
        public Func<string, DateTime, DateTime, Resolution, IEnumerable<IPricePeriod>> UpdateAction { get; set; }

        /// <summary>
        /// Gets a list of <see cref="IPricePeriod"/>s containing price data for the requested DateTime range.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        public override IEnumerable<IPricePeriod> GetPriceData(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            if (UpdateAction == null) throw new NotImplementedException();

            return UpdateAction(ticker, head, tail, resolution);
        }

        /// <summary>
        /// Gets a <see cref="IPriceSeries"/> containing price history.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> containing price history to be updated.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        public override void UpdatePriceSeries(IPriceSeries priceSeries, DateTime head, DateTime tail, Resolution resolution)
        {
            if (UpdateAction == null) throw new NotImplementedException();

            var periods = UpdateAction(priceSeries.Ticker, head, tail, resolution);
            priceSeries.AddPriceData(periods);
        }

        public override string GetIndexTicker(StockIndex index) { throw new NotImplementedException(); }
    }
}