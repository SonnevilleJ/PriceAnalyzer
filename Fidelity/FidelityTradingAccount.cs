using System;
using System.Threading;
using Sonneville.PriceTools.AutomatedTrading;

namespace Sonneville.PriceTools.Fidelity
{
    /// <summary>
    /// A trading account which communicates with Fidelity to perform execution of orders.
    /// </summary>
    public class FidelityTradingAccount : TradingAccount
    {
        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        /// <param name="token"></param>
        protected override void ProcessOrder(Order order, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}