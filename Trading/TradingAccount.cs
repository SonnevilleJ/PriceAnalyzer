﻿using System;
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

        #endregion

        #region Constructors

        protected TradingAccount(TradingAccountFeatures features)
        {
            Features = features;
        }

        #endregion

        #region Public Interface

        /// <summary>
        /// A list of <see cref="IPosition"/>s currently held in this account.
        /// </summary>
        public ReadOnlyCollection<IPosition> Positions { get { return _positions.AsReadOnly(); } }

        public TradingAccountFeatures Features { get; private set; }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        public void Submit(Order order)
        {
            if (!ValidateOrder(order)) throw new ArgumentOutOfRangeException("order", order, "Cannot execute this order.");
            var thread = new Thread(o => ProcessOrder(order));
            lock (_inProcess) _inProcess.Add(order, thread);
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
            bool gotValue;
            lock (_inProcess) gotValue = _inProcess.TryGetValue(order, out value);
            if(gotValue)
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

        protected void InvokeOrderFilled(OrderExecutedEventArgs e)
        {
            RemoveInProcess(e.Order);
            ProcessFill(e.Transaction);
            TriggerFilled(e);
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

        private bool ValidateOrder(Order order)
        {
            lock (_positions)
            {
                var commission = Features.CommissionSchedule.PriceCheck(order);
                var expectedTransaction = TransactionFactory.Instance.CreateShareTransaction(DateTime.Now, order.OrderType, order.Ticker, order.Price, order.Shares, commission);
                var position = GetPosition(order.Ticker);
                return position.TransactionIsValid(expectedTransaction);
            }
        }

        private void ProcessFill(IShareTransaction transaction)
        {
            lock (_positions)
            {
                var ticker = transaction.Ticker;
                var position = GetPosition(ticker);
                position.AddTransaction(transaction);
            }
        }

        private IPosition GetPosition(string ticker)
        {
            lock (_positions)
            {
                var position = Positions.Where(p => p.Ticker == ticker).FirstOrDefault();
                if (position == null)
                {
                    position = PositionFactory.CreatePosition(ticker);
                    _positions.Add(position);
                }
                return position;
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

        private void RemoveInProcess(Order order)
        {
            lock (_inProcess)
            {
                _inProcess.Remove(order);
            }
        }

        #endregion
    }
}
