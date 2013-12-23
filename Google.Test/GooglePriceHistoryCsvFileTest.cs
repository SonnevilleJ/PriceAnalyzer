using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.Data.Test;

namespace Sonneville.PriceTools.Google.Test
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
