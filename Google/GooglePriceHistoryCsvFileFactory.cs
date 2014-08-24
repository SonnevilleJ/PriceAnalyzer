using System;
using System.IO;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Google
{
    public sealed class GooglePriceHistoryCsvFileFactory : IPriceHistoryCsvFileFactory
    {
        public PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail, Resolution? impliedResolution = null)
        {
            return new GooglePriceHistoryCsvFile(stream, head, tail, impliedResolution);
        }
    }
}