using System;
using System.Linq;
using NUnit.Framework;
using Sonneville.PriceTools.SampleData;
using Sonneville.PriceTools.TestUtilities;

namespace Sonneville.PriceTools.Test
{
    [TestFixture]
    public class PriceSeriesTest
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
        public void CloseTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p3.Close;
            var actual = target.Close;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OrderedCloseTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p3);
            target.AddPriceData(p2);
            target.AddPriceData(p1);

            var expected = p3.Close;
            var actual = target.Close;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HasValue1Test()
        {
            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            Assert.IsFalse(_timeSeriesUtility.HasValueInRange(target, DateTime.Now));
        }

        [Test]
        public void HasValue2Test()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            Assert.IsTrue(_timeSeriesUtility.HasValueInRange(target, p1.Head));
        }

        [Test]
        public void HasValue3Test()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            Assert.IsTrue(_timeSeriesUtility.HasValueInRange(target, p3.Tail));
        }

        [Test]
        public void HeadTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p1.Head;
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HeadEmptyTest()
        {
            var test = _priceSeriesFactory.ConstructPriceSeries("DE").Head;

            Assert.IsNull(test);
        }

        [Test]
        public void HighTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            decimal? expected = new[] {p1.High, p2.High, p3.High}.Max();
            decimal? actual = target.High;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IndexerValueAtHeadTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            decimal? expected = target.Open;
            decimal? actual = target[p1.Head];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IndexerValueAtTailTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            decimal? expected = p3.Close;
            decimal? actual = target[target.Tail];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IndexerValueBeforeHeadTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            const decimal expected = 0.00m;
            var actual = target[p1.Head.Subtract(new TimeSpan(1))];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IndexerValueAfterTailTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p3.Close;
            var actual = target[p3.Tail.Add(new TimeSpan(1))];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LowTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            decimal? expected = p3.Low;
            decimal? actual = target.Low;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OpenTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            decimal? expected = p1.Open;
            decimal? actual = target.Open;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OrderedOpenTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p3);
            target.AddPriceData(p2);
            target.AddPriceData(p1);

            decimal? expected = p1.Open;
            decimal? actual = target.Open;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PricePeriodsTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            Assert.AreEqual(3, target.PricePeriods.Count());
            Assert.IsTrue(target.PricePeriods.Contains(p1));
            Assert.IsTrue(target.PricePeriods.Contains(p2));
            Assert.IsTrue(target.PricePeriods.Contains(p3));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetDailyPeriodsFromWeeklyPeriodsTest()
        {
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries("DE", Resolution.Weeks);
            _timeSeriesUtility.ResizePricePeriods(priceSeries, Resolution.Days);
        }

        [Test]
        public void TailTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p3.Tail;
            var actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TailEmptyTest()
        {
            var test = _priceSeriesFactory.ConstructPriceSeries("DE").Tail;

            Assert.IsNull(test);
        }

        [Test]
        public void TickerTest()
        {
            var ticker = "DE";
            var target = _priceSeriesFactory.ConstructPriceSeries(ticker);

            var expected = ticker;
            var actual = target.Ticker;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VolumeTest()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p1.Volume + p3.Volume; // p2 has no volume
            var actual = target.Volume;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIndexerGetsMostRecentPriceBeforeNextPeriod()
        {
            var p1 = PricePeriodUtilities.CreatePeriod1();
            var p2 = PricePeriodUtilities.CreatePeriod2();
            var p3 = PricePeriodUtilities.CreatePeriod3();

            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            target.AddPriceData(p1);
            target.AddPriceData(p2);
            target.AddPriceData(p3);

            var expected = p2.Close;
            var actual = target[p3.Head.Subtract(new TimeSpan(1))];
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DefaultResolutionTest()
        {
            var target = _priceSeriesFactory.ConstructPriceSeries("DE");

            const Resolution expected = Resolution.Days;
            var actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddPricePeriodAddsToPricePeriodsTest()
        {
            var target = _priceSeriesFactory.ConstructPriceSeries("DE");
            var head = new DateTime(2011, 12, 28);
            var tail = head.NextPeriodClose(target.Resolution);
            const decimal close = 5.00m;
            var period = new PricePeriod(head, tail, close);

            target.AddPriceData(period);

            Assert.IsTrue(target.PricePeriods.Contains(period));
        }

        [Test]
        public void TimePeriodsTest()
        {
            var target = SamplePriceDatas.Deere.PriceSeries;

            CollectionAssert.AreEquivalent(target.PricePeriods.Cast<ITimePeriod<decimal>>().ToList(), target.TimePeriods.ToList());
        }
    }
}