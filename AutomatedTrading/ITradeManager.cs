using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface ITradeManager
    {
        void Submit(Order order);
        IEnumerable<Order> GetOpenOrders();
        void CancelOrder(Order order);
        void RefreshFromBrokerage();
    }
}