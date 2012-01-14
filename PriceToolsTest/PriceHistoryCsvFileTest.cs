using System;
using Sonneville.PriceTools;
using Sonneville.PriceTools.SamplePriceData;
using Sonneville.PriceTools.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for PriceHistoryCsvFileTest and is intended
    ///to contain all PriceHistoryCsvFileTest Unit Tests
    ///</summary>
    [TestClass]
    public class PriceHistoryCsvFileTest
    {
        [TestMethod]
        public void YahooDailyTestDates()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            PriceHistoryCsvFile target = PriceHistoryCsvFiles.DE_1_1_2011_to_3_15_2011_Daily_Yahoo;

            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }

        [TestMethod]
        public void YahooDailyTestPeriods()
        {
            PriceHistoryCsvFile target = PriceHistoryCsvFiles.DE_1_1_2011_to_3_15_2011_Daily_Yahoo;
            
            Assert.AreEqual(50, target.PriceSeries.PricePeriods.Count);
        }

        [TestMethod]
        public void YahooDailyTestResolution()
        {
            PriceHistoryCsvFile target = PriceHistoryCsvFiles.DE_1_1_2011_to_3_15_2011_Daily_Yahoo;
            
            Assert.AreEqual(Resolution.Days, target.PriceSeries.Resolution);
            foreach (var period in ((PriceSeries)target.PriceSeries).PricePeriods)
            {
                Assert.IsTrue(period.Tail - period.Head < new TimeSpan(24, 0, 0));
            }
        }
    }
}
