using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.Test.Utilities;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PriceSeriesEqualityTests
    {
        [TestMethod]
        public void EqualsEmptyPriceSeries()
        {
            var ticker = TestUtilities.GetUniqueTicker();
            var ps1 = PriceSeriesFactory.CreatePriceSeries(ticker);
            var ps2 = PriceSeriesFactory.CreatePriceSeries(ticker);

            Assert.IsTrue(ps1.Equals(ps2));
        }

        [TestMethod]
        public void EqualsTestSameTicker()
        {
            var ticker = TestUtilities.GetUniqueTicker();
            var ps1 = PriceSeriesFactory.CreatePriceSeries(ticker);
            var ps2 = PriceSeriesFactory.CreatePriceSeries(ticker);

            Assert.IsTrue(ps1.Equals(ps2));
        }

        [TestMethod]
        public void EqualsTestDifferentTicker()
        {
            var ps1 = PriceSeriesFactory.CreatePriceSeries(TestUtilities.GetUniqueTicker());
            var ps2 = PriceSeriesFactory.CreatePriceSeries(TestUtilities.GetUniqueTicker());

            Assert.IsFalse(ps1.Equals(ps2));
        }

        [TestMethod]
        public void ReferenceEqualsTestSameTicker()
        {
            var ticker = TestUtilities.GetUniqueTicker();
            var ps1 = PriceSeriesFactory.CreatePriceSeries(ticker);
            var ps2 = PriceSeriesFactory.CreatePriceSeries(ticker);

            Assert.IsTrue(ReferenceEquals(ps1, ps2));
        }

        [TestMethod]
        public void ReferenceEqualsTestDifferentTicker()
        {
            var ps1 = PriceSeriesFactory.CreatePriceSeries(TestUtilities.GetUniqueTicker());
            var ps2 = PriceSeriesFactory.CreatePriceSeries(TestUtilities.GetUniqueTicker());

            Assert.IsFalse(ReferenceEquals(ps1, ps2));
        }

        [TestMethod]
        public void GetHashCodeTestSameTicker()
        {
            var ticker = TestUtilities.GetUniqueTicker();
            var ps1 = PriceSeriesFactory.CreatePriceSeries(ticker);
            var ps2 = PriceSeriesFactory.CreatePriceSeries(ticker);

            Assert.IsTrue(ReferenceEquals(ps1, ps2));
        }

        [TestMethod]
        public void GetHashCodeTestDifferentTicker()
        {
            var ps1 = PriceSeriesFactory.CreatePriceSeries(TestUtilities.GetUniqueTicker());
            var ps2 = PriceSeriesFactory.CreatePriceSeries(TestUtilities.GetUniqueTicker());

            Assert.IsFalse(ReferenceEquals(ps1, ps2));
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithDifferentData()
        {
            var ticker1 = TestUtilities.GetUniqueTicker();
            var ticker2 = TestUtilities.GetUniqueTicker();
            var ticker3 = TestUtilities.GetUniqueTicker();
            var series1 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series2 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series3 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series4 = PriceSeriesFactory.CreatePriceSeries(ticker3);
            var dateTime = DateTime.Now;
            series1.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series2.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series4.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithSameData()
        {
            var ticker1 = TestUtilities.GetUniqueTicker();
            var ticker2 = TestUtilities.GetUniqueTicker();
            var series1 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series2 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series3 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series4 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var dateTime = DateTime.Now;
            series1.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series2.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4 };

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithExtraseries()
        {
            var ticker1 = TestUtilities.GetUniqueTicker();
            var ticker2 = TestUtilities.GetUniqueTicker();
            var ticker3 = TestUtilities.GetUniqueTicker();
            var series1 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series2 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series3 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series4 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series5 = PriceSeriesFactory.CreatePriceSeries(ticker3);
            var dateTime = DateTime.Now;
            series1.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series2.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series5.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4, series5 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentWithMissingseries()
        {
            var ticker1 = TestUtilities.GetUniqueTicker();
            var ticker2 = TestUtilities.GetUniqueTicker();
            var series1 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series2 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series3 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var dateTime = DateTime.Now;
            series1.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series2.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3 };

            CollectionAssert.AreNotEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableIsEquivalentOrderCheck()
        {
            var ticker1 = TestUtilities.GetUniqueTicker();
            var ticker2 = TestUtilities.GetUniqueTicker();
            var series1 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series2 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series3 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series4 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var dateTime = DateTime.Now;
            series1.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series2.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4 };

            CollectionAssert.AreEquivalent(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithDifferentData()
        {
            var ticker1 = TestUtilities.GetUniqueTicker();
            var ticker2 = TestUtilities.GetUniqueTicker();
            var ticker3 = TestUtilities.GetUniqueTicker();
            var series1 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series2 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series3 = PriceSeriesFactory.CreatePriceSeries(ticker3);
            var series4 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var dateTime = DateTime.Now;
            series1.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series2.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series3.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4 };

            Assert.IsFalse(list1.Equals(list2));
        }

        [TestMethod]
        public void EnumerableEqualsWithSameData()
        {
            var ticker1 = TestUtilities.GetUniqueTicker();
            var ticker2 = TestUtilities.GetUniqueTicker();
            var series1 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series2 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series3 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series4 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var dateTime = DateTime.Now;
            series1.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series2.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4 };

            CollectionAssert.AreEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithExtraseries()
        {
            var ticker1 = TestUtilities.GetUniqueTicker();
            var ticker2 = TestUtilities.GetUniqueTicker();
            var ticker3 = TestUtilities.GetUniqueTicker();
            var series1 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series2 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series3 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series4 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series5 = PriceSeriesFactory.CreatePriceSeries(ticker3);
            var dateTime = DateTime.Now;
            series1.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series2.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series5.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4, series5 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsWithMissingseries()
        {
            var ticker1 = TestUtilities.GetUniqueTicker();
            var ticker2 = TestUtilities.GetUniqueTicker();
            var series1 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series2 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series3 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var dateTime = DateTime.Now;
            series1.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series2.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3 };

            CollectionAssert.AreNotEqual(list1, list2);
        }

        [TestMethod]
        public void EnumerableEqualsOrderCheck()
        {
            var ticker1 = TestUtilities.GetUniqueTicker();
            var ticker2 = TestUtilities.GetUniqueTicker();
            var series1 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var series2 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series3 = PriceSeriesFactory.CreatePriceSeries(ticker2);
            var series4 = PriceSeriesFactory.CreatePriceSeries(ticker1);
            var dateTime = DateTime.Now;
            series1.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));
            series2.AddPriceData(PricePeriodFactory.ConstructStaticPricePeriod(dateTime, dateTime.GetFollowingClose(), 100));

            var list1 = new List<PriceSeries> { series1, series2 };
            var list2 = new List<PriceSeries> { series3, series4 };

            CollectionAssert.AreNotEqual(list1, list2);
        }
    }
}
