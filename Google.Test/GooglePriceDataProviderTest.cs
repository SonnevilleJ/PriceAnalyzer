using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.Google;

namespace Test.Sonneville.PriceTools.Google
{
    [TestClass]
    public class GooglePriceDataProviderTest
    {
        private IPriceSeriesFactory _priceSeriesFactory;
        private IPriceSeries _priceSeries;
        private IPriceDataProvider _provider;
        private IPriceHistoryCsvFileFactory _priceHistoryCsvFileFactory;

        [TestInitialize]
        public void Initialize()
        {
            _priceSeriesFactory = new PriceSeriesFactory();
            _priceSeries = _priceSeriesFactory.ConstructPriceSeries("DE");
            _priceHistoryCsvFileFactory = new GooglePriceDataProvider();
            _provider = new CsvPriceDataProvider(new WebClientWrapper(), new GooglePriceHistoryQueryUrlBuilder());
        }

        [TestMethod]
        public void DailyDownloadSingleDay()
        {
            var head = new DateTime(2012, 8, 7);
            var tail = new DateTime(2012, 8, 7).CurrentPeriodClose(Resolution.Days);

            DownloadPeriodsTest(head, tail, 1, Resolution.Days);
        }

        [TestMethod]
        public void DailyDownloadResolution()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var minTimeSpan = new TimeSpan(0, 0, 1);
            var maxTimeSpan = new TimeSpan(24, 0, 0);

            DownloadResolutionTest(head, tail, minTimeSpan, maxTimeSpan, Resolution.Days);
        }

        [TestMethod]
        public void DailyDownloadPeriods()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);

            DownloadPeriodsTest(head, tail, 50, Resolution.Days);
        }

        [TestMethod]
        public void DailyDownloadDates()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);

            DownloadDatesTest(head, tail, Resolution.Days);
        }

        [TestMethod]
        [Ignore]
        public void WeeklyDownloadPeriods()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);

            DownloadPeriodsTest(head, tail, 11, Resolution.Weeks);
        }

        [TestMethod]
        [Ignore]
        public void WeeklyDownloadResolution()
        {
            _priceSeries = _priceSeriesFactory.ConstructPriceSeries("DE", Resolution.Weeks);
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var minTimeSpan = new TimeSpan(1, 0, 0, 0);
            var maxTimeSpan = new TimeSpan(7, 0, 0, 0);

            DownloadResolutionTest(head, tail, minTimeSpan, maxTimeSpan, Resolution.Weeks);
        }

        [TestMethod]
        [Ignore]
        public void WeeklyDownloadDates()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);

            DownloadDatesTest(head, tail, Resolution.Weeks);
        }

        private void DownloadPeriodsTest(DateTime head, DateTime tail, int expected, Resolution resolution)
        {
            _provider.UpdatePriceSeries(_priceSeries, head, tail, resolution, _priceHistoryCsvFileFactory);
            Assert.AreEqual(expected, _priceSeries.PricePeriods.Count());
        }

        private void DownloadResolutionTest(DateTime head, DateTime tail, TimeSpan minTimeSpan, TimeSpan maxTimeSpan, Resolution resolution)
        {
            _provider.UpdatePriceSeries(_priceSeries, head, tail, resolution, _priceHistoryCsvFileFactory);

            Assert.AreEqual(resolution, _priceSeries.Resolution);
            var periods = _priceSeries.PricePeriods.ToArray();
            for (var i = 1; i < periods.Count() - 1; i++) // skip check on first and last periods
            {
                Assert.IsTrue(periods[i].Tail - periods[i].Head >= minTimeSpan);
                Assert.IsTrue(periods[i].Tail - periods[i].Head < maxTimeSpan);
            }
        }

        private void DownloadDatesTest(DateTime head, DateTime tail, Resolution resolution)
        {
            _provider.UpdatePriceSeries(_priceSeries, head, tail, resolution, _priceHistoryCsvFileFactory);

            Assert.AreEqual(head, _priceSeries.Head);
            Assert.AreEqual(tail, _priceSeries.Tail);
        }
    }
}
