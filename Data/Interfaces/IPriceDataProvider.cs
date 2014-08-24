using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.Data
{
    public interface IPriceDataProvider
    {
        void UpdatePriceSeries(IPriceSeries priceSeries, DateTime head, DateTime tail, Resolution resolution);

        Resolution BestResolution { get; }

        IList<IPricePeriod> DownloadPricePeriods(string ticker, DateTime head, DateTime tail, Resolution resolution);
    }
}