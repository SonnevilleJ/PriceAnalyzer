﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Test.PriceData;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    /// <summary>
    ///This is a test class for PriceSeriesTest and is intended to contain all PriceSeriesTest Unit Tests
    ///</summary>
    [TestClass]
    public class PriceSeriesTest
    {
        private readonly IPricePeriodFactory _pricePeriodFactory;
        private readonly IPriceSeriesFactory _priceSeriesFactory;

        public PriceSeriesTest()
        {
            _pricePeriodFactory = new PricePeriodFactory();
            _priceSeriesFactory = new PriceSeriesFactory();
        }

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod]
        public void CloseTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p3.Close;
            var actual = target.Close;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod]
        public void OrderedCloseTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p3);
            target.AddPriceData(p2);
            target.AddPriceData(p1);

            var expected = p3.Close;
            var actual = target.Close;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HasValue1Test()
        {
            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            Assert.IsFalse(target.HasValueInRange(DateTime.Now));
        }

        [TestMethod]
        public void HasValue2Test()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            Assert.IsTrue(target.HasValueInRange(p1.Head));
        }

        [TestMethod]
        public void HasValue3Test()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            Assert.IsTrue(target.HasValueInRange(p3.Tail));
        }

        /// <summary>
        ///A test for Head
        ///</summary>
        [TestMethod]
        public void HeadTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p1.Head;
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HeadEmptyTest()
        {
            var test = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker()).Head;
        }

        /// <summary>
        ///A test for High
        ///</summary>
        [TestMethod]
        public void HighTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            decimal? expected = new[] {p1.High, p2.High, p3.High}.Max();
            decimal? actual = target.High;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAtHeadTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            decimal? expected = target.Open;
            decimal? actual = target[p1.Head];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAtTailTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            decimal? expected = p3.Close;
            decimal? actual = target[target.Tail];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueBeforeHeadTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            const decimal expected = 0.00m;
            var actual = target[p1.Head.Subtract(new TimeSpan(1))];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod]
        public void IndexerValueAfterTailTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p3.Close;
            var actual = target[p3.Tail.Add(new TimeSpan(1))];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Low
        ///</summary>
        [TestMethod]
        public void LowTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            decimal? expected = p3.Low;
            decimal? actual = target.Low;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Open
        ///</summary>
        [TestMethod]
        public void OpenTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            decimal? expected = p1.Open;
            decimal? actual = target.Open;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Open
        ///</summary>
        [TestMethod]
        public void OrderedOpenTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p3);
            target.AddPriceData(p2);
            target.AddPriceData(p1);

            decimal? expected = p1.Open;
            decimal? actual = target.Open;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PricePeriods
        ///</summary>
        [TestMethod]
        public void PricePeriodsTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            Assert.AreEqual(3, target.PricePeriods.Count());
            Assert.IsTrue(target.PricePeriods.Contains(p1));
            Assert.IsTrue(target.PricePeriods.Contains(p2));
            Assert.IsTrue(target.PricePeriods.Contains(p3));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetDailyPeriodsFromWeeklyPeriodsTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker(), Resolution.Weeks);
            priceSeries.ResizePricePeriods(Resolution.Days);
        }

        /// <summary>
        ///A test for Tail
        ///</summary>
        [TestMethod]
        public void TailTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p3.Tail;
            var actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TailEmptyTest()
        {
            var test = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker()).Tail;
        }

        /// <summary>
        ///A test for Ticker
        ///</summary>
        [TestMethod]
        public void TickerTest()
        {
            var ticker = TickerManager.GetUniqueTicker();
            var target = _priceSeriesFactory.ConstructPriceSeries(ticker);

            var expected = ticker;
            var actual = target.Ticker;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Volume
        ///</summary>
        [TestMethod]
        public void VolumeTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p1.Volume + p3.Volume; // p2 has no volume
            var actual = target.Volume;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIndexerGetsMostRecentPriceBeforeNextPeriod()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p2.Close;
            var actual = target[p3.Head.Subtract(new TimeSpan(1))];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DefaultResolutionTest()
        {
            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());

            const Resolution expected = Resolution.Days;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddPricePeriodAddsToPricePeriodsTest()
        {
            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            var head = new DateTime(2011, 12, 28);
            var tail = head.NextPeriodClose(target.Resolution);
            const decimal close = 5.00m;
            var period = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, close);

            target.AddPriceData(period);

            Assert.IsTrue(target.PricePeriods.Contains(period));
        }

        [TestMethod]
        public void AddPricePeriodOverlapTestInner()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var head = new DateTime(2011, 2, 28);
            var tail = head.NextPeriodClose(target.Resolution);
            const decimal close = 5.00m;
            var period = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, close);

            var triggered = false;
            EventHandler<NewDataAvailableEventArgs> handler = (sender, e) => { triggered = true; };

            try
            {
                target.AddPriceData(period);
            }
            finally
            {
            }

            Assert.IsFalse(triggered);
        }

        [TestMethod]
        public void AddPricePeriodOverlapHeadTest()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var tail = target.Head.NextPeriodClose(target.Resolution);
            var head = tail.AddDays(-1).PreviousPeriodOpen(target.Resolution);
            const decimal close = 5.00m;
            var period = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, close);

            var triggered = false;
            EventHandler<NewDataAvailableEventArgs> handler = (sender, e) => { triggered = true; };

            try
            {
                target.AddPriceData(period);
            }
            finally
            {
            }

            Assert.IsFalse(triggered);
        }

        [TestMethod]
        public void AddPricePeriodOverlapTailTest()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var head = target.Tail.PreviousPeriodOpen(target.Resolution);
            var tail = head.AddDays(1).NextPeriodClose(target.Resolution);
            const decimal close = 5.00m;
            var period = _pricePeriodFactory.ConstructStaticPricePeriod(head, tail, close);

            var triggered = false;
            EventHandler<NewDataAvailableEventArgs> handler = (sender, e) => { triggered = true; };

            try
            {
                target.AddPriceData(period);
            }
            finally
            {
            }

            Assert.IsFalse(triggered);
        }

        [TestMethod]
        public void TimePeriodsTest()
        {
            var target = TestPriceSeries.DE_1_1_2011_to_6_30_2011;

            CollectionAssert.AreEquivalent(target.PricePeriods.Cast<ITimePeriod>().ToList(), target.TimePeriods.ToList());
        }
    }
}