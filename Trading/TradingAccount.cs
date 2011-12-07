using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A trading account which communicates with a brokerage to perform execution of orders.
    /// </summary>
    public abstract class TradingAccount
    {
        #region Private Members

        private readonly List<IPosition> _positions;

        private delegate void ProcessDelegate(Order order);

        private readonly IDictionary<Order, Thread> _inProcess = new Dictionary<Order, Thread>();

        #endregion

        protected TradingAccount()
        {
            _positions = new List<IPosition>();
        }

        /// <summary>
        /// A list of <see cref="IPosition"/>s currently held in this account.
        /// </summary>
        public ReadOnlyCollection<IPosition> Positions { get { return _positions.AsReadOnly(); } }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        public void Submit(Order order)
        {
            var thread = new Thread(o => ProcessOrder(order));
            _inProcess.Add(order, thread);
            thread.Start();
        }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        protected abstract void ProcessOrder(Order order);

        public void TryCancelOrder(Order order)
        {
            Thread value;
            if(_inProcess.TryGetValue(order, out value))
            {
                value.Abort();
                var cancelled = DateTime.Now;
                _inProcess.Remove(order);
                InvokeOrderCancelled(new OrderCancelledEventArgs(cancelled, order));
            }
        }

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
            EventHandler<OrderExecutedEventArgs> handler = OrderFilled;
            if (handler != null) handler(this, e);
        }

        public void InvokeOrderExpired(OrderExpiredEventArgs e)
        {
            EventHandler<OrderExpiredEventArgs> handler = OrderExpired;
            if (handler != null) handler(this, e);
        }

        public void InvokeOrderCancelled(OrderCancelledEventArgs e)
        {
            EventHandler<OrderCancelledEventArgs> handler = OrderCancelled;
            if (handler != null) handler(this, e);
        }
    }
}
