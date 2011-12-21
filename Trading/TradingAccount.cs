using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A trading account which communicates with a brokerage to perform execution of orders.
    /// </summary>
    public abstract class TradingAccount
    {
        #region Private Members

        private readonly List<IPosition> _positions = new List<IPosition>();
        private readonly IDictionary<Order, Thread> _inProcess = new Dictionary<Order, Thread>();
        private readonly object _lock = new object();

        #endregion

        #region Public Interface

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
        /// Attempts to cancel an <see cref="Order"/> before it is filled.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to attempt to cancel.</param>
        public bool TryCancelOrder(Order order)
        {
            Thread value;
            var result = false;
            if(_inProcess.TryGetValue(order, out value))
            {
                EventHandler<OrderCancelledEventArgs> handler = (sender, args) => result = order == args.Order;
                try
                {
                    OrderCancelled += handler;
                    value.Abort();
                    InvokeOrderCancelled(new OrderCancelledEventArgs(DateTime.Now, order));
                }
                finally
                {
                    OrderCancelled -= handler;
                }
            }
            return result;
        }

        #endregion

        #region Abstract members

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        protected abstract void ProcessOrder(Order order);

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

        /// <summary>
        /// Triggered after an order has been filled and the resulting <see cref="IShareTransaction"/> has been processed.
        /// </summary>
        public event EventHandler<OrderExecutedEventArgs> TransactionProcessed;

        protected void InvokeOrderFilled(OrderExecutedEventArgs e)
        {
            RemoveInProcess(e.Order);
            TriggerFilled(e);

            ProcessFill(e);
            TriggerProcessed(e);
        }

        protected void InvokeOrderExpired(OrderExpiredEventArgs e)
        {
            RemoveInProcess(e.Order);
            TriggerExpired(e);
        }

        private void InvokeOrderCancelled(OrderCancelledEventArgs e)
        {
            RemoveInProcess(e.Order);
            TriggerCancelled(e);
        }

        #endregion

        #region Private Methods

        private void ProcessFill(OrderExecutedEventArgs e)
        {
            lock (_lock)
            {
                var transaction = e.Transaction;
                var ticker = transaction.Ticker;
                var position = Positions.Where(p => p.Ticker == ticker).FirstOrDefault();
                if (position == null)
                {
                    position = PositionFactory.CreatePosition(ticker);
                    _positions.Add(position);
                }
                position.AddTransaction(transaction);
            }
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

        private void TriggerProcessed(OrderExecutedEventArgs e)
        {
            var handler = TransactionProcessed;
            if (handler != null) handler(this, e);
        }

        private void RemoveInProcess(Order order)
        {
            _inProcess.Remove(order);
        }

        #endregion
    }
}
