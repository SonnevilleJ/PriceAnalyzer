using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sonneville.PriceTools.TestPriceData;

namespace Sonneville.PriceTools.Data.Test
{
    [TestClass]
    public class PriceRetrievalExtensionsTest
    {
        private DateTime _head;
        private IPriceSeries _priceSeries;
        private Mock<IPriceDataProvider> _provider;
        private IPriceHistoryCsvFileFactory _priceHistoryCsvFileFactory;

        [TestInitialize]
        public void Initialize()
        {
            _head = new DateTime(2011, 1, 1);
            _priceSeries = new PriceSeriesFactory().ConstructPriceSeries("DE");

            _provider = new Mock<IPriceDataProvider>();
            _provider.Setup(x => x.UpdatePriceSeries(It.IsAny<IPriceSeries>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Resolution>(), It.IsAny<IPriceHistoryCsvFileFactory>()))
                .Callback(() => _priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011);
            _priceHistoryCsvFileFactory = new Mock<IPriceHistoryCsvFileFactory>().Object;
        }

        [TestMethod]
        public void TestDownloadPriceDataHead()
        {
            _priceSeries.UpdatePriceData(_provider.Object, _head, _priceHistoryCsvFileFactory);

            Assert.IsNotNull(_priceSeries[_head.AddHours(12)]);    // add 12 hours because no price is available at midnight.
        }

        [TestMethod]
        public void TestDownloadPriceDataHeadTail()
        {
            var tail = _head.AddMonths(1);

            _priceSeries.UpdatePriceData(_provider.Object, _head, tail, _priceHistoryCsvFileFactory);

            Assert.IsNotNull(_priceSeries[tail]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDownloadPriceDataProvidersResolutionIsChecked()
        {
            _provider.SetupGet(x => x.BestResolution).Returns(_priceSeries.Resolution + 1L);
            var tail = _head.AddMonths(1);

            _priceSeries.UpdatePriceData(_provider.Object, _head, tail, _priceHistoryCsvFileFactory);
        }
    }
}