﻿using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.TestPriceData;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Google.Test
{
    [TestClass]
    public class GooglePriceHistoryCsvFileTest
    {
        [TestMethod]
        public void GoogleWeeklyTestPeriods()
        {
            var target = TestPriceHistoryCsvFiles.MSFT_Apr_June_2011_Weekly_Google;

            Assert.AreEqual(14, target.PricePeriods.Count);
        }

        [TestMethod]
        public void GoogleWeeklyTestResolution()
        {
            var target = TestPriceHistoryCsvFiles.MSFT_Apr_June_2011_Weekly_Google;

            Assert.AreEqual(Resolution.Weeks, target.PriceSeries.Resolution);
            var periods = target.PricePeriods;

            for (var i = 1; i < periods.Count - 1; i++) // skip check on first and last periods
            {
                Assert.IsTrue(periods[i].Tail - periods[i].Head >= new TimeSpan(23, 59, 59));
                Assert.IsTrue(periods[i].Tail - periods[i].Head < new TimeSpan(7, 0, 0, 0));
            }
        }

        [TestMethod]
        public void GoogleWeeklyTestDates()
        {
            var head = new DateTime(2011, 4, 1);
            var tail = new DateTime(2011, 7, 1, 23, 59, 59);
            PriceHistoryCsvFile target = new GooglePriceHistoryCsvFile(TestUtilities.GetUniqueTicker(), new ResourceStream(TestCsvPriceHistory.MSFT_Apr_June_2011_Weekly_Google), head, tail);

            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }

        [TestMethod]
        public void GoogleWeeklyTestWriteAndRead()
        {
            PriceHistoryCsvFile originalFile;
            PriceHistoryCsvFile targetFile;
            var tempFileName = Path.GetTempFileName();

            using (var writer = new StreamWriter(tempFileName))
            {
                originalFile = TestPriceHistoryCsvFiles.MSFT_Apr_June_2011_Weekly_Google;
                originalFile.Write(writer);
            }

            using (var reader = File.Open(tempFileName, FileMode.Open))
            {
                targetFile = new GooglePriceHistoryCsvFile();
                targetFile.Read(TestUtilities.GetUniqueTicker(), reader);
            }

            CollectionAssert.AreEqual(originalFile.PricePeriods.ToArray(), targetFile.PricePeriods.ToArray());
        }
    }
}
