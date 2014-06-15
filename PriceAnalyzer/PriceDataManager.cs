using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.Google;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class PriceDataManager
    {
        private readonly IPriceDataProvider _csvPriceDataProvider;

        public PriceDataManager()
        {
            _csvPriceDataProvider = new CsvPriceDataProvider(new GooglePriceHistoryQueryUrlBuilder(), new GooglePriceHistoryCsvFileFactory());
        }

        public IList<IPricePeriod> ParseCsvFile(string filename)
        {
            var fileStream = File.OpenRead(filename);
            var priceHistoryCsvFile = new GooglePriceHistoryCsvFile(fileStream);
            return priceHistoryCsvFile.PricePeriods;
        }

        public List<IPricePeriod> DownloadPricePeriods(string ticker, DateTime startDateTime, DateTime endDateTime)
        {
            var priceSeries = new PriceSeriesFactory().ConstructPriceSeries(ticker);
            _csvPriceDataProvider.UpdatePriceSeries(priceSeries, startDateTime, endDateTime, Resolution.Days);
            return priceSeries.PricePeriods.ToList();
        }
    }
}