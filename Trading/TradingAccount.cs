using System;

namespace Sonneville.PriceTools.Trading
{
    public abstract class TradingAccount : ITradingAccount
    {
        #region Implementation of ITradingAccount

        /// <summary>
        /// The portfolio of transactions recorded by this TradingAccount.
        /// </summary>
        public IPortfolio Portfolio { get; set; }

        /// <summary>
        /// Gets the list of features supported by this TradingAccount.
        /// </summary>
        public TradingAccountFeatures Features { get; set; }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        public virtual void Submit(Order order)
        {
            if (!ValidateOrder(order)) throw new ArgumentOutOfRangeException("order", order, Strings.TradingAccount_Submit_Cannot_execute_this_order_);

            ProcessOrder(order);
        }

        /// <summary>
        /// Attempts to cancel an <see cref="Order"/> before it is filled.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to attempt to cancel.</param>
        public virtual void TryCancelOrder(Order order)
        {
            throw new NotSupportedException("Order cancellation is not supported by the synchronous TradingAccount.");
        }

        /// <summary>
        /// Blocks the calling thread until all submitted orders are filled, cancelled, or expired.
        /// </summary>
        public virtual void WaitAll()
        {
        }

        #endregion

        #region Events and Invokers

        /// <summary>
        /// Triggered when an order has been filled.
        /// </summary>
        public event EventHandler<OrderExecutedEventArgs> OrderFilled;

        /// <summary>
        /// Triggered when an order has expired.
        /// </summary>
        public event EventHandler<OrderExpiredEventArgs> OrderExpired;

        /// <summary>
        /// Triggered when an order has been cancelled.
        /// </summary>
        public event EventHandler<OrderCancelledEventArgs> OrderCancelled;

        protected void InvokeOrderFilled(OrderExecutedEventArgs e)
        {
            ProcessFill(e.Transaction);
            TriggerFilled(e);
        }

        protected void InvokeOrderExpired(OrderExpiredEventArgs e)
        {
            TriggerExpired(e);
        }

        protected void InvokeOrderCancelled(OrderCancelledEventArgs e)
        {
            TriggerCancelled(e);
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        protected abstract void ProcessOrder(Order order);

        #endregion

        #region Private Methods

        private bool ValidateOrder(Order order)
        {
            var commission = Features.CommissionSchedule.PriceCheck(order);
            var expectedTransaction = TransactionFactory.Instance.CreateShareTransaction(DateTime.Now, order.OrderType, order.Ticker, order.Price, order.Shares, commission);
            return Portfolio.TransactionIsValid(expectedTransaction);
        }

        private void ProcessFill(IShareTransaction transaction)
        {
            Portfolio.AddTransaction(transaction);
        }

        private void TriggerFilled(OrderExecutedEventArgs e)
        {
            var handler = OrderFilled;
            if (handler != null) handler(this, e);
        }

        private void TriggerExpired(OrderExpiredEventArgs e)
        {
            var handler = OrderExpired;
            if (handler != null) handler(this, e);
        }

        private void TriggerCancelled(OrderCancelledEventArgs e)
        {
            var handler = OrderCancelled;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}