using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// A trading account which communicates with a brokerage to perform execution of orders.
    /// </summary>
    public abstract class AsynchronousTradingAccount : TradingAccount
    {
        #region Private Members

        private readonly ConcurrentDictionary<Order, CancellationTokenSource> _tokenSources = new ConcurrentDictionary<Order, CancellationTokenSource>();
        private readonly BlockingCollection<Order> _orders = new BlockingCollection<Order>();
        private readonly CancellationTokenSource _consumer = new CancellationTokenSource();

        #endregion

        #region Constructors

        protected AsynchronousTradingAccount()
        {
            Task.Factory.StartNew(() => Consumer(_consumer.Token));
        }

        #endregion

        #region Public Interface

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        public override void Submit(Order order)
        {
            if (!ValidateOrder(order)) throw new ArgumentOutOfRangeException("order", order, Strings.TradingAccount_Submit_Cannot_execute_this_order_);
            _orders.Add(order);
        }

        /// <summary>
        /// Attempts to cancel an <see cref="Order"/> before it is filled.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to attempt to cancel.</param>
        public override void TryCancelOrder(Order order)
        {
            CancellationTokenSource cts;
            if (_tokenSources.TryRemove(order, out cts)) cts.Cancel();
        }

        /// <summary>
        /// Blocks the calling thread until all submitted orders are filled, cancelled, or expired.
        /// </summary>
        public void Stop()
        {
            _consumer.Cancel();
        }

        #endregion

        private void Consumer(CancellationToken cancellationToken)
        {
            while (true)
            {
                var order = _orders.Take(cancellationToken);
                if (cancellationToken.IsCancellationRequested) return;
                
                var cts = new CancellationTokenSource();
                _tokenSources.GetOrAdd(order, cts);
                Task.Factory.StartNew(() => ProcessOrder(order, cts.Token));
            }
        }
    }
}
