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
        private readonly ITransactionFactory _factory;
        private readonly List<IShareTransaction> _transactions;
        private readonly int _maxOrderAge;

        public SimulatedBrokerage(int maxOrderAge = 1)
        {
            _openOrders = new List<Order>();
            _canceledOrders = new List<Order>();
            _factory = new TransactionFactory();
            _transactions = new List<IShareTransaction>();
            _maxOrderAge = maxOrderAge;
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

        public IEnumerable<IShareTransaction> GetTransactions(DateTime head, DateTime tail)
        {

            for (var i = _openOrders.Count -1; i >= 0; i--)
            {
                var order = _openOrders[i];
                if (order.Issued <= tail.AddDays(-_maxOrderAge))
                {
                    _transactions.Add(_factory.ConstructBuy(order.Ticker, order.Issued, order.Shares, order.Price));
                    _openOrders.Remove(order);
                }
            }

            return _transactions;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _openOrders.Concat(_canceledOrders);
        }
    }
}