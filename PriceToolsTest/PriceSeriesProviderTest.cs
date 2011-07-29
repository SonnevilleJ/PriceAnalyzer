using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class PriceSeriesProviderTest
    {
        [TestMethod]
        public void YahooDownloadDailyTestResolution()
        {
            var provider = new YahooPriceSeriesProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).AddHours(23).AddMinutes(59).AddSeconds(59);

            PriceHistoryCsvFile target = provider.GetPriceHistoryCsvFile("DE", head, tail, PriceSeriesResolution.Days);

            Assert.AreEqual(PriceSeriesResolution.Days, target.PriceSeries.Resolution);
            foreach (var period in target.PriceSeries.DataPeriods)
            {
                Assert.IsTrue(period.Tail - period.Head < new TimeSpan(24, 0, 0));
            }
        }

        [TestMethod]
        public void YahooDownloadDailyTestPeriods()
        {
            var provider = new YahooPriceSeriesProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).AddHours(23).AddMinutes(59).AddSeconds(59);

            PriceHistoryCsvFile target = provider.GetPriceHistoryCsvFile("DE", head, tail, PriceSeriesResolution.Days);

            Assert.AreEqual(50, target.PriceSeries.PricePeriods.Count);
        }

        [TestMethod]
        public void YahooDownloadDailyTestDates()
        {
            var provider = new YahooPriceSeriesProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).AddHours(23).AddMinutes(59).AddSeconds(59);

            PriceHistoryCsvFile target = provider.GetPriceHistoryCsvFile("DE", head, tail, PriceSeriesResolution.Days);

            // verify dates
            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }

        [TestMethod]
        public void GoogleDownloadDailyTestResolution()
        {
            var provider = new GooglePriceSeriesProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).AddHours(23).AddMinutes(59).AddSeconds(59);
            PriceHistoryCsvFile target = provider.GetPriceHistoryCsvFile("DE", head, tail, PriceSeriesResolution.Days);

            Assert.AreEqual(PriceSeriesResolution.Days, target.PriceSeries.Resolution);
            foreach (var period in target.PriceSeries.DataPeriods)
            {
                Assert.IsTrue(period.Tail - period.Head < new TimeSpan(24, 0, 0));
            }
        }

        [TestMethod]
        public void GoogleDownloadDailyTestPeriods()
        {
            var provider = new GooglePriceSeriesProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).AddHours(23).AddMinutes(59).AddSeconds(59);
            PriceHistoryCsvFile target = provider.GetPriceHistoryCsvFile("DE", head, tail, PriceSeriesResolution.Days);

            Assert.AreEqual(50, target.PriceSeries.PricePeriods.Count);
        }

        [TestMethod]
        public void GoogleDownloadDailyTestDates()
        {
            var provider = new GooglePriceSeriesProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).AddHours(23).AddMinutes(59).AddSeconds(59);
            PriceHistoryCsvFile target = provider.GetPriceHistoryCsvFile("DE", head, tail, PriceSeriesResolution.Days);

            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }

        [TestMethod]
        public void GoogleDownloadWeeklyTestPeriods()
        {
            var provider = new GooglePriceSeriesProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).AddHours(23).AddMinutes(59).AddSeconds(59);
            PriceHistoryCsvFile target = provider.GetPriceHistoryCsvFile("DE", head, tail, PriceSeriesResolution.Weeks);

            Assert.AreEqual(11, target.PricePeriods.Count);
        }

        [TestMethod]
        public void GoogleDownloadWeeklyTestResolution()
        {
            var provider = new GooglePriceSeriesProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).AddHours(23).AddMinutes(59).AddSeconds(59);
            PriceHistoryCsvFile target = provider.GetPriceHistoryCsvFile("DE", head, tail, PriceSeriesResolution.Weeks);

            Assert.AreEqual(PriceSeriesResolution.Weeks, target.PriceSeries.Resolution);
            var periods = target.PricePeriods;
            for (int i = 1; i < periods.Count - 1; i++) // skip check on first and last periods
            {
                Assert.IsTrue(periods[i].Tail - periods[i].Head >= new TimeSpan(23, 59, 59));
                Assert.IsTrue(periods[i].Tail - periods[i].Head < new TimeSpan(7, 0, 0, 0));
            }
        }

        [TestMethod]
        public void GoogleDownloadWeeklyTestDates()
        {
            var provider = new GooglePriceSeriesProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).AddHours(23).AddMinutes(59).AddSeconds(59);
            PriceHistoryCsvFile target = provider.GetPriceHistoryCsvFile("DE", head, tail, PriceSeriesResolution.Weeks);

            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }
    }
}
