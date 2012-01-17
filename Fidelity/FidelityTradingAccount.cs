using System;
using Sonneville.PriceTools.AutomatedTrading;

namespace Sonneville.PriceTools.Fidelity
{
    /// <summary>
    /// A trading account which communicates with Fidelity to perform execution of orders.
    /// </summary>
    public class FidelityTradingAccount : SynchronousTradingAccount
    {
        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        protected override void ProcessOrder(IOrder order)
        {
            throw new NotImplementedException();
        }
    }
}