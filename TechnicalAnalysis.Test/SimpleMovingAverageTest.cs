using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.TechnicalAnalysis;

namespace Test.Sonneville.PriceTools.TechnicalAnalysis
{
    [TestClass]
    public class SimpleMovingAverageTest
    {
        [TestMethod]
        public void FlatPeriodReturnsSameAverage()
        {
            var date = new DateTime(2011, 3, 1);
            const int price = 2;

            var series = CreateTestPriceSeries(10, date, price);

            const int lookback = 2;
            var ma = new SimpleMovingAverage(series, lookback);

            const decimal expected = price;
            for (var i = lookback; i < series.PricePeriods.Count; i++)
            {
                var actual = ma[date.AddDays(i)];
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void DataInNonTradingPeriods()
        {
            var p1 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p2 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p3 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p4 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p5 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p6 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p7 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p8 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p9 = PricePeriodFactory.ConstructTickedPricePeriod();

            var date = new DateTime(2000, 1, 1);
            p1.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date, 1));
            p2.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.AddDays(1), 2));
            p3.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.AddDays(2), 3));
            p4.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.AddDays(3), 4));
            p5.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.AddDays(4), 5));
            p6.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.AddDays(5), 4));
            p7.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.AddDays(6), 3));
            p8.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.AddDays(7), 2));
            p9.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.AddDays(8), 1));

            var series = PriceSeriesFactory.CreatePriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            series.AddPriceData(p1);
            series.AddPriceData(p2);
            series.AddPriceData(p3);
            series.AddPriceData(p4);
            series.AddPriceData(p5);
            series.AddPriceData(p6);
            series.AddPriceData(p7);
            series.AddPriceData(p8);
            series.AddPriceData(p9);

            // create 4 day moving average
            const int lookback = 4;
            var target = new SimpleMovingAverage(series, lookback);

            target.CalculateAll();
            Assert.AreEqual(2.5m, target[date.AddDays(3)]);
            Assert.AreEqual(3.5m, target[date.AddDays(4)]);
            Assert.AreEqual(4.0m, target[date.AddDays(5)]);
            Assert.AreEqual(4.0m, target[date.AddDays(6)]);
            Assert.AreEqual(3.5m, target[date.AddDays(7)]);
            Assert.AreEqual(2.5m, target[date.AddDays(8)]);
        }

        [TestMethod]
        public void DataInTradingPeriods()
        {
            var p1 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p2 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p3 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p4 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p5 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p6 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p7 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p8 = PricePeriodFactory.ConstructTickedPricePeriod();
            var p9 = PricePeriodFactory.ConstructTickedPricePeriod();

            const Resolution resolution = Resolution.Days;
            var date = new DateTime(2000, 1, 1);
            p1.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.SeekTradingPeriods(1, resolution), 1));
            p2.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.SeekTradingPeriods(2, resolution), 2));
            p3.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.SeekTradingPeriods(3, resolution), 3));
            p4.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.SeekTradingPeriods(4, resolution), 4));
            p5.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.SeekTradingPeriods(5, resolution), 5));
            p6.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.SeekTradingPeriods(6, resolution), 4));
            p7.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.SeekTradingPeriods(7, resolution), 3));
            p8.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.SeekTradingPeriods(8, resolution), 2));
            p9.AddPriceTicks(PriceTickFactory.ConstructPriceTick(date.SeekTradingPeriods(9, resolution), 1));

            var series = PriceSeriesFactory.CreatePriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            series.AddPriceData(p1);
            series.AddPriceData(p2);
            series.AddPriceData(p3);
            series.AddPriceData(p4);
            series.AddPriceData(p5);
            series.AddPriceData(p6);
            series.AddPriceData(p7);
            series.AddPriceData(p8);
            series.AddPriceData(p9);

            // create 4 day moving average
            const int lookback = 4;
            var target = new SimpleMovingAverage(series, lookback);

            target.CalculateAll();
            Assert.AreEqual(2.5m, target[date.SeekTradingPeriods(4, resolution)]);
            Assert.AreEqual(3.5m, target[date.SeekTradingPeriods(5, resolution)]);
            Assert.AreEqual(4.0m, target[date.SeekTradingPeriods(6, resolution)]);
            Assert.AreEqual(4.0m, target[date.SeekTradingPeriods(7, resolution)]);
            Assert.AreEqual(3.5m, target[date.SeekTradingPeriods(8, resolution)]);
            Assert.AreEqual(2.5m, target[date.SeekTradingPeriods(9, resolution)]);
        }

        private static IPriceSeries CreateTestPriceSeries(int count, DateTime startDate, decimal price)
        {
            var series = PriceSeriesFactory.CreatePriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            for (var i = 0; i < count; i++)
            {
                var period = PricePeriodFactory.ConstructTickedPricePeriod();
                period.AddPriceTicks(PriceTickFactory.ConstructPriceTick(startDate.AddDays(i), price));
                series.AddPriceData(period);
            }
            return series;
        }
    }
}
