using System;
using System.IO;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Sonneville.PriceToolsTest
{
    
    
    /// <summary>
    ///This is a test class for YahooDataManagerTest and is intended
    ///to contain all YahooDataManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class YahooDataManagerTest
    {
        /// <summary>
        ///A test for PriceParser
        ///</summary>
        [TestMethod()]
        public void PriceParserTest()
        {
            IPriceSeries actual;
            using (Stream dataStream = new MemoryStream(TestData.SPX_8_Dec_2010_to_10_Dec_2010))
            {
                actual = YahooPriceSeriesProvider.Instance.ParsePriceSeries(dataStream);
            }

            PricePeriod p1 = new PricePeriod(DateTime.Parse("2010-12-10"), DateTime.Parse("2010-12-10"), 71.31m, 71.55m, 70.32m, 71.52m, 360200);
            PricePeriod p2 = new PricePeriod(DateTime.Parse("2010-12-09"), DateTime.Parse("2010-12-09"), 71.47m, 71.51m, 70.65m, 71.00m, 251600);
            PricePeriod p3 = new PricePeriod(DateTime.Parse("2010-12-08"), DateTime.Parse("2010-12-08"), 71.17m, 71.57m, 70.48m, 70.90m, 273400);

            IPriceSeries expected = new PriceSeries(p1, p2, p3);
            Assert.AreEqual(expected, actual);
        }
    }
}
