using System;
using System.Security.Authentication;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IBrokerage
    {
        /// <summary>
        /// Logs the user into the brokerage.
        /// </summary>
        /// <exception cref="AuthenticationException">Thrown when the supplied credentials are incorrect.</exception>
        ITradingAccount LogIn(string username, string password);

        /// <summary>
        /// The ID of this brokerage.
        /// </summary>
        /// <remarks>The BrokerageID should be unique and tie back to the real world brokerage.</remarks>
        Guid BrokerageID { get; }
    }
}
