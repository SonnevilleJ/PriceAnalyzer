using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A trading account which communicates with a brokerage to perform execution of orders.
    /// </summary>
    public abstract class TradingAccount : ITradingAccount
    {
        #region Private Members

        private readonly List<IPosition> _positions = new List<IPosition>();
        private readonly IList<Tuple<Order, Task, CancellationTokenSource>> _items = new List<Tuple<Order, Task, CancellationTokenSource>>();

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
            if (!ValidateOrder(order)) throw new ArgumentOutOfRangeException("order", order, Strings.TradingAccount_Submit_Cannot_execute_this_order_);

            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var task = new Task(() => ProcessOrder(order, token), token);
            lock (_items) _items.Add(new Tuple<Order, Task, CancellationTokenSource>(order, task, cts));
            task.Start();
        }

        /// <summary>
        /// Attempts to cancel an <see cref="Order"/> before it is filled.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to attempt to cancel.</param>
        public void TryCancelOrder(Order order)
        {
            Tuple<Order, Task, CancellationTokenSource> value;
            lock (_items) value = _items.First(tuple => tuple.Item1 == order);
            var cts = value.Item3;

            cts.Cancel();
        }

        #endregion

        #region Abstract members

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        /// <param name="token"></param>
        protected abstract void ProcessOrder(Order order, CancellationToken token);

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

        protected void InvokeOrderCancelled(OrderCancelledEventArgs e)
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
            lock (_items)
            {
                var tuple = _items.First(t => t.Item1 == order);
                _items.Remove(tuple);
            }
        }

        #endregion
    }
}
