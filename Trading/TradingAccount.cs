using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Remoting.Messaging;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A trading account which communicates with a brokerage to perform execution of orders.
    /// </summary>
    public abstract class TradingAccount
    {
        private readonly List<IPosition> _positions;
        private delegate void OrderDelegate(Order order);

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
        public void SubmitOrder(Order order)
        {
            var executor = new OrderDelegate(SubmitOrderImpl);
            var result = executor.BeginInvoke(order, OrderFilledCallback, null);
        }

        private static void OrderFilledCallback(IAsyncResult result)
        {
            var ar = (AsyncResult) result;
            var del = (OrderDelegate) ar.AsyncDelegate;

            del.EndInvoke(result);
        }

        /// <summary>
        /// Triggered when an order has been filled.
        /// </summary>
        public event EventHandler<OrderExecutedEventArgs> OrderFilled;

        protected void InvokeOrderFilled(OrderExecutedEventArgs e)
        {
            EventHandler<OrderExecutedEventArgs> handler = OrderFilled;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Submits an order for execution by the brokerage.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> to execute.</param>
        protected abstract void SubmitOrderImpl(Order order);
    }
}
