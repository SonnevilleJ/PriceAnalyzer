using System;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// Contains event data for an Order expiration.
    /// </summary>
    public sealed class OrderExpiredEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new OrderExpiredEventArgs object.
        /// </summary>
        /// <param name="order">The Order which was executed.</param>
        public OrderExpiredEventArgs(IOrder order)
        {
            Order = order;
        }

        /// <summary>
        /// The DateTime at which the order expired.
        /// </summary>
        public DateTime Expired { get { return Order.Expiration; } }

        /// <summary>
        /// The Order which expired.
        /// </summary>
        public IOrder Order { get; private set; }
    }
}