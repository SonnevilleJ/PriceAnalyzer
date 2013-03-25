using System;
using System.Threading;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.Fidelity
{
    /// <summary>
    /// A trading account which communicates with Fidelity to perform execution of orders.
    /// </summary>
    public class FidelityTradingAccount : TradingAccountImpl
    {
        public FidelityTradingAccount() : base(Guid.Parse("54B3E2BA-93F3-4FE3-B70E-58016D13C681"))
        {
        }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="IOrder"/> to execute.</param>
        /// <param name="token"></param>
        protected override void ProcessOrder(IOrder order, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}