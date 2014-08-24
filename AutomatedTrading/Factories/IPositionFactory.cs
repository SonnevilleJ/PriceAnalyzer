using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IPositionFactory
    {
        Position ConstructPosition(string ticker, params ShareTransaction[] transactions);

        Position ConstructPosition(string ticker, IEnumerable<ShareTransaction> transactions);
    }
}