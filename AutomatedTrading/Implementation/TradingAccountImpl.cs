using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Sonneville.PriceTools.AutomatedTrading.Implementation
{
    public abstract class TradingAccountImpl : ITradingAccount
    {
        #region Private Members

        private readonly ConcurrentDictionary<IOrder, CancellationTokenSource> _tokenSources = new ConcurrentDictionary<IOrder, CancellationTokenSource>();
        private readonly BlockingCollection<IOrder> _orders = new BlockingCollection<IOrder>();

        #endregion

        #region Constructors

        protected TradingAccountImpl()
        {
            Task.Factory.StartNew(Consumer);
        }

        #endregion

        #region Implementation of TradingAccount

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
        /// <param name="order">The <see cref="IOrder"/> to execute.</param>
        public void Submit(IOrder order)
        {
            if (!ValidateOrder(order)) throw new ArgumentOutOfRangeException("order", order, Strings.TradingAccount_Submit_Cannot_execute_this_order_);

            var cts = new CancellationTokenSource();
            _tokenSources.GetOrAdd(order, cts);
            _orders.Add(order);
        }

        /// <summary>
        /// Attempts to cancel an <see cref="IOrder"/> before it is filled.
        /// </summary>
        /// <param name="order">The <see cref="IOrder"/> to attempt to cancel.</param>
        public void TryCancelOrder(IOrder order)
        {
            CancellationTokenSource cts;
            if (_tokenSources.TryRemove(order, out cts)) cts.Cancel();
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
        /// <param name="order">The <see cref="IOrder"/> to execute.</param>
        /// <param name="token"></param>
        protected abstract void ProcessOrder(IOrder order, CancellationToken token);

        #endregion

        #region Private Methods

        private void Consumer()
        {
            while (true)
            {
                var order = _orders.Take();
                var cts = _tokenSources[order];
                Task.Factory.StartNew(() => ProcessOrder(order, cts.Token));
            }
        }

        private bool ValidateOrder(IOrder order)
        {
            var commission = Features.CommissionSchedule.PriceCheck(order);
            var expectedTransaction = TransactionFactory.ConstructShareTransaction(order.OrderType, order.Ticker, DateTime.Now, order.Shares, order.Price, commission);
            return ((PortfolioImpl) Portfolio).TransactionIsValid(expectedTransaction);
        }

        private void ProcessFill(IShareTransaction transaction)
        {
            ((PortfolioImpl) Portfolio).AddTransaction(transaction);
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