using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IBrokerage
    {
        ITradingAccount LogIn(string username, string password);

        Guid BrokerageID { get; }
        IList<Order> GetOpenOrders();
        void SubmitOrders(IEnumerable<Order> orders);
        IEnumerable<IShareTransaction> GetTransactions(string ticker, DateTime head, DateTime tail);
    }
}
