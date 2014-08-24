using System;

namespace Sonneville.PriceTools.Data
{
    public interface IPriceHistoryQueryUrlBuilder
    {
        string FormPriceHistoryQueryUrl(string ticker, DateTime head, DateTime tail, Resolution resolution);
    }
}