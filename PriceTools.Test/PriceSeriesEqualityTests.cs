using System;
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

        public PriceSeriesEqualityTests()
        {
            _pricePeriodFactory = new PricePeriodFactory();
            _priceSeriesFactory = new PriceSeriesFactory();
        }

        [TestMethod]
        public void EqualsEmptyPriceSeries()
        {
            var ticker = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ps1 = _priceSeriesFactory.ConstructPriceSeries(ticker);
            var ps2 = _priceSeriesFactory.ConstructPriceSeries(ticker);

            Assert.IsTrue(ps1.Equals(ps2));
        }

        [TestMethod]
        public void EqualsTestSameTicker()
        {
            var ticker = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ps1 = _priceSeriesFactory.ConstructPriceSeries(ticker);
            var ps2 = _priceSeriesFactory.ConstructPriceSeries(ticker);

            Assert.IsTrue(ps1.Equals(ps2));
        }

        [TestMethod]
        public void EqualsTestDifferentTicker()
        {
            var ps1 = _priceSeriesFactory.ConstructPriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            var ps2 = _priceSeriesFactory.ConstructPriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());

            Assert.IsFalse(ps1.Equals(ps2));
        }

        [TestMethod]
        public void ReferenceEqualsTestSameTicker()
        {
            var ticker = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ps1 = _priceSeriesFactory.ConstructPriceSeries(ticker);
            var ps2 = _priceSeriesFactory.ConstructPriceSeries(ticker);

            Assert.IsTrue(ReferenceEquals(ps1, ps2));
        }

        [TestMethod]
        public void ReferenceEqualsTestDifferentTicker()
        {
            var ps1 = _priceSeriesFactory.ConstructPriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            var ps2 = _priceSeriesFactory.ConstructPriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());

            Assert.IsFalse(ReferenceEquals(ps1, ps2));
        }

        [TestMethod]
        public void GetHashCodeTestSameTicker()
        {
            var ticker = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ps1 = _priceSeriesFactory.ConstructPriceSeries(ticker);
            var ps2 = _priceSeriesFactory.ConstructPriceSeries(ticker);

            Assert.IsTrue(ReferenceEquals(ps1, ps2));
        }

        [TestMethod]
        public void GetHashCodeTestDifferentTicker()
        {
            var ps1 = _priceSeriesFactory.ConstructPriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            var ps2 = _priceSeriesFactory.ConstructPriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());

            Assert.IsFalse(ReferenceEquals(ps1, ps2));
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithDifferentData()
        {
            var ticker1 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker2 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker3 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var series1 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(ticker3);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));
            series4.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series4.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3, series4 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithSameData()
        {
            var ticker1 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker2 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var series1 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3, series4 };

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithExtraseries()
        {
            var ticker1 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker2 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker3 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var series1 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series5 = _priceSeriesFactory.ConstructPriceSeries(ticker3);
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
            var ticker1 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker2 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var series1 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentOrderCheck()
        {
            var ticker1 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker2 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var series1 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3, series4 };

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithDifferentData()
        {
            var ticker1 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker2 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker3 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var series1 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(ticker3);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
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
            var ticker1 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker2 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var series1 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
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
            var ticker1 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker2 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker3 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var series1 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series5 = _priceSeriesFactory.ConstructPriceSeries(ticker3);
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
            var ticker1 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker2 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var series1 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
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
            var ticker1 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var ticker2 = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var series1 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var series2 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series3 = _priceSeriesFactory.ConstructPriceSeries(ticker2);
            var series4 = _priceSeriesFactory.ConstructPriceSeries(ticker1);
            var dateTime = DateTime.Now;
            series1.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series1.Resolution), 100));
            series2.AddPriceData(_pricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.NextPeriodClose(series2.Resolution), 100));

            var list1 = new List<IPriceSeries> { series1, series2 };
            var list2 = new List<IPriceSeries> { series3, series4 };

            CollectionAssert.AreNotEqual(list1, list2);
        }
    }
}
