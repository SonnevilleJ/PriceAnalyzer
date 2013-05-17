using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.Google;
using Test.Sonneville.PriceTools.Data;

namespace Test.Sonneville.PriceTools.Google
{
    [TestClass]
    public class GooglePriceHistoryCsvFileTest : PriceHistoryCsvFileTest
    {
        protected override PriceHistoryCsvFile GetTestObject(Stream stream, DateTime seriesHead, DateTime seriesTail)
        {
            return new GooglePriceHistoryCsvFile(stream, seriesHead, seriesTail);
        }
    }
}
