using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.Google;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class PriceDataManager
    {
        public IList<IPricePeriod> ParseCsvFile(string filename)
        {
            var fileStream = File.OpenRead(filename);
            var priceHistoryCsvFile = new GooglePriceHistoryCsvFile(fileStream);
            return priceHistoryCsvFile.PricePeriods;
        }

        public List<IPricePeriod> DownloadPricePeriods(string ticker)
        {
            var priceSeries = new PriceSeriesFactory().ConstructPriceSeries(ticker);
            UpdatePriceSeriesWithLatestData(priceSeries);
            return priceSeries.PricePeriods.ToList();
        }

        private static void UpdatePriceSeriesWithLatestData(IPriceSeries priceSeries)
        {
            new CsvPriceDataProvider(new GooglePriceHistoryQueryUrlBuilder()).UpdatePriceSeries(priceSeries,
                new DateTime(2014, 1, 1), DateTime.Today, Resolution.Days, new GooglePriceDataProvider());
        }
    }
}