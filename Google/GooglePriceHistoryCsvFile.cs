using System;
using System.IO;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Google
{
    public sealed class GooglePriceHistoryCsvFile : PriceHistoryCsvFile
    {
        public GooglePriceHistoryCsvFile(Stream stream, DateTime impliedHead = default(DateTime), DateTime impliedTail = default(DateTime), Resolution? impliedResolution = null)
            : base(stream, impliedHead, impliedTail, impliedResolution)
        {
        }

        public GooglePriceHistoryCsvFile(Stream stream)
            : base(stream)
        {
        }
    }
}