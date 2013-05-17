using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Test.PriceData;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools.Data
{
    [TestClass]
    public class PriceRetrievalExtensionsTest
    {
        private string _ticker;
        private DateTime _head;
        private IPriceSeries _priceSeries;

        [TestInitialize]
        public void Initialize()
        {
            _ticker = TickerManager.GetUniqueTicker();
            _head = new DateTime(2011, 1, 1);
            _priceSeries = new PriceSeriesFactory().ConstructPriceSeries(_ticker);
        }

        private Mock<IPriceDataProvider> Provider
        {
            get
            {
                var provider = new Mock<IPriceDataProvider>();
                provider.Setup(x => x.UpdatePriceSeries(It.IsAny<IPriceSeries>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Resolution>())).Callback(
                    () => _priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011);

                return provider;
            }
        }

        [TestMethod]
        public void TestDownloadPriceDataHead()
        {
            _priceSeries.UpdatePriceData(Provider.Object, _head);

            Assert.IsNotNull(_priceSeries[_head.AddHours(12)]);    // add 12 hours because no price is available at midnight.
        }

        [TestMethod]
        public void TestDownloadPriceDataHeadTail()
        {
            var tail = _head.AddMonths(1);

            _priceSeries.UpdatePriceData(Provider.Object, _head, tail);

            Assert.IsNotNull(_priceSeries[tail]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDownloadPriceDataProvidersResolutionIsChecked()
        {
            var provider = Provider;
            provider.SetupGet(x => x.BestResolution).Returns((Resolution) ((long) _priceSeries.Resolution + 1));
            var tail = _head.AddMonths(1);

            _priceSeries.UpdatePriceData(provider.Object, _head, tail);
        }
    }
}