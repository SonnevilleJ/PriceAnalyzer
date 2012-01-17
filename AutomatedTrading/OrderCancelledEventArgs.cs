using System;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// Contains event data for an Order cancellation.
    /// </summary>
    public sealed class OrderCancelledEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new OrderCancelledEventArgs object.
        /// </summary>
        /// <param name="cancelled">The DateTime at which the Order was cancelled.</param>
        /// <param name="order">The Order which was executed.</param>
        public OrderCancelledEventArgs(DateTime cancelled, Order order)
        {
            Cancelled = cancelled;
            Order = order;
        }

        /// <summary>
        /// The DateTime at which the order was cancelled.
        /// </summary>
        public DateTime Cancelled { get; private set; }

        /// <summary>
        /// The Order which was cancelled.
        /// </summary>
        public Order Order { get; private set; }
    }
}