using System;
using System.IO;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Yahoo
{
    public sealed class YahooPriceHistoryCsvFile : PriceHistoryCsvFile
    {
        public YahooPriceHistoryCsvFile(Stream stream, DateTime impliedHead = default(DateTime), DateTime impliedTail = default(DateTime), Resolution? impliedResolution = null)
            : base(stream, impliedHead, impliedTail, impliedResolution)
        {
        }

        public YahooPriceHistoryCsvFile(Stream stream)
            : base(stream)
        {
        }
    }
}