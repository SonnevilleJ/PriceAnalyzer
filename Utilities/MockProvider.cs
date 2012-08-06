using System;
using System.Collections.Generic;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data;

namespace Sonneville.Utilities
{
    public abstract class MockProvider : PriceDataProvider
    {
        public Func<string, DateTime, DateTime, Resolution, IEnumerable<PricePeriod>> UpdateAction { get; set; }

        /// <summary>
        /// Gets a list of <see cref="PricePeriod"/>s containing price data for the requested DateTime range.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="PricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        public override IEnumerable<PricePeriod> GetPricePeriods(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            if (UpdateAction == null) throw new NotImplementedException();

            return UpdateAction(ticker, head, tail, resolution);
        }

        /// <summary>
        /// Gets a <see cref="PriceSeries"/> containing price history.
        /// </summary>
        /// <param name="ticker">The ticker symbol to price.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="PricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        public override PriceSeries GetPriceSeries(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            if (UpdateAction == null) throw new NotImplementedException();

            var periods = UpdateAction(ticker, head, tail, resolution);
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker, resolution);
            priceSeries.AddPriceData(periods);
            return priceSeries;
        }

        public override string GetIndexTicker(StockIndex index) { throw new NotImplementedException(); }
    }
}