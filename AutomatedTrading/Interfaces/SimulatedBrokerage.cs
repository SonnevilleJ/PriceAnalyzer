using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class SimulatedBrokerage : IBrokerage
    {
        private readonly List<Order> _openOrders;
        private readonly List<Order> _canceledOrders; 

        public SimulatedBrokerage()
        {
            _openOrders = new List<Order>();
            _canceledOrders = new List<Order>();
        }

        public Guid BrokerageID { get; private set; }

        public IList<Order> GetOpenOrders()
        {
            return _openOrders;
        }

        public void SubmitOrders(IEnumerable<Order> orders)
        {
            _openOrders.AddRange(orders);
        }

        public void CancelOrder(Order order)
        {
            if (_openOrders.Contains(order))
            {
                _canceledOrders.Add(order);
                _openOrders.Remove(order);
            }
        }

        public IEnumerable<IShareTransaction> GetTransactions(string ticker, DateTime head, DateTime tail)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _openOrders.Concat(_canceledOrders);
        }
    }
}