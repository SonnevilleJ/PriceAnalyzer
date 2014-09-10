using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class TradeManager : ITradeManager
    {
        private IBrokerage _brokerage;
        private IList<Order> _openOrders = new List<Order>();

        public TradeManager(IBrokerage brokerage)
        {
            _brokerage = brokerage;
        }

        public void Submit(Order order)
        {
            var submitOrder = new List<Order> {order};
            _brokerage.SubmitOrders(submitOrder);
            _openOrders.Add(order);
        }

        public IEnumerable<Order> GetOpenOrders()
        {
            return new ReadOnlyCollection<Order>(_openOrders);
        }

        public void CancelOrder(Order order)
        {
            if (! _openOrders.Contains(order)) return;
            _brokerage.CancelOrder(order);
            _openOrders.Remove(order);
        }

        public void RefreshFromBrokerage()
        {
            _openOrders = _brokerage.GetOpenOrders();
        }
    }
}