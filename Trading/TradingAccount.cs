﻿using System;
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

        private readonly List<IPosition> _positions = new List<IPosition>();

        private readonly IDictionary<Order, Thread> _inProcess = new Dictionary<Order, Thread>();

        #endregion

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
            var handler = OrderFilled;
            if (handler != null) handler(this, e);
            _inProcess.Remove(e.Order);
        }

        protected void InvokeOrderExpired(OrderExpiredEventArgs e)
        {
            var handler = OrderExpired;
            if (handler != null) handler(this, e);
            _inProcess.Remove(e.Order);
        }

        private void InvokeOrderCancelled(OrderCancelledEventArgs e)
        {
            var handler = OrderCancelled;
            if (handler != null) handler(this, e);
            _inProcess.Remove(e.Order);
        }
    }
}