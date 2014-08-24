using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Sonneville.PriceTools.Data
{
    public class PriceDataProvider : IPriceDataProvider
    {
        private readonly IWebClient _webClient;
        private readonly IPriceHistoryQueryUrlBuilder _priceHistoryQueryUrlBuilder;
        private readonly IPriceHistoryCsvFileFactory _priceHistoryCsvFileFactory;

        public PriceDataProvider(IPriceHistoryQueryUrlBuilder priceHistoryQueryUrlBuilder, IPriceHistoryCsvFileFactory priceHistoryCsvFileFactory)
            : this(new WebClientWrapper(), priceHistoryQueryUrlBuilder, priceHistoryCsvFileFactory)
        {
        }

        public PriceDataProvider(IWebClient webClient, IPriceHistoryQueryUrlBuilder priceHistoryQueryUrlBuilder, IPriceHistoryCsvFileFactory priceHistoryCsvFileFactory)
        {
            _webClient = webClient;
            _priceHistoryQueryUrlBuilder = priceHistoryQueryUrlBuilder;
            _priceHistoryCsvFileFactory = priceHistoryCsvFileFactory;
        }

        public void UpdatePriceSeries(IPriceSeries priceSeries, DateTime head, DateTime tail, Resolution resolution)
        {
            var pricePeriods = DownloadPricePeriods(priceSeries.Ticker, head, tail, resolution);
            priceSeries.AddPriceData(pricePeriods);
        }

        public IList<IPricePeriod> DownloadPricePeriods(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            using (var stream = DownloadPricesToCsv(ticker, head, tail, resolution))
            {
                var priceHistoryCsvFile = _priceHistoryCsvFileFactory.CreatePriceHistoryCsvFile(stream, head, tail, resolution);
                return priceHistoryCsvFile.PricePeriods;
            }
        }

        public Resolution BestResolution
        {
            get { return Resolution.Days; }
        }

        private Stream DownloadPricesToCsv(string ticker, DateTime head, DateTime tail, Resolution resolution)
        {
            try
            {
                var url = _priceHistoryQueryUrlBuilder.FormPriceHistoryQueryUrl(ticker, head, tail, resolution);
                return _webClient.OpenRead(url);
            }
            catch (WebException e)
            {
                throw new WebException(Strings.DownloadPricesToCsv_InternetAccessFailed, e, e.Status, e.Response);
            }
        }
    }
}