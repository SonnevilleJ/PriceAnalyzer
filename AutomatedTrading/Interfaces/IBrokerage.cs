using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IBrokerage
    {
        Guid BrokerageID { get; }
        IList<Order> GetOpenOrders();
        void SubmitOrders(IEnumerable<Order> orders);
        void CancelOrder(Order order);
        IEnumerable<IShareTransaction> GetTransactions(string ticker, DateTime head, DateTime tail);
    }
}
