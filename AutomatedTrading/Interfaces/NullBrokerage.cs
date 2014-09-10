using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class NullBrokerage : IBrokerage
    {
        public ITradingAccount LogIn(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Guid BrokerageID { get; private set; }
        public IList<Order> GetOpenOrders()
        {
            throw new NotImplementedException();
        }

        public void SubmitOrders(IEnumerable<Order> orders)
        {
        }

        public void CancelOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IShareTransaction> GetTransactions(string ticker, DateTime head, DateTime tail)
        {
            throw new NotImplementedException();
        }
    }
}