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

        public List<IPricePeriod> DownloadPricePeriods(string ticker, DateTime startDateTime, DateTime endDateTime)
        {
            var priceSeries = new PriceSeriesFactory().ConstructPriceSeries(ticker);
            UpdatePriceSeriesWithLatestData(priceSeries, endDateTime, startDateTime);
            return priceSeries.PricePeriods.ToList();
        }

        private static void UpdatePriceSeriesWithLatestData(IPriceSeries priceSeries, DateTime endDateTime, DateTime startDateTime)
        {
            new CsvPriceDataProvider(new GooglePriceHistoryQueryUrlBuilder()).UpdatePriceSeries(priceSeries,
                startDateTime, endDateTime, Resolution.Days, new GooglePriceDataProvider());
        }
    }
}