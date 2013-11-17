using System;
using System.Collections.Concurrent;
using System.Threading;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading.Implementation
{
    public abstract class TradingAccountImpl : ITradingAccount
    {
        private readonly ConcurrentDictionary<Order, CancellationTokenSource> _tokenSources = new ConcurrentDictionary<Order, CancellationTokenSource>();

        protected TradingAccountImpl(Guid brokerageGuid, string accountNumber)
        {
            AccountNumber = accountNumber;
            TransactionFactory = new TransactionFactory(brokerageGuid);
        }

        /// <summary>
        /// Gets the <see cref="ITransactionFactory"/> associated with the user's brokerage account.
        /// </summary>
        /// <value>The <see cref="ITransactionFactory"/> associated with the user's brokerage account.</value>
        public ITransactionFactory TransactionFactory { get; private set; }

        /// <summary>
        /// The portfolio of transactions recorded by this TradingAccount.
        /// </summary>
        public Portfolio Portfolio { get; set; }

        /// <summary>
        /// The account number identifying this account at the <see cref="IBrokerage"/>.
        /// </summary>
        public string AccountNumber { get; private set; }

        /// <summary>
        /// Gets the list of features supported by this TradingAccount.
        /// </summary>
        public TradingAccountFeatures Features { get; set; }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        public void Submit(Order order)
        {
            if (!ValidateOrder(order)) throw new ArgumentOutOfRangeException("order", order, Strings.TradingAccount_Submit_Cannot_execute_this_order_);

            var cts = new CancellationTokenSource();
            _tokenSources.GetOrAdd(order, cts);
            ProcessOrder(order, cts.Token);
        }

        /// <summary>
        /// Attempts to cancel an <see cref="Order"/> before it is filled.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to attempt to cancel.</param>
        public void TryCancelOrder(Order order)
        {
            CancellationTokenSource cts;
            if (_tokenSources.TryRemove(order, out cts)) cts.Cancel();
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

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        /// <param name="token"></param>
        protected abstract void ProcessOrder(Order order, CancellationToken token);

        private bool ValidateOrder(Order order)
        {
            var commission = Features.CommissionSchedule.PriceCheck(order);
            var expectedTransaction = TransactionFactory.ConstructShareTransaction(order.OrderType, order.Ticker, DateTime.Now, order.Shares, order.Price, commission);
            return Portfolio.TransactionIsValid(expectedTransaction);
        }

        private void ProcessFill(ShareTransaction transaction)
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
    }
}