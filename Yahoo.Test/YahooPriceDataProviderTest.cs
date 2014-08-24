using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.SampleData;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Yahoo.Test
{
    [TestFixture]
    public class YahooPriceDataProviderTest
    {
        private IPriceSeriesFactory _priceSeriesFactory;
        private IPriceSeries _priceSeries;
        private IPriceDataProvider _provider;
        private IPriceHistoryCsvFileFactory _priceHistoryCsvFileFactory;
        private Mock<IPriceHistoryQueryUrlBuilder> _priceHistoryQueryUrlBuilder;
        private string _ticker;

        [SetUp]
        public void Setup()
        {
            _ticker = "IBM";

            _priceSeriesFactory = new PriceSeriesFactory();
            _priceSeries = _priceSeriesFactory.ConstructPriceSeries(_ticker);
            _priceHistoryCsvFileFactory = new YahooPriceHistoryCsvFileFactory();
            var webClientMock = new Mock<IWebClient>();
            const string ibmDaily = "IBM 1-3 to 3-15 Daily";
            const string ibmWeekly = "IBM 1-3 to 3-15 Weekly";
            const string ibmSingleDay = "IBM 8-7 Single Day";
            webClientMock.Setup(x => x.OpenRead(ibmDaily))
                .Returns(new ResourceStream(SamplePriceDatas.IBM_Daily.CsvString));
            webClientMock.Setup(x => x.OpenRead(ibmWeekly))
                .Returns(new ResourceStream(SamplePriceDatas.IBM_Weekly.CsvString));
            webClientMock.Setup(x => x.OpenRead(ibmSingleDay))
                .Returns(new ResourceStream(SamplePriceDatas.IBM_SingleDay.CsvString));

            _priceHistoryQueryUrlBuilder = new Mock<IPriceHistoryQueryUrlBuilder>();
            _priceHistoryQueryUrlBuilder.Setup(x => x.FormPriceHistoryQueryUrl(_ticker, new DateTime(2011, 1, 3), new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days), Resolution.Days))
                .Returns(ibmDaily);
            _priceHistoryQueryUrlBuilder.Setup(x => x.FormPriceHistoryQueryUrl(_ticker, new DateTime(2011, 1, 3), new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days), Resolution.Weeks))
                .Returns(ibmWeekly);
            _priceHistoryQueryUrlBuilder.Setup(x => x.FormPriceHistoryQueryUrl(_ticker, new DateTime(2012, 8, 7), new DateTime(2012, 8, 7).CurrentPeriodClose(Resolution.Days), Resolution.Days))
                .Returns(ibmSingleDay);

            _provider = new PriceDataProvider(webClientMock.Object, _priceHistoryQueryUrlBuilder.Object, _priceHistoryCsvFileFactory);
        }

        [Test]
        public void DailyDownloadSingleDay()
        {
            var head = new DateTime(2012, 8, 7);
            var tail = new DateTime(2012, 8, 7).CurrentPeriodClose(Resolution.Days);

            DownloadPeriodsTest(head, tail, 1, Resolution.Days);
        }

        [Test]
        public void DailyDownloadResolution()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var minTimeSpan = new TimeSpan(0, 0, 1);
            var maxTimeSpan = new TimeSpan(24, 0, 0);

            DownloadResolutionTest(head, tail, minTimeSpan, maxTimeSpan, Resolution.Days);
        }

        [Test]
        public void DailyDownloadPeriods()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);

            DownloadPeriodsTest(head, tail, 50, Resolution.Days);
        }

        [Test]
        public void DailyDownloadDates()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);

            DownloadDatesTest(head, tail, Resolution.Days);
        }

        [Test]
        public void WeeklyDownloadPeriods()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);

            DownloadPeriodsTest(head, tail, 11, Resolution.Weeks);
        }

        [Test]
        public void WeeklyDownloadResolution()
        {
            _priceSeries = _priceSeriesFactory.ConstructPriceSeries(_ticker, Resolution.Weeks);
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var minTimeSpan = new TimeSpan(1, 0, 0, 0);
            var maxTimeSpan = new TimeSpan(7, 0, 0, 0);

            DownloadResolutionTest(head, tail, minTimeSpan, maxTimeSpan, Resolution.Weeks);
        }

        [Test]
        public void WeeklyDownloadDates()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);

            DownloadDatesTest(head, tail, Resolution.Weeks);
        }

        private void DownloadPeriodsTest(DateTime head, DateTime tail, int expected, Resolution resolution)
        {
            _provider.UpdatePriceSeries(_priceSeries, head, tail, resolution);
            Assert.AreEqual(expected, _priceSeries.PricePeriods.Count());
        }

        private void DownloadResolutionTest(DateTime head, DateTime tail, TimeSpan minTimeSpan, TimeSpan maxTimeSpan, Resolution resolution)
        {
            _provider.UpdatePriceSeries(_priceSeries, head, tail, resolution);

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
            _provider.UpdatePriceSeries(_priceSeries, head, tail, resolution);

            Assert.AreEqual(head, _priceSeries.Head);
            Assert.AreEqual(tail, _priceSeries.Tail);
        }
    }
}
