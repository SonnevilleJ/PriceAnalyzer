using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    public interface IMarketIndex
    {
        IList<string> GetTickers();
    }
}