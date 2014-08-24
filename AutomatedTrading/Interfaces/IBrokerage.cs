using System;
using System.Security.Authentication;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IBrokerage
    {
        ITradingAccount LogIn(string username, string password);

        Guid BrokerageID { get; }
    }
}
