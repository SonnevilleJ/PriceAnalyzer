﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    [TestClass]
    public class PriceSeriesEqualityTests
    {
        private readonly IPricePeriodFactory _pricePeriodFactory;
        private readonly IPriceSeriesFactory _priceSeriesFactory;
        private string _ticker1 = "MSFT";
        private string _ticker2 = "GOOG";
        private string _ticker3 = "AAPL";

        public PriceSeriesEqualityTests()
        {
            _pricePeriodFactory = new PricePeriodFactory();
            _priceSeriesFactory = new PriceSeriesFactory();
        }

        [TestMethod]
        public void EqualsEmptyPriceSeries()
        {
            var ps1 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var ps2 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);

            Assert.AreEqual(ps1, ps2);
        }

        [TestMethod]
        public void EqualsTestDifferentTicker()
        {
            var ps1 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var ps2 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);

            Assert.IsFalse(ps1.Equals(ps2));
        }

        [TestMethod]
        public void ReferenceEqualsTestDifferentTicker()
        {
            var ps1 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var ps2 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);

            Assert.IsFalse(ReferenceEquals(ps1, ps2));
        }

        [TestMethod]
        public void GetHashCodeTestDifferentTicker()
        {
            var ps1 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var ps2 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);

            Assert.IsFalse(ReferenceEquals(ps1, ps2));
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithDifferentData()
        {
            var series1 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(_ticker3);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));
            series4.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series4.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3, series4 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithExtraseries()
        {
            var series1 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);
            var series5 = _priceSeriesFactory.ConstructPriceSeries(_ticker3);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));
            series5.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series5.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3, series4, series5 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithMissingseries()
        {
            var series1 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithDifferentData()
        {
            var series1 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(_ticker3);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));
            series3.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series3.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3, series4 };

            Assert.IsFalse(list1.Equals(list2));
        }

        [TestMethod]
        public void EnumerableEqualsWithSameData()
        {
            var series1 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3, series4 };

            CollectionAssert.AreEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithExtraseries()
        {
            var series1 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);
            var series5 = _priceSeriesFactory.ConstructPriceSeries(_ticker3);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));
            series5.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series5.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3, series4, series5 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithMissingseries()
        {
            var series1 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsOrderCheck()
        {
            var series1 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(_ticker2);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(_ticker1);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3, series4 };

            CollectionAssert.AreNotEqual(list1, list2);
        }
    }
}
