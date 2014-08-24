using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;

namespace Sonneville.PriceTools.Test
{
    /// <summary>
    /// Summary description for ConstantPriceSeriesTest
    /// </summary>
    [TestFixture]
    public class ConstantPriceSeriesTest
    {
        private IPriceSeriesFactory _priceSeriesFactory;
        private ITimeSeriesUtility _timeSeriesUtility;

        [SetUp]
        public void Setup()
        {
            _priceSeriesFactory = new PriceSeriesFactory();
            _timeSeriesUtility = new TimeSeriesUtility();
        }

        [Test]
        public void IndexerValueTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            Assert.AreEqual(1m, priceSeries[DateTime.MinValue]);
            Assert.AreEqual(1m, priceSeries[new DateTime(2014, 6, 28)]);
            Assert.AreEqual(1m, priceSeries[DateTime.MaxValue]);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void AddPriceDataTest1()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            priceSeries.AddPriceData(new Mock<IPricePeriod>().Object);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void AddPriceDataTest2()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            var pricePeriod = new Mock<IPricePeriod>().Object;
            priceSeries.AddPriceData(new List<IPricePeriod> {pricePeriod});
        }

        [Test]
        public void OpenTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            Assert.AreEqual(1m, priceSeries.Open);
        }

        [Test]
        public void HighTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            Assert.AreEqual(1m, priceSeries.High);
        }

        [Test]
        public void LowTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            Assert.AreEqual(1m, priceSeries.Low);
        }

        [Test]
        public void CloseTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            Assert.AreEqual(1m, priceSeries.Close);
        }

        [Test]
        public void VolumeTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructConstantPriceSeries("FTEXX");

            Assert.IsNull(priceSeries.Volume);
        }
    }
}
