using System;
using System.Collections.Generic;
using System.IO;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Google;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class MainFormViewModel
    {
        private const Resolution DefaultResolution = Resolution.Days;
        private readonly IPriceDataProvider _priceDataProvider;
        private string _fileName;

        public MainFormViewModel()
        {
            _priceDataProvider = new PriceDataProvider(new GooglePriceHistoryQueryUrlBuilder(), new GooglePriceHistoryCsvFileFactory());
        }

        public IList<IPricePeriod> Download(string ticker, DateTime startDateTime, DateTime endDateTime)
        {
            return _priceDataProvider.DownloadPricePeriods(ticker, startDateTime, endDateTime, DefaultResolution);
        }

        private IList<IPricePeriod> ParseCsvFile(string filename)
        {
            var fileStream = File.OpenRead(filename);
            var priceHistoryCsvFile = new GooglePriceHistoryCsvFile(fileStream);
            return priceHistoryCsvFile.PricePeriods;
        }

        public IList<IPricePeriod> OpenFile(string fullFileName)
        {
            _fileName = fullFileName;
            return ParseCsvFile(_fileName);
        }

        public string Ticker
        {
            get
            {
                if (_fileName == null)
                {
                    return "";
                }

                var fileInfo = new FileInfo(_fileName);
                var fileName = fileInfo.Name;
                return fileName.Substring(0, fileName.IndexOf("."));
            }
        }
    }
}
