using System;
using System.Threading;

namespace Sonneville.PriceTools.Trading.Fidelity
{
    /// <summary>
    /// A trading account which communicates with Fidelity to perform execution of orders.
    /// </summary>
    public class FidelityTradingAccount : SynchronousTradingAccount
    {
        public FidelityTradingAccount(TradingAccountFeatures features)
            : base(features)
        {
        }

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