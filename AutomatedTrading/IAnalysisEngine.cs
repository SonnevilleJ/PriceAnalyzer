using System;
using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IAnalysisEngine
    {
        IEnumerable<Order> DetermineOrdersFor(IPortfolio portfolio, IPriceSeries stockBeingConsidered, DateTime startDate, DateTime endDate,
            IList<Order> openOrders);

    }
}