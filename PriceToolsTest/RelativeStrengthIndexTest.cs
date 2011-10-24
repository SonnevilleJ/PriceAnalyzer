using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.SamplePriceData;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    /// Summary description for RelativeStrengthIndexTest
    /// </summary>
    [TestClass]
    public class RelativeStrengthIndexTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Settings.SetDefaultSettings();
            Settings.CanConnectToInternet = false;
        }

        [TestMethod]
        public void ResolutionDaysByDefault()
        {
            IPriceSeries priceSeries = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;

            RelativeStrengthIndex target = new RelativeStrengthIndex(priceSeries);

            const Resolution expected = Resolution.Days;
            Resolution actual = target.Resolution;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadTest()
        {
            IPriceSeries priceSeries = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;

            RelativeStrengthIndex target = new RelativeStrengthIndex(priceSeries);

            DateTime expected = priceSeries.GetPricePeriods(target.Resolution)[target.Lookback - 1].Head;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void QueryBeforeHeadThrowsException()
        {
            IPriceSeries series = SamplePriceSeries.DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS;
            RelativeStrengthIndex rsi = new RelativeStrengthIndex(series);

            var result = rsi[rsi.Head.Subtract(new TimeSpan(1))];
        }
    }
}
