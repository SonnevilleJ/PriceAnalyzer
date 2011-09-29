using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Services;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    /// Summary description for RelativeStrengthIndexTest
    /// </summary>
    [TestClass]
    public class RelativeStrengthIndexTest
    {
        private static IPriceSeries GetPriceSeries()
        {
            return new YahooPriceHistoryCsvFile(new ResourceStream(TestData.DE_1_1_2011_to_3_15_2011_Daily_Yahoo)).PriceSeries;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Settings.SetDefaultSettings();
            Settings.CanConnectToInternet = false;
        }

        [TestMethod]
        public void ResolutionDaysByDefault()
        {
            IPriceSeries priceSeries = GetPriceSeries();

            RelativeStrengthIndex target = new RelativeStrengthIndex(priceSeries);

            const Resolution expected = Resolution.Days;
            Resolution actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadTest()
        {
            DateTime date = new DateTime(2011, 1, 1);
            IPriceSeries priceSeries = GetPriceSeries();

            RelativeStrengthIndex target = new RelativeStrengthIndex(priceSeries);

            DateTime expected = date.AddDays(target.Lookback);
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void QueryBeforeHeadThrowsException()
        {
            IPriceSeries series = GetPriceSeries();
            RelativeStrengthIndex rsi = new RelativeStrengthIndex(series);

            var result = rsi[rsi.Head.Subtract(new TimeSpan(1))];
        }
    }
}
