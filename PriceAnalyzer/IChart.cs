using System.Collections.Generic;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public interface IChart
    {
        void DrawPricePeriods(IList<IPricePeriod> pricePeriods);
    }
}