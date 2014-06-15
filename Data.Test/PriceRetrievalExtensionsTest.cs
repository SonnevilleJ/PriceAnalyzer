using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sonneville.PriceTools.SampleData;

namespace Sonneville.PriceTools.Data.Test
{
    [TestClass]
    public class PriceRetrievalExtensionsTest
    {
        private DateTime _head;
        private IPriceSeries _priceSeries;
        private Mock<IPriceDataProvider> _provider;
        private IPriceHistoryCsvFileFactory _priceHistoryCsvFileFactory;
        private IPriceSeriesRetriever _priceSeriesRetriever;

        [TestInitialize]
        public void Initialize()
        {
            _head = new DateTime(2011, 1, 1);
            _priceSeries = new PriceSeriesFactory().ConstructPriceSeries("DE");

            _provider = new Mock<IPriceDataProvider>();
            _provider.Setup(x => x.UpdatePriceSeries(It.IsAny<IPriceSeries>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Resolution>()))
                .Callback(() => _priceSeries = SamplePriceDatas.Deere.PriceSeries);
            _priceHistoryCsvFileFactory = new Mock<IPriceHistoryCsvFileFactory>().Object;
            
            _priceSeriesRetriever = new PriceSeriesRetriever();
        }

        [TestMethod]
        public void TestDownloadPriceDataHead()
        {
            _priceSeriesRetriever.UpdatePriceData(_priceSeries, _provider.Object, _head, _priceHistoryCsvFileFactory);

            Assert.IsNotNull(_priceSeries[_head.AddHours(12)]);    // add 12 hours because no price is available at midnight.
        }

        [TestMethod]
        public void TestDownloadPriceDataHeadTail()
        {
            var tail = _head.AddMonths(1);

            _priceSeriesRetriever.UpdatePriceData(_priceSeries, _provider.Object, _head, tail, _priceHistoryCsvFileFactory);

            Assert.IsNotNull(_priceSeries[tail]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDownloadPriceDataProvidersResolutionIsChecked()
        {
            _provider.SetupGet(x => x.BestResolution).Returns(_priceSeries.Resolution + 1L);
            var tail = _head.AddMonths(1);

            _priceSeriesRetriever.UpdatePriceData(_priceSeries, _provider.Object, _head, tail, _priceHistoryCsvFileFactory);
        }
    }
}