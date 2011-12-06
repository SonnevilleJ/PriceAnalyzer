using System;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A trading strategy which issues orders to buy and sell a security.
    /// </summary>
    public abstract class TradingStrategy
    {
        protected TradingStrategy(IPriceSeries priceSeries)
        {
            PriceSeries = priceSeries;
        }

        public IPriceSeries PriceSeries { get; private set; }

        protected abstract void ProcessPeriod(int index);

        #region Signal Events

        /// <summary>
        /// Triggered when a new order is signaled by the TradingStrategy.
        /// </summary>
        public event EventHandler<Order> SubmitOrder;

        /// <summary>
        /// Triggered when the TradingStrategy signals that all open orders should be cancelled.
        /// </summary>
        public event EventHandler CancelAllOrders;

        #endregion
    }
}
