using System.Collections.Generic;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data.Csv;

namespace SampleData
{
    public class SamplePriceData : ISamplePriceData
    {
        public string Ticker { get; set; }

        public IPriceSeries PriceSeries
        {
            get
            {
                var priceSeries = new PriceSeriesFactory().ConstructPriceSeries(Ticker);
                priceSeries.AddPriceData(PricePeriods);
                return priceSeries;
            }
        }

        public IEnumerable<IPricePeriod> PricePeriods
        {
            get { return PriceHistory.PricePeriods; }
        }

        public PriceHistoryCsvFile PriceHistory { get; set; }
    }
}