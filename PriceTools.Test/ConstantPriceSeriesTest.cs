using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Sonneville.PriceTools.Test
{
    /// <summary>
    /// Summary description for ConstantPriceSeriesTest
    /// </summary>
    [TestClass]
    public class ConstantPriceSeriesTest
    {
        private IPriceSeriesFactory _priceSeriesFactory;
        private ITimeSeriesUtility _timeSeriesUtility;

        [TestInitialize]
        public void Initialize()
        {
            _priceSeriesFactory = new PriceSeriesFactory();
            _timeSeriesUtility = new TimeSeriesUtility();
        }

        [TestMethod]
        public void IndexerValueTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            Assert.AreEqual(1m, priceSeries[DateTime.MinValue]);
            Assert.AreEqual(1m, priceSeries[new DateTime(2014, 6, 28)]);
            Assert.AreEqual(1m, priceSeries[DateTime.MaxValue]);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AddPriceDataTest1()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            priceSeries.AddPriceData(new Mock<IPricePeriod>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AddPriceDataTest2()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            var pricePeriod = new Mock<IPricePeriod>().Object;
            priceSeries.AddPriceData(new List<IPricePeriod> {pricePeriod});
        }

        [TestMethod]
        public void OpenTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            Assert.AreEqual(1m, priceSeries.Open);
        }

        [TestMethod]
        public void HighTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            Assert.AreEqual(1m, priceSeries.High);
        }

        [TestMethod]
        public void LowTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            Assert.AreEqual(1m, priceSeries.Low);
        }

        [TestMethod]
        public void CloseTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            Assert.AreEqual(1m, priceSeries.Close);
        }

        [TestMethod]
        public void VolumeTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            Assert.IsNull(priceSeries.Volume);
        }
    }
}
