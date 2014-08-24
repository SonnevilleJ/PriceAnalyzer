using System;
using System.IO;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Yahoo
{
    public sealed class YahooPriceHistoryCsvFileFactory : IPriceHistoryCsvFileFactory
    {
        //
        // Yahoo has many features beyond price history - i.e. fundamental indicators.
        // See http://www.codeproject.com/KB/aspnet/StockQuote.aspx for details.
        //
        // Also, a REST API is now available: http://www.jarloo.com/yahoo_finance/
        //

        public PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail, Resolution? impliedResolution = null)
        {
            return new YahooPriceHistoryCsvFile(stream, head, tail, impliedResolution);
        }
    }
}