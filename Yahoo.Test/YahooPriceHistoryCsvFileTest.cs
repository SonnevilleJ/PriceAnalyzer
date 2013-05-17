using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.Yahoo;
using Test.Sonneville.PriceTools.Data;

namespace Test.Sonneville.PriceTools.Yahoo
{
    [TestClass]
    public class YahooPriceHistoryCsvFileTest : PriceHistoryCsvFileTest
    {
        protected override PriceHistoryCsvFile GetTestObject(Stream stream, DateTime seriesHead, DateTime seriesTail)
        {
            return new YahooPriceHistoryCsvFile(stream, seriesHead, seriesTail);
        }
    }
}
