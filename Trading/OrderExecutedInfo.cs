using System;

namespace Sonneville.PriceTools.Trading
{
    public sealed class OrderExecutedInfo : EventArgs
    {
        /// <summary>
        /// Constructs a new OrderExecutedInfo event args.
        /// </summary>
        /// <param name="executed">The DateTime at which the order was executed.</param>
        /// <param name="order">The Order which was executed.</param>
        /// <param name="transaction">The resulting IShareTransaction from the execution of the order.</param>
        public OrderExecutedInfo(DateTime executed, Order order, IShareTransaction transaction)
        {
            Executed = executed;
            Order = order;
            Transaction = transaction;
        }

        /// <summary>
        /// The DateTime at which the order was executed.
        /// </summary>
        public DateTime Executed { get; private set; }

        /// <summary>
        /// The Order which was executed.
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// The resulting IShareTransaction from the execution of the order.
        /// </summary>
        public IShareTransaction Transaction { get; private set; }
    }
}